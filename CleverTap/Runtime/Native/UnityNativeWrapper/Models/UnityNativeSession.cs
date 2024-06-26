#if !UNITY_IOS && !UNITY_ANDROID
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeSession {
        private readonly long _sessionId;
        private readonly long _startTimestamp;
        private readonly bool _isFirstSession;
        
        private bool _isAppLaunched;
        private long _lastUpdateTimestamp;
        private string _userIdentity;

        internal UnityNativeSession(bool isFirstSession = false, string userIdentity = null)
        {
            _sessionId =  DateTime.Now.Millisecond / 1000;
            _startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _isFirstSession = isFirstSession;
            _isAppLaunched = false;
            _lastUpdateTimestamp = _startTimestamp;
            _userIdentity = userIdentity;
        }
        
        internal long SessionId => _sessionId;
        internal long StartTimestamp => _startTimestamp;
        internal bool IsFirstSession => _isFirstSession;
        internal bool isAppLaunched => _isAppLaunched;
        internal long LastUpdateTimestamp => _lastUpdateTimestamp;
        internal string UserIdentity => _userIdentity;

        internal void SetIsAppLaunched(bool isAppLaunched) {
            _isAppLaunched = isAppLaunched;
        }

        internal long UpdateTimestamp() {
            _lastUpdateTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return _lastUpdateTimestamp;
        }

        internal void SetUserIdentity(string userIdentity) {
            if (_userIdentity == null) {
                _userIdentity = userIdentity;
                UpdateTimestamp();
            }
        }
    }
}
#endif