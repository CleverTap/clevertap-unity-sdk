using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Native;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

internal class UnityNativeVarCacheMock : UnityNativeVarCache
{
    private readonly Dictionary<string, object> _values = new();

    internal override void RegisterVariable(IVar variable)
    {
        _values[variable.Name] = variable.DefaultObjectValue;
    }

    internal override object GetMergedValueFromComponentArray(object[] components)
    {
        return _values.TryGetValue(string.Join(".", components), out var value) ? value : null;
    }

    internal void SetValue(string name, object value)
    {
        _values[name] = value;
    }
}

public class UnityNativeVarTest
{
    private UnityNativeVarCacheMock _varCache;
    private UnityNativeVar<int> _intVar;
    private UnityNativeCoreState _coreState;

    [SetUp]
    public void SetUp()
    {
        var accountInfo = new UnityNativeAccountInfo("accountId", "token");
        _coreState = new UnityNativeCoreState(accountInfo);

        _varCache = new UnityNativeVarCacheMock();
        _varCache.SetCoreState(_coreState);

        _intVar = new UnityNativeVar<int>("testVar", CleverTapVariableKind.INT, 10, _varCache);
    }

    [TearDown]
    public void TearDown()
    {
        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        prefManager.DeleteKey(_varCache.GetDiffsKey(_coreState.DeviceInfo.DeviceId));
    }

    [Test]
    public void Constructor_Initializes_Properly()
    {
        Assert.AreEqual(10, _intVar.Value);
        Assert.IsFalse(_intVar.HadStarted);
    }

    [Test]
    public void Update_Changes_Value_And_Triggers_Callback()
    {
        _varCache.SetHasVarsRequestCompleted(true);
        _varCache.SetValue("testVar", 42);

        int eventTriggeredCount = 0;
        // Triggers immediately since HasVarsRequestCompleted
        _intVar.OnValueChanged += () => eventTriggeredCount++;

        // Triggers callback again
        _intVar.Update();

        Assert.AreEqual(42, _intVar.Value);
        Assert.IsTrue(_intVar.HadStarted);
        Assert.AreEqual(2, eventTriggeredCount);
    }

    [Test]
    public void Update_Triggers_Callback_If_Same_Value_When_HasVarsRequestCompleted()
    {
        int eventTriggeredCount = 0;
        _intVar.OnValueChanged += () => eventTriggeredCount++;

        _intVar.Update();

        _varCache.SetHasVarsRequestCompleted(true);
        _intVar.Update();

        Assert.IsTrue(_intVar.HadStarted);
        Assert.AreEqual(1, eventTriggeredCount);
    }

    [Test]
    public void Update_Not_Trigger_Callback_If_Value_Unchanged()
    {
        // Callbacks trigger only if HasVarsRequestCompleted
        _varCache.SetHasVarsRequestCompleted(true);
        // First update
        _intVar.Update();
        // Set same value
        _varCache.SetValue("testVar", 10);

        int eventTriggeredCount = 0;
        // Callback is triggered immediately due to HasVarsRequestCompleted
        _intVar.OnValueChanged += () => eventTriggeredCount++;
        // Second update with the same value
        _intVar.Update();

        Assert.AreEqual(1, eventTriggeredCount);
    }

    [Test]
    public void Reset_Sets_HadStarted_To_False()
    {
        _varCache.SetHasVarsRequestCompleted(true);
        _intVar.Update();
        Assert.IsTrue(_intVar.HadStarted);

        _intVar.Reset();
        Assert.IsFalse(_intVar.HadStarted);
    }

    [Test]
    public void OnValueChanged_Triggers_Immediately()
    {
        // If HasVarsRequestCompleted then OnValueChanged should trigger immediately
        _varCache.SetHasVarsRequestCompleted(true);
        bool eventTriggered = false;
        _intVar.OnValueChanged += () => eventTriggered = true;

        Assert.IsTrue(eventTriggered);
    }

    [UnityTest]
    public IEnumerator Update_HandlesInvalidConversionGracefully()
    {
        yield return null;

        LogAssert.Expect(LogType.Error, new Regex("Failed to convert value from String to Int32" + ".*"));

        _varCache.SetValue("testVar", "invalid_value");

        Assert.DoesNotThrow(() => _intVar.Update());
        // Should retain default value
        Assert.AreEqual(10, _intVar.Value);
    }
}