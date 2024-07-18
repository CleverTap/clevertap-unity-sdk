#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace CleverTapSDK.Native {
        internal class UnityNativePreferenceManager {

        private List<string> _sessionKeys = new List<string>();

        internal UnityNativePreferenceManager() {

        }

        internal void GetSessionPreff(string key) {
            PlayerPrefs.GetString($"session-{key}", null);
        }

        internal void AddSessionPreff(string key, string value) {
            PlayerPrefs.SetString($"session-{key}", value);
            _sessionKeys.Add($"session-{key}");
        }

        internal void RemoveSessionPreff(string key) {
            PlayerPrefs.DeleteKey($"session-{key}");
            _sessionKeys.Remove($"session-{key}");
        }

        internal void ClearAllPreffs() {
            PlayerPrefs.DeleteAll();
            _sessionKeys.Clear();
        }

        internal string GetGUIDForIdentifier(string key, string identifier)
        {
           string cachedGUID = null;
           string cachedIdentities = GetUserIdentities();
           if(string.IsNullOrEmpty(cachedIdentities)){
                return null;
           }
           string identKey = string.Format("{0}_{1}",key,identifier);
           Dictionary<string,string> cachedValues = JsonConvert.DeserializeObject<Dictionary<string,string>>(cachedIdentities) as Dictionary<string,string>;
           if(cachedValues.ContainsKey(identKey)){
                cachedGUID = cachedValues[identKey];   
           }
           return cachedGUID;
        }

        internal string GetUserIdentities()
        {
           return PlayerPrefs.GetString(UnityNativeConstants.SDK.CACHED_GUIDS_KEY+UnityNativeAccountManager.Instance.AccountInfo.AccountId,null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="key">Key as in Email or Identity</param>
        /// <param name="identifier"> identifier value like abc@efg.com or 1212sdsk</param>
        public void SetGUIDForIdentifier(string guid,string key,string identifier){
            string cachedIdentities = GetUserIdentities();
            Dictionary<string,string> cachedValues = null;
            if(string.IsNullOrEmpty(cachedIdentities)){
                cachedValues = new Dictionary<string, string>();
           }else
                cachedValues = JsonConvert.DeserializeObject<Dictionary<string,string>>(cachedIdentities) as Dictionary<string,string>;

            cachedValues[string.Format("{0}_{1}",key,identifier)] = guid;
            PlayerPrefs.SetString(UnityNativeConstants.SDK.CACHED_GUIDS_KEY+UnityNativeAccountManager.Instance.AccountInfo.AccountId,
                JsonConvert.SerializeObject(cachedValues));
         
        }
    }
}
#endif