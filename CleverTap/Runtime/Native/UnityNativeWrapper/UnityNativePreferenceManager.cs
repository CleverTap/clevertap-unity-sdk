#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Native {
        internal class UnityNativePreferenceManager {

        private List<string> _sessionKeys = new List<string>();

        internal UnityNativePreferenceManager() {
        }

        internal void ClearAllPreffs() {
            PlayerPrefs.DeleteAll();
            _sessionKeys.Clear();
        }

        internal string GetGUIDForIdentifier(string key, string identifier)
        {
            string cachedGUID = null;
            string cachedIdentities = GetUserIdentities();
            if (string.IsNullOrEmpty(cachedIdentities))
            {
                return null;
            }
            string identKey = GetKeyIdentifier(key, identifier);
            Dictionary<string, object> cachedValues = Json.Deserialize(cachedIdentities) as Dictionary<string, object>;
            if (cachedValues.ContainsKey(identKey))
            {
                cachedGUID = cachedValues[identKey].ToString();
            }
            return cachedGUID;
        }

        internal string GetUserIdentities() {
           return PlayerPrefs.GetString(GetStorageKey(UnityNativeConstants.SDK.CACHED_GUIDS_KEY), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="key">Key as in Email or Identity</param>
        /// <param name="identifier"> identifier value like abc@efg.com or 1212sdsk</param>
        internal void SetGUIDForIdentifier(string guid, string key, string identifier) {
            string cachedIdentities = GetUserIdentities();
            Dictionary<string, object> cachedValues;
            if (string.IsNullOrEmpty(cachedIdentities))
            {
                cachedValues = new Dictionary<string, object>();
            }
            else
            {
                cachedValues = Json.Deserialize(cachedIdentities) as Dictionary<string, object>;
            }

            cachedValues[GetKeyIdentifier(key, identifier)] = guid;
            PlayerPrefs.SetString(GetStorageKey(UnityNativeConstants.SDK.CACHED_GUIDS_KEY),
                Json.Serialize(cachedValues));
        }

        internal string GetStorageKey(string suffix) {
            return UnityNativeConstants.GetStorageKeyWithAccountId(suffix);
        }

        internal string GetKeyIdentifier(string key, string identifier) {
            return string.Format("{0}_{1}", key, identifier);
        }
    }
}
#endif