using System.Collections.Generic;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;
using NUnit.Framework;


internal class UnityNativeVariablesResponseHandlerMock : IVariablesResponseHandler
{
    internal bool? isSuccess = null;
    internal IDictionary<string, object> vars;

    public void HandleVariablesResponse(IDictionary<string, object> vars)
    {
        isSuccess = true;
        this.vars = vars;
    }

    public void HandleVariablesResponseError()
    {
        isSuccess = false;
    }
}

public class UnityNativeVariablesResponseInterceptorTest
{
    private IUnityNativeResponseInterceptor _interceptor;
    private UnityNativeVariablesResponseHandlerMock _responseHandler;

    [SetUp]
    public void Setup()
    {
        _responseHandler = new UnityNativeVariablesResponseHandlerMock();
        _interceptor = new UnityNativeVariablesResponseInterceptor(_responseHandler);
    }

    [Test]
    public void Intercept_Success()
    {
        var content = new Dictionary<string, object>
        {
            { "vars", new Dictionary<string, object>
                { { "var1", 1 } }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);

        Assert.IsTrue(_responseHandler.isSuccess);
        Assert.AreEqual(content["vars"], _responseHandler.vars);
    }

    [Test]
    public void Intercept_Content_Null()
    {
        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.OK, null, null);
        _interceptor.Intercept(response);
        // Neither of the methods is called
        Assert.IsNull(_responseHandler.isSuccess);
    }

    [Test]
    public void Intercept_Error()
    {
        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.InternalServerError, null, null);
        _interceptor.Intercept(response);
        Assert.IsFalse(_responseHandler.isSuccess);
    }

    [Test]
    public void Intercept_Error_With_Content()
    {
        var content = new Dictionary<string, object>
        {
            { "vars", new Dictionary<string, object>
                { { "var1", 1 } }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.BadRequest, null, Json.Serialize(content));
        _interceptor.Intercept(response);

        Assert.IsFalse(_responseHandler.isSuccess);
    }

    [Test]
    public void Intercept_Success_With_Different_Content()
    {
        var content = new Dictionary<string, object>
        {
            { "variables", new Dictionary<string, object>
                { { "var1", 1 } }
            }
        };

        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);

        // Neither of the methods is called
        Assert.IsNull(_responseHandler.isSuccess);
    }

    [Test]
    public void Intercept_Success_With_Vars_Null()
    {
        var content = new Dictionary<string, object>
        {
            { "vars", null }
        };

        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);

        Assert.IsFalse(_responseHandler.isSuccess);
    }

    [Test]
    public void Intercept_Success_With_Vars_Not_Dictionary()
    {
        var content = new Dictionary<string, object>
        {
            { "vars", new List<string>() }
        };

        UnityNativeResponse response = new UnityNativeResponse(null, System.Net.HttpStatusCode.OK, null, Json.Serialize(content));
        _interceptor.Intercept(response);

        Assert.IsFalse(_responseHandler.isSuccess);
    }
}

