using System.Collections;
using System.Collections.Generic;
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnityNativePlatformVariableTest
{
    private UnityNativeCoreState _coreState;
    private UnityNativeVarCache _cache;
    private UnityNativePlatformVariable _platformVariable;
    private UnityNativeCallbackHandler _callbackHandler;

    private readonly float TimeoutSeconds = 5f;

    [SetUp]
    public void Setup()
    {
        _cache = new UnityNativeVarCache();
        _platformVariable = new UnityNativePlatformVariable(_cache);

        var accountInfo = new UnityNativeAccountInfo("accountId", "token");
        _coreState = new UnityNativeCoreState(accountInfo);
        _callbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>(CleverTapGameObjectName.UNITY_NATIVE_CALLBACK_HANDLER);

        var eventManager = new UnityNativeEventManager(_callbackHandler, _platformVariable);
        _platformVariable.Load(eventManager, _callbackHandler, _coreState);
    }

    [TearDown]
    public void TearDown()
    {
        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        prefManager.DeleteKey(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId));
    }

    private T CreateGameObjectAndAttachCallbackHandler<T>(string objectName) where T : CleverTapCallbackHandler
    {
        var gameObject = new GameObject(objectName);
        gameObject.AddComponent<T>();
        GameObject.DontDestroyOnLoad(gameObject);
        return gameObject.GetComponent<T>();
    }

    [UnityTest]
    public IEnumerator HandleVariablesResponse_Success_TriggersCallbacks()
    {
        bool variablesChangedTriggered = false;
        bool variablesChangedNoDownloadsTriggered = false;

        void VariablesChanged() => variablesChangedTriggered = true;
        void VariablesChangedNoDownloads() => variablesChangedNoDownloadsTriggered = true;

        try
        {
            _callbackHandler.OnVariablesChanged += VariablesChanged;
            _callbackHandler.OnVariablesChangedAndNoDownloadsPending += VariablesChangedNoDownloads;

            _platformVariable.HandleVariablesResponseSuccess(new Dictionary<string, object>());

            // Wait up to 5 seconds for both callbacks to trigger
            float timeout = Time.time + TimeoutSeconds;
            while (!(variablesChangedTriggered && variablesChangedNoDownloadsTriggered) && Time.time < timeout)
            {
                yield return null;
            }

            Assert.IsTrue(_platformVariable.HasVarsRequestCompleted);
            Assert.IsTrue(variablesChangedTriggered, "VariablesChanged was not called in time.");
            Assert.IsTrue(variablesChangedNoDownloadsTriggered, "VariablesChangedNoDownloads was not called in time.");
        }
        finally
        {
            _callbackHandler.OnVariablesChanged -= VariablesChanged;
            _callbackHandler.OnVariablesChangedAndNoDownloadsPending -= VariablesChangedNoDownloads;
        }
    }

    [UnityTest]
    public IEnumerator HandleVariablesResponse_Success_UpdatesValues()
    {
        bool variablesChangedTriggered = false;

        var var1 = _platformVariable.Define("var1", 3);
        var var2 = _platformVariable.Define("group1.var2", "value");
        var var3 = _platformVariable.Define("group1.var3", 20);

        void variablesChanged()
        {
            variablesChangedTriggered = true;

            // Assest values are updated
            Assert.AreEqual(10, var1.Value);
            Assert.AreEqual("new value", var2.Value);
            Assert.AreEqual(30, var3.Value);
        }

        try
        {
            _callbackHandler.OnVariablesChanged += variablesChanged;

            var diffs = new Dictionary<string, object>
            {
                { "var1", 10 },
                { "group1", new Dictionary<string, object>
                {
                    { "var2", "new value" },
                    { "var3", 30 },
                }}
            };

            _platformVariable.HandleVariablesResponseSuccess(diffs);

            float timeout = Time.time + TimeoutSeconds;
            while (!variablesChangedTriggered && Time.time < timeout)
            {
                yield return null;
            }

            Assert.IsTrue(variablesChangedTriggered, "VariablesChanged was not called in time.");
        }
        finally
        {
            _callbackHandler.OnVariablesChanged -= variablesChanged;
        }
    }

    [UnityTest]
    public IEnumerator HandleVariablesResponse_Error_TriggersCallbacks()
    {
        bool variablesChangedTriggered = false;
        bool variablesChangedNoDownloadsTriggered = false;

        void VariablesChanged() => variablesChangedTriggered = true;
        void VariablesChangedNoDownloads() => variablesChangedNoDownloadsTriggered = true;

        var var1 = _platformVariable.Define("var1", 3);
        bool var1_ValueChanged = false;
        void var1_OnValueChanged()
        {
            var1_ValueChanged = true;
            Assert.AreEqual(3, var1.Value);
        }

        try
        {
            _callbackHandler.OnVariablesChanged += VariablesChanged;
            _callbackHandler.OnVariablesChangedAndNoDownloadsPending += VariablesChangedNoDownloads;
            var1.OnValueChanged += var1_OnValueChanged;

            _platformVariable.HandleVariablesResponseError();

            float timeout = Time.time + TimeoutSeconds;
            while (!(variablesChangedTriggered && variablesChangedNoDownloadsTriggered && var1_ValueChanged) && Time.time < timeout)
            {
                yield return null;
            }

            Assert.IsTrue(_platformVariable.HasVarsRequestCompleted);
            Assert.IsTrue(variablesChangedTriggered, "VariablesChanged was not called in time.");
            Assert.IsTrue(variablesChangedNoDownloadsTriggered, "VariablesChangedNoDownloads was not called in time.");
            Assert.IsTrue(var1_ValueChanged, "Variable var1 ValueChanged was not called in time.");
        }
        finally
        {
            _callbackHandler.OnVariablesChanged -= VariablesChanged;
            _callbackHandler.OnVariablesChangedAndNoDownloadsPending -= VariablesChangedNoDownloads;
            var1.OnValueChanged -= var1_OnValueChanged;
        }
    }

    [UnityTest]
    public IEnumerator HandleVariablesResponse_Success_TriggersVarUpdate()
    {
        var var1 = _platformVariable.Define("var1", 3);
        var var2 = _platformVariable.Define("group1.var2", "value");

        bool var1_ValueChanged = false;
        void var1_OnValueChanged()
        {
            var1_ValueChanged = true;
            Assert.AreEqual(10, var1.Value);
        }

        bool var2_ValueChanged = false;
        void var2_OnValueChanged()
        {
            var2_ValueChanged = true;
            Assert.AreEqual("new value", var2.Value);
        }

        try
        {
            var1.OnValueChanged += var1_OnValueChanged;
            var2.OnValueChanged += var2_OnValueChanged;
            var diffs = new Dictionary<string, object>
            {
                { "var1", 10 },
                { "group1", new Dictionary<string, object>
                    { { "var2", "new value" } }
                }
            };

            _platformVariable.HandleVariablesResponseSuccess(diffs);

            float timeout = Time.time + TimeoutSeconds;
            while (!var1_ValueChanged && Time.time < timeout)
            {
                yield return null;
            }

            Assert.IsTrue(var1_ValueChanged, "Variable var1 ValueChanged was not called in time.");
            Assert.IsTrue(var2_ValueChanged, "Variable var2 ValueChanged was not called in time.");
        }
        finally
        {
            var1.OnValueChanged -= var1_OnValueChanged;
            var2.OnValueChanged -= var2_OnValueChanged;
        }
    }

    [Test]
    public void HandleVariablesResponse_Success_SavesDiffs()
    {
        _ = _platformVariable.Define("var1", 3);
        var diffs = new Dictionary<string, object>
        {
            { "var1", 10 }
        };

        _platformVariable.HandleVariablesResponseSuccess(diffs);

        string expected = Json.Serialize(diffs);
        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        string actual = prefManager.GetString(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId), string.Empty);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void SwitchUser_LoadDiffs()
    {
        int defaultValue = 3;
        var var1 = _platformVariable.Define("var1", defaultValue);
        int newValue = 10;
        var diffs = new Dictionary<string, object>
        {
            { "var1", newValue }
        };
        _platformVariable.HandleVariablesResponseSuccess(diffs);
        Assert.IsTrue(_platformVariable.HasVarsRequestCompleted);
        Assert.AreEqual(newValue, var1.Value);

        var prefManager = UnityNativePreferenceManager.GetPreferenceManager(_coreState.AccountInfo.AccountId);
        prefManager.DeleteKey(_cache.GetDiffsKey(_coreState.DeviceInfo.DeviceId));

        // Change user
        _coreState.DeviceInfo.ForceNewDeviceID();
        _platformVariable.SwitchUser();
        Assert.IsFalse(_platformVariable.HasVarsRequestCompleted);
        Assert.AreEqual(defaultValue, var1.Value);
    }
}