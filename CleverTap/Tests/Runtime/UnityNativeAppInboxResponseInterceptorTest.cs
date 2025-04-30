using System;
using System.Collections.Generic;
using System.Net;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;
using NUnit.Framework;

public class UnityNativeAppInboxResponseInterceptorTest
{
    private IUnityNativeResponseInterceptor _interceptor;
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
            { "inbox_notifs", new List<object>()
                {
                    new Dictionary<string, object>
                    {
                        { "_id", "msg1" },
                    }
                }
            }
        };

        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
    }

    [Test]
    public void Intercept_Content_Null()
    {
        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, null);
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
    }

    [Test]
    public void Intercept_Error()
    {
        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.InternalServerError, null, null);
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
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
                        { "id", "msg1" },
                    }
                }
            }
        };

        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.BadRequest, null, Json.Serialize(content));
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
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
                        { "ids", "msg1" },
                    }
                }
            }
        };

        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
    }

    [Test]
    public void Intercept_Success_With_Vars_Null()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs", null }
        };

        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
    }

    [Test]
    public void Intercept_Success_With_Vars_Not_List()
    {
        var content = new Dictionary<string, object>
        {
            { "inbox_notifs", new Dictionary<string,Object>() }
        };

        try
        {
            UnityNativeResponse response = new UnityNativeResponse(HttpStatusCode.OK, null, Json.Serialize(content));
            _interceptor.Intercept(response);
        }
        catch (System.Exception e)
        {
            Assert.Fail($"Exception: {e.Message}, Stack Trace: {e.StackTrace}");
        }
    }
}