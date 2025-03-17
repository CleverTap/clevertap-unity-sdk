using System;
using System.Collections;
using System.Collections.Generic;
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Native;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnityNativePlatformVariableTest
{
    private UnityNativeCoreState _coreState;
    private UnityNativePlatformVariable _platformVariable;
    private UnityNativeCallbackHandler _callbackHandler;

    [SetUp]
    public void Setup()
    {
        _platformVariable = new UnityNativePlatformVariable();

        var accountInfo = new UnityNativeAccountInfo("accountId", "token");
        _coreState = new UnityNativeCoreState(accountInfo);
        _callbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>(CleverTapGameObjectName.UNITY_NATIVE_CALLBACK_HANDLER);

        _platformVariable.Load(null, _callbackHandler, _coreState);
    }

    [TearDown]
    public void TearDown()
    {
        PlayerPrefs.DeleteAll();
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

            _platformVariable.HandleVariablesResponse(new Dictionary<string, object>());

            // Wait up to 5 seconds for both callbacks to trigger
            float timeout = Time.time + 5f;
            while (!(variablesChangedTriggered && variablesChangedNoDownloadsTriggered) && Time.time < timeout)
            {
                yield return null;
            }

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

            _platformVariable.HandleVariablesResponse(diffs);

            // Wait up to 5 seconds for both callbacks to trigger
            float timeout = Time.time + 5f;
            while (!variablesChangedTriggered && Time.time < timeout)
            {
                yield return null;
            }

            Assert.IsTrue(variablesChangedTriggered, "Handler1 was not called in time.");
        }
        finally
        {
            _callbackHandler.OnVariablesChanged -= variablesChanged;
        }
    }
}