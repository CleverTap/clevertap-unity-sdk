#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CleverTapSDK.Native {
    internal class UnityNativeNetworkEngine : MonoBehaviour {
        private static readonly Lazy<UnityNativeNetworkEngine> _instance = new Lazy<UnityNativeNetworkEngine>(() => {
            var gameObject = new GameObject("UnityNativeNetworkEngine");
            gameObject.AddComponent<UnityNativeNetworkEngine>();
            DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<UnityNativeNetworkEngine>();
        });

        private string _baseURI;
        private int? _timeout;
        private IReadOnlyDictionary<string, string> _headers;
        private KeyValuePair<string, string>? _authorization;
        private IReadOnlyList<IUnityNativeRequestInterceptor> _requestInterceptors;
        private IReadOnlyList<IUnityNativeResponseInterceptor> _responseInterceptors;

        private UnityNativeNetworkEngine() { }

        internal static UnityNativeNetworkEngine Instance => _instance.Value;

        internal UnityNativeNetworkEngine SetBaseURI(string baseUri) {
            _baseURI = baseUri;
            return this;
        }

        internal UnityNativeNetworkEngine SetTimeout(int? timeout) {
            _timeout = timeout;
            return this;
        }

        internal UnityNativeNetworkEngine SetHeaders(Dictionary<string, string> headers) {
            _headers = headers;
            return this;
        }

        internal UnityNativeNetworkEngine SetAuthorization(KeyValuePair<string, string>? authorization) {
            _authorization = authorization;
            return this;
        }

        internal UnityNativeNetworkEngine SetRequestInterceptors(List<IUnityNativeRequestInterceptor> requestInterceptors) {
            _requestInterceptors = requestInterceptors;
            return this;
        }

        internal UnityNativeNetworkEngine SetResponseInterceptors(List<IUnityNativeResponseInterceptor> responseInterceptors) {
            _responseInterceptors = responseInterceptors;
            return this;
        }

        internal async Task<UnityNativeResponse> ExecuteRequest(UnityNativeRequest request) {
            if (request == null) {
                return null;
            }
            
            ApplyNetworkEngineRequestConfiguration(request);
            
            // Intercept request before sending
            if (request.RequestInterceptors?.Count > 0) {
                foreach (var requestInterceptors in request.RequestInterceptors) {
                    request = requestInterceptors.Intercept(request);
                }
            }
            
            var response = await SendRequest(request);
            
            // Intercept reponse before retuning
            if (request.ResponseInterceptors?.Count > 0) {
                foreach (var responseInterceptors in request.ResponseInterceptors) {
                    response = responseInterceptors.Intercept(response);
                }
            }

            return response;
        }

        private void ApplyNetworkEngineRequestConfiguration(UnityNativeRequest request) {
            // Set Headers
            if (_headers?.Count > 0) {
                if (request.Headers == null) {
                    request.SetHeaders(_headers.ToDictionary(x => x.Key, x => x.Value));
                } else {
                    var allHeaders = request.Headers.ToDictionary(x => x.Key, x => x.Value);
                    foreach (var header in _headers) {
                        // Do not overwrite existing headers
                        if (allHeaders.ContainsKey(header.Key)) {
                            allHeaders.Add(header.Key, header.Value);
                        }
                    }
                    request.SetHeaders(allHeaders);
                }
            }

            // Set Timeout
            if (_timeout != null && request.Timeout == null) {
                request.SetTimeout(_timeout);
            }

            // Set Authorization
            if (_authorization.HasValue && !request.Authorization.HasValue) {
                request.SetAuthorization(_authorization.Value);
            }

            // Set Request Interceptors
            if (_requestInterceptors?.Count > 0) {
                if (request.RequestInterceptors == null) {
                    request.SetRequestInterceptors(_requestInterceptors.ToList());
                } else {
                    var allRequestInterceptors = request.RequestInterceptors.ToList();
                    allRequestInterceptors.AddRange(_requestInterceptors.ToList());
                    request.SetRequestInterceptors(allRequestInterceptors);
                }
            }

            // Set Response Interceptors
            if (_responseInterceptors?.Count > 0) {
                if (request.ResponseInterceptors == null) {
                    request.SetResponseInterceptors(_responseInterceptors.ToList());
                } else {
                    var allResponseInterceptors = request.ResponseInterceptors.ToList();
                    allResponseInterceptors.AddRange(_responseInterceptors.ToList());
                    request.SetResponseInterceptors(allResponseInterceptors);
                }
            }
        }

        private async Task<UnityNativeResponse> SendRequest(UnityNativeRequest request) {
            // Check if there is internet connection
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                CleverTapLogger.LogError("Internet connection is not reachable!");
                return new UnityNativeResponse(request, HttpStatusCode.InternalServerError, null, null, "Internet connection is not reachable");
            }

            try {
                var unityWebRequest = request.BuildRequest(_baseURI);

                // Workaround for async
                var unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
                while (!unityWebRequestAsyncOperation.isDone) {
                    await Task.Yield();
                }

                if (unityWebRequest.result == UnityWebRequest.Result.Success) {
                    return new UnityNativeResponse(request, (HttpStatusCode)unityWebRequest.responseCode, unityWebRequest.GetResponseHeaders(), unityWebRequest.downloadHandler.text);
                }
                // TODO : Added other cases (eg. UnityWebRequest.Result.NetworkError)
                else {
                    // Error occured 
                    CleverTapLogger.LogError("Failed");
                    return new UnityNativeResponse(request, (HttpStatusCode)unityWebRequest.responseCode, unityWebRequest.GetResponseHeaders(), unityWebRequest.downloadHandler.text, errorMessage: unityWebRequest.error);
                }

            } catch (Exception ex) {
                CleverTapLogger.LogError("Failed");
                return new UnityNativeResponse(request, HttpStatusCode.InternalServerError, null, null, ex.Message);
            }
        }
    }
}
#endif