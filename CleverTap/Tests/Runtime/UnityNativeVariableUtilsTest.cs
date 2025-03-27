using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Native;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

public class UnityNativeVariableUtilsTest
{
    [TestCase("a", new string[] { "a" })]
    [TestCase("a.b.c", new string[] { "a", "b", "c" })]
    public void GetNameComponents(string name, string[] expected)
    {
        Assert.AreEqual(expected, UnityNativeVariableUtils.GetNameComponents(name));
    }

    [Test]
    public void CopyDictionary_With_Nested_Dictionaries()
    {
        var input = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 42 },
            { "key3", new Dictionary<string, object>
                {
                    { "nestedKey1", "nestedValue1" },
                    { "nestedKey2", 100 }
                }
            }
        };

        var copiedDict = UnityNativeVariableUtils.CopyDictionary(input);

        Assert.AreEqual(input.Count, copiedDict.Count);
        Assert.AreEqual(input["key1"], copiedDict["key1"]);
        Assert.AreEqual(input["key2"], copiedDict["key2"]);

        var originalNested = (Dictionary<string, object>)input["key3"];
        var copiedNested = (Dictionary<string, object>)copiedDict["key3"];

        Assert.AreEqual(originalNested.Count, copiedNested.Count);
        Assert.AreEqual(originalNested["nestedKey1"], copiedNested["nestedKey1"]);
        Assert.AreEqual(originalNested["nestedKey2"], copiedNested["nestedKey2"]);

        Assert.AreNotSame(input, copiedDict);
        Assert.AreNotSame(originalNested, copiedNested);
    }

    #region MergeHelper

    [Test]
    public void MergeHelper_With_Null_Diff()
    {
        var vars = new Dictionary<string, object> { { "a", 1 }, { "b", 2 } };

        var result = UnityNativeVariableUtils.MergeHelper(vars, null);
        Assert.AreEqual(vars, result);
    }

    [TestCase("defaultValue", "newValue")]
    [TestCase(null, "newValue")]
    [TestCase(199, 1234.56)]
    [TestCase("a", 1)]
    public void MergeHelper_With_Primitives(object vars, object diff)
    {
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        Assert.AreEqual(diff, result);
    }

    [Test]
    public void MergeHelper_With_Booleans()
    {
        bool vars = true;
        bool diff = false;
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        Assert.True(result is bool);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void MergeHelper_With_Dictionary_And_Primitive()
    {
        var vars = new Dictionary<string, object> { { "key1", "value1" }, { "key2", "value2" } };
        var diff = "diff";
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        Assert.AreEqual(diff, result);
    }

    [Test]
    public void MergeHelper_With_Dictionaries()
    {
        var vars = new Dictionary<string, object> { { "a", 1 }, { "b", 2 } };
        var diff = new Dictionary<string, object> { { "b", 42 }, { "c", 3 } };
        var expected = new Dictionary<string, object> { { "a", 1 }, { "b", 42 }, { "c", 3 } };
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        CollectionAssert.AreEqual(expected, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_With_Nested_Dictionaries_Add_And_Merge()
    {
        var vars = new Dictionary<string, object>
        {
            { "A", new Dictionary<string, object> { { "a", 1 }, { "b", 2 } } },
            { "B", new Dictionary<string, object> { { "x", 24 }, { "y", 25 } } },
            { "C", new Dictionary<string, object> { { "x", 2 } } }
        };

        var diff = new Dictionary<string, object>
        {
            { "A", new Dictionary<string, object> { { "b", -2 }, { "c", -3 } } },
            { "B", new Dictionary<string, object> { { "x", -24 } } },
            { "C", new Dictionary<string, object> { { "y", "value" } } },
            { "D", new Dictionary<string, object> { { "d", "d" } } }
        };

        var expected = new Dictionary<string, object>
        {
            { "A", new Dictionary<string, object> { { "a", 1 }, { "b", -2 }, { "c", -3 } } },
            { "B", new Dictionary<string, object> { { "x", -24 }, { "y", 25 } } },
            { "C", new Dictionary<string, object> { { "x", 2 }, { "y", "value" } } },
            { "D", new Dictionary<string, object> { { "d", "d" } } }
        };

        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        CollectionAssert.AreEqual(expected, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_Merge_With_Empty()
    {
        var vars = new Dictionary<string, object>
        {
            { "A", 1 },
            { "B", new Dictionary<string, object> { { "x", 24 }, { "y", 25 } } }
        };

        var diff = new Dictionary<string, object>();

        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        CollectionAssert.AreEqual(vars, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_Merge_Empty()
    {
        var vars = new Dictionary<string, object>();

        var diff = new Dictionary<string, object>
        {
            { "A", 1 },
            { "B", new Dictionary<string, object> { { "x", 24 }, { "y", 25 } } }
        };

        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        CollectionAssert.AreEqual(diff, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_Merge_Dictionaries_Include_Values()
    {
        var vars = new Dictionary<string, object>
        {
            { "abc", "qwe" },
            { "nested", new Dictionary<string, object> { { "abc", "qwe" }, { "1", 123 } } },
            { "nested2", new Dictionary<string, object> { { "a", "a" }, { "b", null }, { "c", 4444 } } }
        };

        var diff = new Dictionary<string, object>
        {
            // No "abc" top key-value
            // Change "abc" value, new "qwerty" key-value, no "1" key-value
            { "nested", new Dictionary<string, object> { { "abc", "abc" }, { "qwerty", "qwerty" } } },
            // Change "a" value, new "d" and "e" key-value, no "b" and "c" key-value
            { "nested2", new Dictionary<string, object> { { "a", "b" }, { "d", 111 }, { "e", "e" } } }
        };

        var expected = new Dictionary<string, object>
        {
            { "abc", "qwe" },
            { "nested", new Dictionary<string, object> { { "abc", "abc" }, { "1", 123 }, { "qwerty", "qwerty" } } },
            { "nested2", new Dictionary<string, object> { { "a", "b" }, { "b", null }, { "c", 4444 }, { "d", 111 }, { "e", "e" } } }
        };

        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        CollectionAssert.AreEqual(expected, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_With_Different_Types()
    {
        var vars = new Dictionary<string, object>
        {
            { "a", 20 },
            { "b", "string" },
            { "c", true },
            { "d", 4.3 }
        };

        var diffs = new Dictionary<string, object>
        {
            { "a", 21 },
            { "c", false },
            { "d", 4.8 }
        };

        var expected = new Dictionary<string, object>
        {
            { "a", 21 },
            { "b", "string" },
            { "c", false },
            { "d", 4.8 }
        };

        var result = UnityNativeVariableUtils.MergeHelper(vars, diffs);
        CollectionAssert.AreEqual(expected, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_With_Different_Types_And_Nested_Dictionaries()
    {
        var vars = new Dictionary<string, object>
        {
            { "k2", new Dictionary<string, object> { { "m1", 1 }, { "m2", "hello" }, { "m3", false } } },
            { "k3", new Dictionary<string, object> { { "m1", 1 }, { "m2", "hello" }, { "m3", false } } },
            { "k4", new Dictionary<string, object> { { "m1", 1 }, { "m2", "hello" }, { "m3", false } } },
            { "k5", 4.3}
        };

        var diffs = new Dictionary<string, object>
        {
            // No change
            { "k2", new Dictionary<string, object> { { "m1", 1 }, { "m2", "hello" }, { "m3", false } } },
            // Change values
            { "k3", new Dictionary<string, object> { { "m1", 2 }, { "m2", "bye" }, { "m3", true } } },
            // Change values and add elements
            { "k4", new Dictionary<string, object> { { "m1", 2 }, { "m3", true }, { "m4", "new key" } } },
        };

        var expected = new Dictionary<string, object>
        {
            { "k2", new Dictionary<string, object> { { "m1", 1 }, { "m2", "hello" }, { "m3", false } } },
            { "k3", new Dictionary<string, object> { { "m1", 2 }, { "m2", "bye" }, { "m3", true } } },
            { "k4", new Dictionary<string, object> { { "m1", 2 }, { "m2", "hello" }, { "m3", true }, { "m4", "new key" } } },
            { "k5", 4.3}
        };

        var result = UnityNativeVariableUtils.MergeHelper(vars, diffs);
        CollectionAssert.AreEqual(expected, result as IEnumerable);
    }

    [Test]
    public void MergeHelper_With_Array()
    {
        var vars = new[] { 1, 2, 3, 4 };
        var diffs = new[] { 1, 2, 3, 4 };
        // MergeHelper does not support merging arrays
        Assert.IsNull(UnityNativeVariableUtils.MergeHelper(vars, diffs));
    }

    [Test]
    public void MergeHelper_With_Dictionary_Array()
    {
        var vars = new Dictionary<string, object> { { "arr", new[] { 1, 2, 3, 4 } } };
        var diffs = new Dictionary<string, object> { { "arr", new[] { 1, 2, 3, 4 } } };
        // MergeHelper does not support merging arrays
        var expected = new Dictionary<string, object> { { "arr", null } };
        var result = UnityNativeVariableUtils.MergeHelper(vars, diffs);
        Assert.AreEqual(expected, result);
    }

    #endregion

    #region ConvertDictionaryToNestedDictionaries

    [Test]
    public void ConvertDictionaryToNestedDictionaries()
    {
        var input = new Dictionary<string, object> { { "a", 1 } };

        var result = UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input);
        CollectionAssert.AreEqual(input, result);
    }

    [Test]
    public void ConvertDictionaryToNestedDictionaries_With_Group()
    {
        var input = new Dictionary<string, object> { { "group.a", 1 } };
        var expected = new Dictionary<string, object>
        {
            { "group", new Dictionary<string, object> { { "a", 1 } } }
        };
        CollectionAssert.AreEqual(expected, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    [Test]
    public void ConvertDictionaryToNestedDictionaries_With_Two_Groups()
    {
        var input = new Dictionary<string, object> { { "group1.group2.a", 1 } };
        var expected = new Dictionary<string, object>
        {
            {
                "group1", new Dictionary<string, object>
                {
                    {
                        "group2", new Dictionary<string, object>
                        {
                            { "a", 1 }
                        }
                    }
                }
            }
        };
        CollectionAssert.AreEqual(expected, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    [Test]
    public void ConvertDictionaryToNestedDictionaries_With_Variables()
    {
        var input = new Dictionary<string, object>
        {
            { "group1.group11.v1", 1 },
            { "group1.group11.v2", "val" },
            { "v3", "v3" },
            { "group2.v4", 4 },
            { "group2.v5", true }
        };

        var expected = new Dictionary<string, object>
        {
            { "group1", new Dictionary<string, object> { { "group11", new Dictionary<string, object> { { "v1", 1 }, { "v2", "val" } } } } },
            { "v3", "v3" },
            { "group2", new Dictionary<string, object> { { "v4", 4 }, { "v5", true } } }
        };

        Assert.AreEqual(expected, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    [Test]
    public void ConvertDictionaryToNestedDictionaries_With_InvalidData()
    {
        var input = new Dictionary<string, object>
        {
            { "a.b.c.d", "d value" },
            { "a.b.c", "c value" },
            { "a.e", "e value" },
            { "a.b", "b value" }
        };

        var expected = new Dictionary<string, object>();
        var a = new Dictionary<string, object> { { "b", "b value" }, { "e", "e value" } };
        expected.Add("a", a);
        CollectionAssert.AreEqual(expected, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    [Test]
    public void ConvertDictionaryToNestedDictionaries_With_Empty()
    {
        var input = new Dictionary<string, object>();
        CollectionAssert.AreEqual(input, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    #endregion

    #region GetFlatVarsPayload

    [Test]
    public void GetFlatVarsPayload()
    {
        var _cache = new UnityNativeVarCache();
        var var1 = new UnityNativeVar<string>("var1", CleverTapVariableKind.STRING, "str", _cache);
        var var2 = new UnityNativeVar<int>("var2", CleverTapVariableKind.INT, 1, _cache);
        var var3 = new UnityNativeVar<Dictionary<string, object>>("group", CleverTapVariableKind.DICTIONARY,
            new Dictionary<string, object> { { "var1", "value" }, { "var2", 2 }, { "var3", 99.95} }, _cache);
        var var4 = new UnityNativeVar<Dictionary<string, string>>("group.group2", CleverTapVariableKind.DICTIONARY,
            new Dictionary<string, string> { { "var4", "default" } }, _cache);
        var var5 = new UnityNativeVar<Dictionary<string, object>>("group1", CleverTapVariableKind.DICTIONARY,
            new Dictionary<string, object> { { "group2", new Dictionary<string, object> { { "var4", 4 } } } }, _cache);
        var var6 = new UnityNativeVar<double>("var6", CleverTapVariableKind.FLOAT, 1.99, _cache);

        var vars = new Dictionary<string, IVar>
        {
            { var1.Name, var1 },
            { var2.Name, var2 },
            { var3.Name, var3 },
            { var4.Name, var4 },
            { var5.Name, var5 },
            { var6.Name, var6 }
        };

        var expected = new Dictionary<string, object>();
        expected.Add("type", "varsPayload");
        expected.Add("vars", new Dictionary<string, object>()
        {
            {
                "var1", new Dictionary<string, object>()
                {
                    { "type", "string" },
                    { "defaultValue", "str" }
                }
            },
            {
                "var2", new Dictionary<string, object>()
                {
                    { "type", "number" },
                    { "defaultValue", 1 }
                }
            },
            {
                "group.var1", new Dictionary<string, object>()
                {
                    { "type", "string" },
                    { "defaultValue", "value" }
                }
            },
            {
                "group.var2", new Dictionary<string, object>()
                {
                    { "type", "number" },
                    { "defaultValue", 2 }
                }
            },
            {
                "group.var3", new Dictionary<string, object>()
                {
                    { "type", "number" },
                    { "defaultValue", 99.95 }
                }
            },
            {
                "group.group2.var4", new Dictionary<string, object>()
                {
                    { "type", "string" },
                    { "defaultValue", "default" }
                }
            },
            {
                "group1.group2.var4", new Dictionary<string, object>()
                {
                    { "type", "number" },
                    { "defaultValue", 4 }
                }
            },
            {
                "var6", new Dictionary<string, object>()
                {
                    { "type", "number" },
                    { "defaultValue", 1.99 }
                }
            }
        });

        var actual = UnityNativeVariableUtils.GetFlatVarsPayload(vars);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFlatVarsPayload_Empty()
    {
        var vars = new Dictionary<string, IVar>();

        var expected = new Dictionary<string, object>();
        expected.Add("type", "varsPayload");
        expected.Add("vars", new Dictionary<string, object>());

        var actual = UnityNativeVariableUtils.GetFlatVarsPayload(vars);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFlatVarsPayload_Null()
    {
        // If called with null, an Error should be logged
        LogAssert.Expect(LogType.Error, new Regex("GetFlatVarsPayload: vars are null." + ".*"));

        Dictionary<string, object> actual = null;
        Assert.DoesNotThrow(() => actual = UnityNativeVariableUtils.GetFlatVarsPayload(null));

        // It should still return empty vars payload
        var expected = new Dictionary<string, object>();
        expected.Add("type", "varsPayload");
        expected.Add("vars", new Dictionary<string, object>());
        CollectionAssert.AreEqual(expected, actual);
    }

    #endregion

    #region ConvertNestedDictionariesToFlat

    [Test]
    public void ConvertNestedDictionariesToFlat_With_Flat()
    {
        var input = new Dictionary<string, object>
        {
            { "a", 1 }
        };
        var output = new Dictionary<string, object>();

        UnityNativeVariableUtils.ConvertNestedDictionariesToFlat("", input, output);
        var expected = new Dictionary<string, object>
        {
            { "a", 1 }
        };
        CollectionAssert.AreEqual(expected, output);
    }

    [Test]
    public void ConvertNestedDictionariesToFlat_With_Groups()
    {
        var input = new Dictionary<string, object>
        {
            { "a", 1},
            { "group", new Dictionary<string, object>
                {
                    { "a", "value" }
                }
            },
            { "group1", new Dictionary<string, object>
                {
                    { "group2", new Dictionary<string, object>
                        {
                            { "b", 99 }
                        }
                    }
                }
            }
        };
        var output = new Dictionary<string, object>();

        UnityNativeVariableUtils.ConvertNestedDictionariesToFlat("", input, output);
        var expected = new Dictionary<string, object>
        {
            { "a", 1 },
            { "group.a", "value" },
            { "group1.group2.b", 99 }
        };
        CollectionAssert.AreEqual(expected, output);
    }

    [Test]
    public void ConvertNestedDictionariesToFlat_With_Prefix()
    {
        var input = new Dictionary<string, object>
        {
            { "a", 1 },
            { "group1", new Dictionary<string, object>
                {
                    { "a", "value" },
                    { "group2", new Dictionary<string, object>
                        {
                            { "b", 99 }
                        }
                    }
                }
            }
        };
        var output = new Dictionary<string, object>();

        UnityNativeVariableUtils.ConvertNestedDictionariesToFlat("prefix.", input, output);
        var expected = new Dictionary<string, object>
        {
            { "prefix.a", 1 },
            { "prefix.group1.a", "value" },
            { "prefix.group1.group2.b", 99 }
        };
        CollectionAssert.AreEqual(expected, output);
    }

    [Test]
    public void ConvertNestedDictionariesToFlat_With_Empty()
    {
        var input = new Dictionary<string, object>();
        var output = new Dictionary<string, object>();

        UnityNativeVariableUtils.ConvertNestedDictionariesToFlat("", input, output);
        CollectionAssert.AreEqual(new Dictionary<string, object>(), output);
    }

    [Test]
    public void ConvertNestedDictionariesToFlat_With_Null()
    {
        Assert.DoesNotThrow(() => UnityNativeVariableUtils.ConvertNestedDictionariesToFlat("", null, null));
    }

    #endregion
}

