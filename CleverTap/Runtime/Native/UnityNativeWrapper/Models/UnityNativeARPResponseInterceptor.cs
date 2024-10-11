using System;
using System.Collections.Generic;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
namespace Native.UnityNativeWrapper.Models
{
    internal class UnityNativeARPResponseInterceptor : IUnityNativeResponseInterceptor
    {
        private readonly UnityNativeEventValidator _eventValidator;
        private readonly string _accountId;
        private readonly string _namespaceARPKey;
        public UnityNativeARPResponseInterceptor(string accountId,string deviceId, UnityNativeEventValidator eventValidator)
        {
            this._accountId = accountId;
            this._namespaceARPKey = string.Format(UnityNativeConstants.Network.ARP_NAMESPACE_KEY,accountId,deviceId); 
            _eventValidator = eventValidator;
        }

        UnityNativeResponse IUnityNativeResponseInterceptor.Intercept(UnityNativeResponse response)
        {
            if (response == null || string.IsNullOrEmpty(response.Content))
            {
                CleverTapLogger.Log($"Failed to process ARP, Response is Null or Empty!");
                return null;
            } 
            var result = Json.Deserialize(response.Content) as Dictionary<string, object>;
            try
            {
                if (result != null && result.ContainsKey(UnityNativeConstants.Network.ARP_KEY))
                    if (result[UnityNativeConstants.Network.ARP_KEY] is Dictionary<string, object> { Count: > 0 } arp)
                    {
                        //Handle Discarded events in ARP
                        try
                        {
                            ProcessDiscardedEventsList(arp);
                        }
                        catch (Exception t)
                        {
                            CleverTapLogger.Log($"Failed to process ARP discarded events, Exception: {t.Message}, Stack Trace: {t.StackTrace}");
                        }

                        HandleARPUpdate(arp);
                    }
            }
            catch (Exception exception)
            {
                CleverTapLogger.Log($"Failed to process ARP, Exception: {exception.Message}, Stack Trace: {exception.StackTrace}");
            }

            return response;
        }


        private void HandleARPUpdate(Dictionary<string, object> arp)
        {
            if (arp == null || arp.Count == 0 || string.IsNullOrEmpty(_namespaceARPKey))
                return;

            UnityNativePreferenceManager preferenceManager = UnityNativePreferenceManager.GetPreferenceManager(_accountId);
            Dictionary<string, object> currentARP = Json.Deserialize(preferenceManager.GetString(_namespaceARPKey, "{}")) as Dictionary<string, object>;
            
            if (currentARP == null || currentARP.Count == 0)
            {
                preferenceManager.SetString(_namespaceARPKey, Json.Serialize(arp));
                return;
            }
               
            foreach (var keyValuePair in arp)
            {
                string key = keyValuePair.Key;
                object value = keyValuePair.Value;

                switch (value)
                {
                    case int i:
                        currentARP[key] = i;
                        break;
                    case long l:
                        currentARP[key] = l;
                        break;
                    case float f:
                        currentARP[key] = f;
                        break;
                    case double d:
                        currentARP[key] = d;
                        break;
                    case string s:
                        currentARP[key] = s;
                        break;
                    case bool b:
                        currentARP[key] = b;
                        break;
                    default:
                        CleverTapLogger.Log($"ARP update for key {key} rejected (invalid data type)");
                        break;
                }
            }

            preferenceManager.SetString(_namespaceARPKey, Json.Serialize(currentARP));
        }


        private void ProcessDiscardedEventsList(Dictionary<string, object> arp)
        {
            if (!arp.ContainsKey(UnityNativeConstants.EventMeta.DISCARDED_EVENT_JSON_KEY))
            {
                CleverTapLogger.Log("ARP doesn't contain the Discarded Events key");
                return;
            }

            try
            {
                var discardedEventsList = arp[UnityNativeConstants.EventMeta.DISCARDED_EVENT_JSON_KEY] as List<string>;
                if (discardedEventsList == null || discardedEventsList.Count == 0)
                {
                    discardedEventsList = new List<string>();
                }
                if (_eventValidator != null)
                    _eventValidator.SetDiscardedEvents(discardedEventsList);
                else
                    CleverTapLogger.Log("Validator object is NULL");
            }
            catch (Exception e)
            {
                CleverTapLogger.Log("Error parsing discarded events list" + e.StackTrace);
            }
        }
    }
}
#endif