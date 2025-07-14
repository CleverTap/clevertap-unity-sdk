using System.Collections.Generic;
using System.Net;
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;
using NUnit.Framework;
using UnityEngine;

public class UnityNativeDisplayResponseInterceptorTest
{
    private IUnityNativeResponseInterceptor _interceptor;
    private UnityNativeDisplayResponseInterceptor _unityNativeDisplayResponseInterceptor;
    private UnityNativeCallbackHandler _callbackHandler = null;
    private UnityNativeEventManager _unityNativeEventManager = null;

    [SetUp]
    public void Setup()
    {
        _callbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>(CleverTapGameObjectName.UNITY_NATIVE_CALLBACK_HANDLER);
        _unityNativeEventManager = new UnityNativeEventManager(_callbackHandler);
        _interceptor = new UnityNativeDisplayResponseInterceptor(_unityNativeEventManager);
    }

    private T CreateGameObjectAndAttachCallbackHandler<T>(string objectName) where T : CleverTapCallbackHandler
    {
        var gameObject = new GameObject(objectName);
        gameObject.AddComponent<T>();
        GameObject.DontDestroyOnLoad(gameObject);
        return gameObject.GetComponent<T>();
    }

    [Test]
    public void Intercept_Success()
    {
        var content = new Dictionary<string, object>
        {
            {
                "adUnit_notifs", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "wzrk_id", "msg1" },
                    }
                }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        List<object> expectedMessages = new List<object>
        {
            new Dictionary<string, object>
            {
                { "wzrk_id", "msg1" },
            }
        };

        Assert.AreEqual(expectedMessages, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Content_Null()
    {
        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, null);
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Error()
    {
        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.InternalServerError, null, null);
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Error_With_Content()
    {
        var content = new Dictionary<string, object>
        {
            { "adUnit_notifs", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "wzrk_id", "msg1" },
                    }
                }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.BadRequest, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Success_With_Different_Content()
    {
        var content = new Dictionary<string, object>
        {
            { "adUnit_notifs123", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "wzrk_id", "msg1" },
                    }
                }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Success_With_Null()
    {
        var content = new Dictionary<string, object>
        {
            { "adUnit_notifs", null }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Success_Not_List()
    {
        var content = new Dictionary<string, object>
        {
            { "adUnit_notifs", new Dictionary<string, int>() }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }

    [Test]
    public void Intercept_Success_With_EmptyList()
    {
        var content = new Dictionary<string, object>
        {
            { "adUnit_notifs", new List<object>() }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeDisplayResponseInterceptor = (UnityNativeDisplayResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeDisplayResponseInterceptor.displayUnits);
    }
}