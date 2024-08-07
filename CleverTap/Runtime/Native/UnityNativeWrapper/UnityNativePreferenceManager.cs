﻿#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Native {
        internal class UnityNativePreferenceManager {

        private static Dictionary<string, UnityNativePreferenceManager> instances = new Dictionary<string, UnityNativePreferenceManager>();

        private string _accountId;

        internal UnityNativePreferenceManager(string accountId) {
            _accountId = accountId;
        }

        internal static UnityNativePreferenceManager GetPreferenceManager(string accountId) {
            if (!instances.ContainsKey(accountId))
            {
                instances[accountId] = new UnityNativePreferenceManager(accountId);
            }
            return instances[accountId];
        }

        internal string GetString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(GetStorageKey(key), defaultValue);
        }

        internal void SetString(string key, string value)
        {
            PlayerPrefs.SetString(GetStorageKey(key), value);
        }

        internal float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(GetStorageKey(key), defaultValue);
        }

        internal void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(GetStorageKey(key), value);
        }

        internal int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(GetStorageKey(key), defaultValue);
        }

        internal void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(GetStorageKey(key), value);
        }

        internal long GetLong(string key, long defaultValue)
        {
            // Use string to get long values in PlayerPrefs
            return long.Parse(PlayerPrefs.GetString(GetStorageKey(key), defaultValue.ToString()));
        }

        internal void SetLong(string key, long value)
        {
            // Use string to set long values in PlayerPrefs
            PlayerPrefs.SetString(GetStorageKey(key), value.ToString());
        }

        internal void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(GetStorageKey(key));
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
           return GetString(UnityNativeConstants.SDK.CACHED_GUIDS_KEY, null);
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
            SetString(UnityNativeConstants.SDK.CACHED_GUIDS_KEY,
                Json.Serialize(cachedValues));
        }

        internal string GetKeyIdentifier(string key, string identifier) {
            return string.Format("{0}_{1}", key, identifier);
        }

        internal string GetStorageKey(string suffix) {
            return $"{_accountId}:{suffix}";
        }
    }
}
#endif