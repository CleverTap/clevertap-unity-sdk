#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR 
using System;
using System.Collections.Generic;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeMetadataResponseInterceptor : IUnityNativeResponseInterceptor
    {
        private readonly UnityNativePreferenceManager _preferenceManager;
        private readonly string _accountId;
        public UnityNativeMetadataResponseInterceptor(string accountId, UnityNativePreferenceManager preferenceManager)
        {
            _accountId = accountId;
            _preferenceManager = preferenceManager;
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
                    SetI(i);
                }
            } catch (Exception t) {
                CleverTapLogger.Log("Error parsing _i values, " + t.StackTrace);
            }

            // Handle j
            try {
                if (responseContent.TryGetValue("_j", out var value)) {
                    long j = long.Parse(value.ToString());
                    SetJ(j);
                }
            } catch (Exception t) {
                CleverTapLogger.Log("Error parsing _j values, " + t.StackTrace);
            }

            return response;
        }
        
        public void SetI(long l)
        {
            string tempKey = $"{UnityNativeConstants.EventMeta.KEY_I}:{_accountId}";
            _preferenceManager.SetLong(tempKey,l);
        }
    
        public void SetJ(long l)
        {
            string tempKey = $"{UnityNativeConstants.EventMeta.KEY_J}:{_accountId}";
            _preferenceManager.SetLong(tempKey,l);
        }
    }
}
#endif