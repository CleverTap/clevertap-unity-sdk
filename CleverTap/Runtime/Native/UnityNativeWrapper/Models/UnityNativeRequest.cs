#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CleverTapSDK.Native {
    internal class UnityNativeRequest {
        private readonly string _path;
        private readonly string _method;

        private Dictionary<string, string> _parameters;
        private Dictionary<string, string> _headers;
        private KeyValuePair<string, string>? _authorization;
        private List<IUnityNativeRequestInterceptor> _requestInterceptors;
        private List<IUnityNativeResponseInterceptor> _responseInterceptors;
        private Dictionary<string, string> _addtionalProperties;

        internal UnityNativeRequest(string path, string method, Dictionary<string, string> addtionalProperties = null) {
            _path = path;
            _method = method;
            _addtionalProperties = addtionalProperties;
        }

        internal Dictionary<string, string> Parameters => _parameters;
        internal Dictionary<string, string> Headers => _headers;
        internal KeyValuePair<string, string>? Authorization => _authorization;
        internal List<IUnityNativeRequestInterceptor> RequestInterceptors => _requestInterceptors;
        internal List<IUnityNativeResponseInterceptor> ResponseInterceptors => _responseInterceptors;
        internal Dictionary<string, string> AddtionalProperties => _addtionalProperties;

        internal UnityNativeRequest SetParameters(Dictionary<string, string> parameters) {
            _parameters = parameters;
            return this;
        }

        internal UnityNativeRequest SetHeaders(Dictionary<string, string> headers) {
            _headers = headers;
            return this;
        }

        internal UnityNativeRequest SetAuthorization(KeyValuePair<string, string>? authorization) {
            _authorization = authorization;
            return this;
        }

        internal UnityNativeRequest SetRequestInterceptors(List<IUnityNativeRequestInterceptor> requestInterceptors) {
            _requestInterceptors = requestInterceptors;
            return this;
        }

        internal UnityNativeRequest SetResponseInterceptors(List<IUnityNativeResponseInterceptor> responseInterceptors) {
            _responseInterceptors = responseInterceptors;
            return this;
        }

        internal UnityWebRequest BuildRequest(string baseURI) {
            // TODO: combine baseURI and path to get full route
            // Create UnityWebRequest and add parameters based on method (query if GET and body if POST)
            return new UnityWebRequest("Route", "POST");
        }
    }
}
#endif