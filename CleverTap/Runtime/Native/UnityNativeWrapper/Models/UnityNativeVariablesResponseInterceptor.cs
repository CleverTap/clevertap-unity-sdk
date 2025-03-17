#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR 
using System;
using System.Collections.Generic;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeVariablesResponseInterceptor : IUnityNativeResponseInterceptor
    {
        private readonly UnityNativePlatformVariable platformVariable;

        public UnityNativeVariablesResponseInterceptor(UnityNativePlatformVariable platformVariable)
        {
            this.platformVariable = platformVariable;
        }

        UnityNativeResponse IUnityNativeResponseInterceptor.Intercept(UnityNativeResponse response)
        {
            if (response == null || string.IsNullOrWhiteSpace(response.Content))
                return response;

            Dictionary<string, object> responseContent = Json.Deserialize(response.Content) as Dictionary<string, object>;
            if (responseContent == null || responseContent.Count == 0)
                return response;

            try
            {
                if (responseContent.TryGetValue("vars", out var vars))
                {
                    platformVariable.HandleVariablesResponse(vars);
                }
            }
            catch (Exception e)
            {
                CleverTapLogger.Log("Error parsing vars values, " + e.StackTrace);
            }

            return response;
        }
    }
}
#endif