using System.Collections;
using System.Collections.Generic;
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;
using NUnit.Framework;

public class UnityNativeVarCacheTest
{
    private UnityNativeVarCache _cache;
    private Dictionary<string, string> _value;
    private Var<Dictionary<string, string>> _variable;

    private UnityNativeCoreState _coreState;

    [SetUp]
    public void Setup()
    {
        _cache = new UnityNativeVarCache();
    }

    [TearDown]
    public void TearDown()
    {
        if (_coreState != null)
        {
            var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
            prefManager.DeleteKey(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId));
            _coreState = null;
        }
    }

    /// <summary>
    /// Creates a variable and registers it.
    /// </summary>
    private void SetupVariable()
    {
        _value = new Dictionary<string, string>
        {
            { "key", "value" }
        };
        _variable = new UnityNativeVar<Dictionary<string, string>>("testVar", CleverTapVariableKind.DICTIONARY, _value, _cache);
    }

    private void SetCoreState()
    {
        var accountInfo = new UnityNativeAccountInfo("accountId", "token");
        _coreState = new UnityNativeCoreState(accountInfo);
        _cache.SetCoreState(_coreState);
    }

    [Test]
    public void RegisterVariable_MergedValue()
    {
        SetupVariable();
        var mergedValue = _cache.GetMergedValue(_variable.Name) as IDictionary;
        CollectionAssert.AreEqual(mergedValue, _value);
    }

    [Test]
    public void RegisterVariable_ShouldAddVariableToCache()
    {
        SetupVariable();
        var registeredVar = _cache.GetVariable<Dictionary<string, string>>("testVar");
        Assert.IsNotNull(registeredVar);
        Assert.AreEqual("testVar", registeredVar.Name);
    }

    [Test]
    public void GetVariable_ShouldReturnNullIfVariableNotRegistered()
    {
        SetupVariable();
        var result = _cache.GetVariable<string>("nonExistentVar");
        Assert.IsNull(result);
    }

    [Test]
    public void GetMergedValue_ShouldReturnMergedValue()
    {
        SetupVariable();
        var mergedValue = _cache.GetMergedValue(_variable.Name);

        Assert.IsNotNull(mergedValue);
        var mergedDict = mergedValue as Dictionary<string, string>;
        Assert.AreEqual("value", mergedDict["key"]);
    }

    [Test]
    public void MergeVariable_ShouldMergeValuesCorrectly()
    {
        SetupVariable();
        var diffs = new Dictionary<string, object>
        {
            { _variable.Name, new Dictionary<string, object> { { "key", "mergedValue" } } }
        };

        _cache.ApplyVariableDiffs(diffs);

        var mergedValue = _cache.GetMergedValue(_variable.Name);
        Assert.IsNotNull(mergedValue);
        var mergedDict = mergedValue as Dictionary<string, string>;
        Assert.AreEqual("mergedValue", mergedDict["key"]);
    }

    [Test]
    public void GetVariable_ShouldReturnDefaultValue()
    {
        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 1, _cache);
        var var2 = new UnityNativeVar<int>("var2", CleverTapVariableKind.INT, 2, _cache);
        var var3 = new UnityNativeVar<string>("var3", CleverTapVariableKind.STRING, "string", _cache);
        var var4 = new UnityNativeVar<bool>("var4", CleverTapVariableKind.BOOLEAN, true, _cache);
        var var5 = new UnityNativeVar<double>("var5", CleverTapVariableKind.FLOAT, 4.99, _cache);

        Assert.AreEqual(5, _cache.VariablesCount);
        Assert.AreEqual(1, _cache.GetVariable<int>(var1.Name).Value);
        Assert.AreEqual(2, _cache.GetVariable<int>(var2.Name).Value);
        Assert.AreEqual("string", _cache.GetVariable<string>(var3.Name).Value);
        Assert.AreEqual(true, _cache.GetVariable<bool>(var4.Name).Value);
        Assert.AreEqual(4.99, _cache.GetVariable<double>(var5.Name).Value);
    }

    [Test]
    public void RegisterVariable_With_Group_GetVariable()
    {
        var var1 = new UnityNativeVar<int>("group.var1", CleverTapVariableKind.INT, 1, _cache);
        var var2 = new UnityNativeVar<int>("group.var2", CleverTapVariableKind.INT, 2, _cache);
        var var3 = new UnityNativeVar<Dictionary<string, object>>("group", CleverTapVariableKind.DICTIONARY,
            new Dictionary<string, object> { { "var3", 3 } }, _cache);

        Assert.AreEqual(3, _cache.VariablesCount);
        Assert.AreEqual(1, _cache.GetVariable<int>(var1.Name).Value);
        Assert.AreEqual(2, _cache.GetVariable<int>("group.var2").Value);
        CollectionAssert.AreEqual(new Dictionary<string, object> { { "var1", 1 }, { "var2", 2 }, { "var3", 3 } },
            _cache.GetVariable<Dictionary<string, object>>("group").Value);
    }

    [Test]
    public void RegisterVariable_With_NestedGroups_GetVariable()
    {
        _cache.ApplyVariableDiffs(new Dictionary<string, object>());

        var var1 = new UnityNativeVar<int>("group1.group2.var3", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<int>("group1.var1", CleverTapVariableKind.INT, 1, _cache);
        var var3 = new UnityNativeVar<Dictionary<string, object>>("group1", CleverTapVariableKind.DICTIONARY,
    new Dictionary<string, object> { { "var2", 2 } }, _cache);
        var var4 = new UnityNativeVar<Dictionary<string, object>>("group1.group2", CleverTapVariableKind.DICTIONARY,
            new Dictionary<string, object> { { "var4", 4 } }, _cache);

        Assert.AreEqual(4, _cache.VariablesCount);
        Assert.AreEqual(3, _cache.GetVariable<int>("group1.group2.var3").Value);
        Assert.AreEqual(1, _cache.GetVariable<int>("group1.var1").Value);

        var expected = new Dictionary<string, object>
        {
            { "var1", 1 }, { "var2", 2 }, { "group2", new Dictionary<string, object> { { "var3", 3 }, { "var4", 4 } } }
        };
        CollectionAssert.AreEqual(expected, _cache.GetVariable<Dictionary<string, object>>("group1").Value);

        expected = new Dictionary<string, object> { { "var3", 3 }, { "var4", 4 } };
        CollectionAssert.AreEqual(expected, _cache.GetVariable<Dictionary<string, object>>("group1.group2").Value);
    }

    [Test]
    public void RegisterVariable_With_Group_And_DefaultValue()
    {
        var var1 = new UnityNativeVar<int>("group.var1", CleverTapVariableKind.INT, 1, _cache);
        var var2 = new UnityNativeVar<Dictionary<string, object>>("group", CleverTapVariableKind.DICTIONARY,
    new Dictionary<string, object> { { "var2", 2 } }, _cache);

        Assert.AreEqual(2, _cache.VariablesCount);
        Assert.AreEqual(1, _cache.GetVariable<int>(var1.Name).Value);
        Assert.AreEqual(1, _cache.GetVariable<int>(var1.Name).DefaultValue);
        CollectionAssert.AreEqual(new Dictionary<string, object> { { "var1", 1 }, { "var2", 2 } },
            _cache.GetVariable<Dictionary<string, object>>("group").Value);
        CollectionAssert.AreEqual(new Dictionary<string, object> { { "var2", 2 } },
            _cache.GetVariable<Dictionary<string, object>>("group").DefaultValue);
    }

    [Test]
    public void RegisterVariable_With_NestedGroups_And_DefaultValue()
    {
        var var1 = new UnityNativeVar<int>("group1.var1", CleverTapVariableKind.INT, 1, _cache);
        var var2 = new UnityNativeVar<int>("group1.group2.var3", CleverTapVariableKind.INT, 3, _cache);
        var var3 = new UnityNativeVar<Dictionary<string, object>>("group1", CleverTapVariableKind.DICTIONARY, new Dictionary<string, object> { { "var2", 2 }, { "group2", new Dictionary<string, object> { { "var4", 4 } } } }, _cache);

        Assert.AreEqual(3, _cache.VariablesCount);
        Assert.AreEqual(1, _cache.GetVariable<int>(var1.Name).Value);
        Assert.AreEqual(3, _cache.GetVariable<int>(var2.Name).Value);

        var expectedValue = new Dictionary<string, object>
        {
            { "var1", 1 }, { "var2", 2 }, { "group2", new Dictionary<string, object> { { "var3", 3 }, { "var4", 4 } } }
        };
        CollectionAssert.AreEqual(expectedValue, _cache.GetVariable<Dictionary<string, object>>("group1").Value);

        var expectedDefaultValue = new Dictionary<string, object>
        {
            { "var2", 2 }, { "group2", new Dictionary<string, object> { { "var4", 4 } } }
        };
        CollectionAssert.AreEqual(expectedDefaultValue, _cache.GetVariable<Dictionary<string, object>>("group1").DefaultValue);
    }

    [Test]
    public void RegisterVariable_With_File()
    {
        var var1 = new UnityNativeVar<string>("var1", CleverTapVariableKind.FILE, null, _cache);
        Assert.AreEqual(1, _cache.VariablesCount);
        Assert.IsNull(_cache.GetVariable<string>(var1.Name).Value);
        Assert.IsNull(_cache.GetMergedValue("var1"));
    }

    [Test]
    public void GetMergedValue()
    {
        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 100, _cache);
        _cache.RegisterVariable(var1);
        Assert.AreEqual(100, _cache.GetMergedValue("var1"));
    }

    [Test]
    public void GetMergedValue_With_Group()
    {
        var var1 = new UnityNativeVar<int>("group.var1", CleverTapVariableKind.INT, 1, _cache);
        var var2 = new UnityNativeVar<Dictionary<string, object>>("group", CleverTapVariableKind.DICTIONARY,
    new Dictionary<string, object> { { "var2", 2 }, { "var3", 3 } }, _cache);
        var var4 = new UnityNativeVar<int>("var4", CleverTapVariableKind.INT, 4, _cache);

        Assert.AreEqual(1, _cache.GetMergedValue("group.var1"));
        Assert.AreEqual(2, _cache.GetMergedValue("group.var2"));
        Assert.AreEqual(3, _cache.GetMergedValue("group.var3"));
        Assert.AreEqual(4, _cache.GetMergedValue("var4"));
    }

    [Test]
    public void GetMergedValue_With_Groups()
    {
        var var1 = new UnityNativeVar<int>("group1.group2.var3", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<int>("group1.var1", CleverTapVariableKind.INT, 1, _cache);
        var var3 = new UnityNativeVar<Dictionary<string, object>>("group1", CleverTapVariableKind.DICTIONARY,
    new Dictionary<string, object> { { "var2", 2 } }, _cache);
        var var4 = new UnityNativeVar<Dictionary<string, object>>("group1.group2", CleverTapVariableKind.DICTIONARY,
            new Dictionary<string, object> { { "var4", 4 } }, _cache);

        Assert.AreEqual(1, _cache.GetMergedValue("group1.var1"));
        Assert.AreEqual(2, _cache.GetMergedValue("group1.var2"));
        Assert.AreEqual(3, _cache.GetMergedValue("group1.group2.var3"));
        Assert.AreEqual(4, _cache.GetMergedValue("group1.group2.var4"));
    }

    [Test]
    public void GetMergedValueFromComponentArray()
    {
        var components = new[] { "a", "b", "c" };
        var values = new Dictionary<string, object>
        {
            { "a", new Dictionary<string, object> { { "b", new Dictionary<string, object> { { "c", "finalValue" } } } } }
        };
        var result = _cache.GetMergedValueFromComponentArray(components, values);
        Assert.AreEqual("finalValue", result);
    }

    [Test]
    public void GetMergedValueFromComponentArray_For_WrongValue()
    {
        var components = new[] { "a", "b", "c" };
        var values = new Dictionary<string, object>
        {
            { "a", new Dictionary<string, object> { { "b", new Dictionary<string, object> { { "d", "finalValue" } } } } }
        };
        var result = _cache.GetMergedValueFromComponentArray(components, values);
        Assert.IsNull(result);
    }

    [Test]
    public void ApplyDiffs_UpdatesValues()
    {
        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<string>("group1.var2", CleverTapVariableKind.STRING, "value", _cache);

        _cache.ApplyVariableDiffs(new Dictionary<string, object>
        {
            { "var1", 10 },
            { "group1", new Dictionary<string, object>
            {
                { "var2", "new value" },
                { "var3", 30 },
            }}
        });

        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("new value", var2.Value);
        Assert.AreEqual(30, _cache.GetMergedValue("group1.var3"));
    }

    [Test]
    public void ApplyDiffs_UpdatesValues_And_DefaultsValues()
    {
        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<string>("group1.var2", CleverTapVariableKind.STRING, "value", _cache);

        // Update var1
        _cache.ApplyVariableDiffs(new Dictionary<string, object>
        {
            { "var1", 10 }
        });
        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("value", var2.Value);

        // Update group1.var2
        // Update var1 to default value since it is not in the diff
        _cache.ApplyVariableDiffs(new Dictionary<string, object>
        {
            { "group1", new Dictionary<string, object>
            {
                { "var2", "new value" }
            }}
        });
        Assert.AreEqual(3, var1.Value);
        Assert.AreEqual("new value", var2.Value);

        // Update to default values since diff is empty dictionary
        _cache.ApplyVariableDiffs(new Dictionary<string, object>());
        Assert.AreEqual(3, var1.Value);
        Assert.AreEqual("value", var2.Value);
    }

    [Test]
    public void ApplyDiffs_UpdatesValues_With_Null()
    {
        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<string>("group1.var2", CleverTapVariableKind.STRING, "value", _cache);

        _cache.ApplyVariableDiffs(new Dictionary<string, object>
        {
            { "var1", 10 }
        });
        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("value", var2.Value);

        // ApplyVariableDiffs with null should not apply the diff
        _cache.ApplyVariableDiffs(null);
        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("value", var2.Value);
    }

    [Test]
    public void LoadDiffs_UpdatesValues()
    {
        var diffs = new Dictionary<string, object>
        {
            { "var1", 10 },
            { "group1", new Dictionary<string, object>
            {
                { "var2", "new value" },
                { "var3", 30 },
            }}
        };

        string serializedData = Json.Serialize(diffs);
        SetCoreState();
        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        prefManager.SetString(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId), serializedData);

        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<string>("group1.var2", CleverTapVariableKind.STRING, "value", _cache);

        _cache.LoadDiffs();

        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("new value", var2.Value);
        Assert.AreEqual(30, _cache.GetMergedValue("group1.var3"));
    }

    [Test]
    public void LoadDiffs_IncorrectData()
    {
        var diffs = new Dictionary<string, object>
        {
            { "var1", 10 },
            { "group1", new Dictionary<string, object>
            {
                { "var2", "new value" }
            }}
        };

        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        var var2 = new UnityNativeVar<string>("group1.var2", CleverTapVariableKind.STRING, "value", _cache);

        // Apply new values
        _cache.ApplyVariableDiffs(diffs);
        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("new value", var2.Value);

        // Persist incorrect data for diffs
        string serializedData = Json.Serialize("incorrect-value");
        SetCoreState();
        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        prefManager.SetString(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId), serializedData);

        // LoadDiffs will read the persisted data
        Assert.DoesNotThrow(() => _cache.LoadDiffs());

        // Persisted data is incorrect, so LoadDiffs will not apply it
        // The variable values should remain the same
        Assert.AreEqual(10, var1.Value);
        Assert.AreEqual("new value", var2.Value);
    }

    [Test]
    public void SaveDiffs_Saves()
    {
        _ = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        _ = new UnityNativeVar<string>("group1.var2", CleverTapVariableKind.STRING, "value", _cache);
        var diffs = new Dictionary<string, object>
        {
            { "var1", 10 },
            { "group1", new Dictionary<string, object>
            {
                { "var2", "new value" },
                { "var3", 30 },
            }}
        };

        SetCoreState();
        _cache.ApplyVariableDiffs(diffs);
        _cache.SaveDiffs();

        string expected = Json.Serialize(diffs);
        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        string actual = prefManager.GetString(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId), string.Empty);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Reset_ClearsState()
    {
        _cache.SetHasVarsRequestCompleted(true);
        var var1 = new UnityNativeVar<int>("var1", CleverTapVariableKind.INT, 3, _cache);
        Assert.IsTrue(_cache.HasVarsRequestCompleted);
        Assert.IsTrue(var1.HadStarted);

        _cache.Reset();
        Assert.IsFalse(_cache.HasVarsRequestCompleted);
        Assert.IsFalse(var1.HadStarted);
    }
}

