using System.Collections.Generic;
using System.Net;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;
using NUnit.Framework;

public class UnityNativeAppInboxResponseInterceptorTest
{
    private IUnityNativeResponseInterceptor _interceptor;
    private UnityNativeAppInboxResponseInterceptor _unityNativeAppInboxResponseInterceptor;
    private readonly UnityNativeEventManager _unityNativeEventManager = null;

    [SetUp]
    public void Setup()
    {
        _interceptor = new UnityNativeAppInboxResponseInterceptor(_unityNativeEventManager);
    }

    [Test]
    public void Intercept_Success()
    {
        var content = new Dictionary<string, object>
        {
            {
                "inbox_notifs", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "_id", "msg1" },
                    }
                }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        List<object> expectedMessages = new List<object>
        {
            new Dictionary<string, object>
            {
                { "_id", "msg1" },
            }
        };

        Assert.AreEqual(expectedMessages, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Content_Null()
    {
        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, null);
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Error()
    {
        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.InternalServerError, null, null);
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Error_With_Content()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "_id", "msg1" },
                    }
                }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.BadRequest, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Success_With_Different_Content()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs123", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "_id", "msg1" },
                    }
                }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Success_With_Null()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs", null }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Success_Not_List()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs", new Dictionary<string, int>() }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }

    [Test]
    public void Intercept_Success_With_EmptyList()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs", new List<object>() }
        };

        UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);
        _unityNativeAppInboxResponseInterceptor = (UnityNativeAppInboxResponseInterceptor)_interceptor;

        Assert.AreEqual(null, _unityNativeAppInboxResponseInterceptor.AppInboxMessages);
    }
}