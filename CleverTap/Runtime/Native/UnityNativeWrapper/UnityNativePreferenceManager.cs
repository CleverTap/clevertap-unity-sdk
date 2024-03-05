#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using UnityEngine;

namespace CleverTapSDK.Native {
    // TODO: This class probbably need to have more access
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
    }
}
#endif