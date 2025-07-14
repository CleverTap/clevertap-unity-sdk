#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR 
using System.Collections.Generic;
using System.Net;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeDisplayResponseInterceptor : IUnityNativeResponseInterceptor
    {
        internal List<object> displayUnits = null;
        private readonly UnityNativeEventManager _unityNativeEventManager = null;

        public UnityNativeDisplayResponseInterceptor(UnityNativeEventManager unityNativeEventManager)
        {
            _unityNativeEventManager = unityNativeEventManager;
        }

        UnityNativeResponse IUnityNativeResponseInterceptor.Intercept(UnityNativeResponse response)
        {
            this.displayUnits = null;

            if (response == null || response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                // Response or response content is null. This is the case for the handshake response.
                return response;
            }

            Dictionary<string, object> result = null;

            try
            {
                result = Json.Deserialize(response.Content) as Dictionary<string, object>;
            }
            catch (System.Exception ex)
            {
                CleverTapLogger.LogError($"Failed to deserialize display units response: {ex.Message}");
                return response;
            }

            if (result != null && result.ContainsKey(UnityNativeConstants.NativeDisplay.DISPLAY_UNIT_KEY))
            {
                if (result[UnityNativeConstants.NativeDisplay.DISPLAY_UNIT_KEY] is List<object> units)
                {
                    if (units.Count == 0)
                    {
                        return response;
                    }
                    else
                    {
                        HandleDisplayUnits(units);
                    }
                }
            }

            return response;
        }

        public void HandleDisplayUnits(List<object> displayUnits)
        {
            this.displayUnits = displayUnits;
            _unityNativeEventManager?.UpdateDisplayUnits(displayUnits);
        }
    }
}
#endif