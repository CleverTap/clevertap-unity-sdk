#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR 
using System;
using System.Collections.Generic;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeAppInboxResponseInterceptor : IUnityNativeResponseInterceptor
    {
        private readonly UnityNativeEventManager _unityNativeEventManager = null;

        public UnityNativeAppInboxResponseInterceptor(UnityNativeEventManager unityNativeEventManager)
        {
            _unityNativeEventManager = unityNativeEventManager;
        }

        UnityNativeResponse IUnityNativeResponseInterceptor.Intercept(UnityNativeResponse response)
        {
            if (response == null || string.IsNullOrEmpty(response.Content))
            {
                // Response or response content is null. This is the case for the handshake response.
                return response;
            }

            Dictionary<string, object> result = Json.Deserialize(response.Content) as Dictionary<string, object>;

            try
            {
                if (result != null && result.ContainsKey(UnityNativeConstants.AppInbox.INBOX_NOTIFS_KEY))
                {
                    List<object> messages = (List<object>)result[UnityNativeConstants.AppInbox.INBOX_NOTIFS_KEY];
                    
                    if (messages == null || messages.Count == 0)
                    {
                        return response;
                    }

                    HandleAppInboxMessages(messages);
                }
            }
            catch (Exception exception)
            {
                CleverTapLogger.Log($"Failed to process App Inbox, Exception: {exception.Message}, Stack Trace: {exception.StackTrace}");
            }

            return response;
        }

        public void HandleAppInboxMessages(List<object> appInboxMessages)
        {
            _unityNativeEventManager?.OnInboxMessagesReceived(appInboxMessages);
        }
    }
}
#endif