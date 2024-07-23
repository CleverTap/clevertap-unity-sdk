#if !UNITY_IOS && !UNITY_ANDROID
using System;
using UnityEngine;

namespace CleverTapSDK.Native {
    internal class UnityNativeSession {
        private long _sessionId;
        private bool _isFirstSession;
        private long _lastSessionLength;

        private bool _isAppLaunched;
        private long _lastUpdateTimestamp;

        internal UnityNativeSession()
        {
            _isAppLaunched = false;
        }

        internal void Initialize()
        {
            long now = GetNow();
            _sessionId = now;
            _lastUpdateTimestamp = now;

            // Use string to get/set long values in PlayerPrefs
            long lastSessionId = long.Parse(PlayerPrefs.GetString(GetStorageKey(UnityNativeConstants.Session.SESSION_ID), "0"));
            if (lastSessionId == 0)
            {
                _isFirstSession = true;
            }

            long lastSessionTime = long.Parse(PlayerPrefs.GetString(GetStorageKey(UnityNativeConstants.Session.LAST_SESSION_TIME), "0"));
            if (lastSessionTime > 0)
            {
                _lastSessionLength = lastSessionTime - lastSessionId;
            }

            PlayerPrefs.SetString(GetStorageKey(UnityNativeConstants.Session.SESSION_ID), _sessionId.ToString());
            HasInitialized = true;
        }

        internal bool HasInitialized { get; private set; }
        internal long SessionId => _sessionId;
        internal long LastSessionLength => _lastSessionLength;
        internal bool IsFirstSession => _isFirstSession;
        internal bool IsAppLaunched => _isAppLaunched;
        internal long LastUpdateTimestamp => _lastUpdateTimestamp;

        internal void SetIsAppLaunched(bool isAppLaunched) {
            _isAppLaunched = isAppLaunched;
        }

        internal long UpdateTimestamp() {
            long now = GetNow();
            PlayerPrefs.SetString(GetStorageKey(UnityNativeConstants.Session.LAST_SESSION_TIME), now.ToString());
            _lastUpdateTimestamp = now;
            return _lastUpdateTimestamp;
        }

        internal string GetStorageKey(string suffix) {
            return UnityNativeConstants.GetStorageKeyWithAccountId(suffix);
        }

        internal long GetNow() {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
#endif