#if !UNITY_IOS && !UNITY_ANDROID
using System;

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

            long lastSessionId = UnityNativePreferenceManager.Instance.GetLong(UnityNativeConstants.Session.SESSION_ID_KEY, 0);
            if (lastSessionId == 0)
            {
                _isFirstSession = true;
            }

            long lastSessionTime = UnityNativePreferenceManager.Instance.GetLong(UnityNativeConstants.Session.LAST_SESSION_TIME_KEY, 0);
            if (lastSessionTime > 0)
            {
                _lastSessionLength = lastSessionTime - lastSessionId;
            }

            UnityNativePreferenceManager.Instance.SetLong(UnityNativeConstants.Session.SESSION_ID_KEY, _sessionId);
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
            UnityNativePreferenceManager.Instance.SetLong(UnityNativeConstants.Session.LAST_SESSION_TIME_KEY, now);
            _lastUpdateTimestamp = now;
            return _lastUpdateTimestamp;
        }

        internal long GetNow() {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
#endif