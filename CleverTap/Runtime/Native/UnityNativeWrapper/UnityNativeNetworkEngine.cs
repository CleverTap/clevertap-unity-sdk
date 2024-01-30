#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverTapSDK.Native {
    internal class UnityNativeNetworkEngine : MonoBehaviour {
        private static readonly Lazy<UnityNativeNetworkEngine> _instance = new Lazy<UnityNativeNetworkEngine>(() => {
            var gameObject = new GameObject("UnityNativeNetworkEngine");
            gameObject.AddComponent<UnityNativeNetworkEngine>();
            GameObject.DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<UnityNativeNetworkEngine>();
        });

        private string _baseURI;
        private Dictionary<string, string> _headers;
        private KeyValuePair<string, string>? _authorization;
        private List<IUnityNativeRequestInterceptor> _requestInterceptors;
        private List<IUnityNativeResponseInterceptor> _responseInterceptors;

        private UnityNativeNetworkEngine() { }
        
        internal static UnityNativeNetworkEngine Instance => _instance.Value;

        internal UnityNativeNetworkEngine SetBaseURI(string baseUri) {
            _baseURI = baseUri;
            return this;
        }

        internal UnityNativeNetworkEngine ApplyHeaders(Dictionary<string, string> headers) {
            _headers = headers;
            return this;
        }

        internal UnityNativeNetworkEngine ApplyAuthorization(KeyValuePair<string, string>? authorization) {
            _authorization = authorization;
            return this;
        }

        internal UnityNativeNetworkEngine ApplyRequestInterceptors(List<IUnityNativeRequestInterceptor> requestInterceptors) {
            _requestInterceptors = requestInterceptors;
            return this;
        }

        internal UnityNativeNetworkEngine ApplyResponseInterceptors(List<IUnityNativeResponseInterceptor> responseInterceptors) {
            _responseInterceptors = responseInterceptors;
            return this;
        }

        internal UnityNativeResponse ExecuteRequest(UnityNativeRequest request) {
            // TODO : Build UnityWebRequest, apply headers, authorization, interceptors,...
            var unityWebRequest = request.BuildRequest(_baseURI);
            // Yield with corutine?

            return new UnityNativeResponse(request, System.Net.HttpStatusCode.OK, new Dictionary<string, string>(), "content", null);
        }
    }
}
#endif