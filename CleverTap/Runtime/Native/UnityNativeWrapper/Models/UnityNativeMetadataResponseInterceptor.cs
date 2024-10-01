using System;
using System.Collections.Generic;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
namespace Native.UnityNativeWrapper
{
    internal class UnityNativeMetadataResponseInterceptor : IUnityNativeResponseInterceptor
    {
        private readonly UnityNativeNetworkEngine _networkEngine;
        public UnityNativeMetadataResponseInterceptor(string accountId, UnityNativeNetworkEngine networkEngine)
        {
            _networkEngine = networkEngine;
        }

        UnityNativeResponse IUnityNativeResponseInterceptor.Intercept(UnityNativeResponse response)
        {
            Dictionary<string,object> responseContent = Json.Deserialize(response.Content) as Dictionary<string,object>;
            if (responseContent == null || responseContent.Count <= 0)
                return response;
            // Handle i
            try {
                if (responseContent.TryGetValue("_i", out var value)) {
                    long i = long.Parse(value.ToString());
                    _networkEngine.SetI(i);
                }
            } catch (Exception t) {
                // Ignore
            }

            // Handle j
            try {
                if (responseContent.TryGetValue("_j", out var value)) {
                    long j = long.Parse(value.ToString());
                    _networkEngine.SetJ(j);
                }
            } catch (Exception t) {
                // Ignore
            }

            return response;
        }
    }
}
#endif