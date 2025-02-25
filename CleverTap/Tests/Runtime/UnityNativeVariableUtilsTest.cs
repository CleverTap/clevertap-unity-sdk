using System.Collections;
using System.Collections.Generic;
using CleverTapSDK.Native;
using NUnit.Framework;

public class UnityNativeVariableUtilsTest
{
    [TestCase("a", new string[] { "a" })]
    [TestCase("a.b.c", new string[] { "a", "b", "c" })]
    public void Test_GetNameComponents(string name, string[] expected)
    {
        Assert.AreEqual(expected, UnityNativeVariableUtils.GetNameComponents(name));
    }

    #region MergeHelper

    [Test]
    public void Test_MergeHelper_With_Null_Diff()
    {
        var vars = new Dictionary<string, object> { { "a", 1 }, { "b", 2 } };

        var result = UnityNativeVariableUtils.MergeHelper(vars, null);
        Assert.AreEqual(vars, result);
    }

    [TestCase("defaultValue", "newValue")]
    [TestCase(null, "newValue")]
    [TestCase(199, 1234.56)]
    [TestCase("a", 1)]
    public void Test_MergeHelper_With_Primitives(object vars, object diff)
    {
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        Assert.AreEqual(diff, result);
    }

    [Test]
    public void Test_MergeHelper_With_Booleans()
    {
        bool vars = true;
        bool diff = false;
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        Assert.True(result is bool);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void Test_MergeHelper_With_Dictionary_And_Primitive()
    {
        var vars = new Dictionary<string, object> { { "key1", "value1" }, { "key2", "value2" } };
        var diff = "diff";
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        Assert.AreEqual(diff, result);
    }

    [Test]
    public void Test_MergeHelper_With_Dictionaries()
    {
        var vars = new Dictionary<string, object> { { "a", 1 }, { "b", 2 } };
        var diff = new Dictionary<string, object> { { "b", 42 }, { "c", 3 } };
        var expected = new Dictionary<string, object> { { "a", 1 }, { "b", 42 }, { "c", 3 } };
        var result = UnityNativeVariableUtils.MergeHelper(vars, diff);
        CollectionAssert.AreEqual(expected, result as IEnumerable);
    }

    [Test]
    public void Test_MergeHelper_With_Nested_Dictionaries_Add_And_Merge()
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
    public void Test_MergeHelper_Merge_With_Empty()
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
    public void Test_MergeHelper_Merge_Empty()
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
    public void Test_MergeHelper_Merge_Dictionaries_Include_Values()
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
    public void Test_MergeHelper_With_Different_Types()
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
    public void Test_MergeHelper_With_Different_Types_And_Nested_Dictionaries()
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
    public void Test_MergeHelper_With_Array()
    {
        var vars = new[] { 1, 2, 3, 4 };
        var diffs = new[] { 1, 2, 3, 4 };
        // MergeHelper does not support merging arrays
        Assert.IsNull(UnityNativeVariableUtils.MergeHelper(vars, diffs));
    }

    [Test]
    public void Test_MergeHelper_With_Dictionary_Array()
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
    public void Test_ConvertDictionaryToNestedDictionaries()
    {
        var input = new Dictionary<string, object> { { "a", 1 } };

        var result = UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input);
        CollectionAssert.AreEqual(input, result);
    }

    [Test]
    public void Test_ConvertDictionaryToNestedDictionaries_With_Group()
    {
        var input = new Dictionary<string, object> { { "group.a", 1 } };
        var expected = new Dictionary<string, object>
        {
            { "group", new Dictionary<string, object> { { "a", 1 } } }
        };
        CollectionAssert.AreEqual(expected, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    [Test]
    public void Test_ConvertDictionaryToNestedDictionaries_With_Two_Groups()
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
    public void Test_ConvertDictionaryToNestedDictionaries_With_Variables()
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
    public void Test_ConvertDictionaryToNestedDictionaries_With_InvalidData()
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
    public void Test_ConvertDictionaryToNestedDictionaries_With_Empty()
    {
        var input = new Dictionary<string, object>();
        CollectionAssert.AreEqual(input, UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(input));
    }

    #endregion
}

