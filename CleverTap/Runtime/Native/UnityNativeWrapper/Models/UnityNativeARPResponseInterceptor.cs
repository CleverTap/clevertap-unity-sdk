using System;
using System.Collections.Generic;
using CleverTapSDK.Native;
using CleverTapSDK.Utilities;

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
namespace Native.UnityNativeWrapper.Models
{
    public class UnityNativeARPResponseInterceptor : IUnityNativeResponseInterceptor
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
            var result = Json.Deserialize(response.Content) as Dictionary<string, object>;
            try
            {
                if (result.ContainsKey("arp"))
                    if (Json.Deserialize(result["arp"].ToString()) is Dictionary<string, object> { Count: > 0 } arp)
                    {
                        //Handle Discarded events in ARP
                        try
                        {
                            ProcessDiscardedEventsList(arp);
                        }
                        catch (Exception t)
                        {
                            CleverTapLogger.Log("Error handling discarded events response: " + t.StackTrace);
                        }

                        HandleARPUpdate(arp);
                    }
            }
            catch (Exception exception)
            {
                CleverTapLogger.Log("Failed to process ARP");
            }

            return response;
        }


        private void HandleARPUpdate(Dictionary<string, object> arp)
        {
            if (arp == null || arp.Count == 0 || string.IsNullOrEmpty(_namespaceARPKey))
                return;

            UnityNativePreferenceManager preferenceManager = UnityNativePreferenceManager.GetPreferenceManager(_accountId);
            Dictionary<string, object> oldARP = Json.Deserialize(preferenceManager.GetString(_namespaceARPKey, string.Empty)) as Dictionary<string, object>;
            if (oldARP == null || oldARP.Count == 0)
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
                        oldARP[key] = i;
                        break;
                    case long l:
                        oldARP[key] = l;
                        break;
                    case float f:
                        oldARP[key] = f;
                        break;
                    case double d:
                        oldARP[key] = d;
                        break;
                    case string s:
                        oldARP[key] = s;
                        break;
                    case bool b:
                        oldARP[key] = b;
                        break;
                    default:
                        CleverTapLogger.Log($"ARP update for key {key} rejected (invalid data type)");
                        break;
                }
            }

            preferenceManager.SetString(_namespaceARPKey, Json.Serialize(oldARP));
        }


        private void ProcessDiscardedEventsList(Dictionary<string, object> response)
        {
            if (!response.ContainsKey(UnityNativeConstants.EventMeta.DISCARDED_EVENT_JSON_KEY))
            {
                CleverTapLogger.Log("ARP doesn't contain the Discarded Events key");
                return;
            }

            try
            {
                var discardedEventsList = new List<string>();
                if (Json.Deserialize(response[UnityNativeConstants.EventMeta.DISCARDED_EVENT_JSON_KEY].ToString()) is
                    List<string> discardedEventsArray) discardedEventsList.AddRange(discardedEventsArray);
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