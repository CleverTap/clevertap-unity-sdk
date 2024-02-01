#if !UNITY_IOS && !UNITY_ANDROID
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeSession {
        private readonly Guid _sessionId;
        private readonly long _startTimestamp;
        
        private long _lastUpdateTimestamp;
        private string _userIdentity;

        internal UnityNativeSession(string userIdentity = null)
        {
            _sessionId = Guid.NewGuid();
            _startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _lastUpdateTimestamp = _startTimestamp;
            _userIdentity = userIdentity;
        }
        
        internal Guid SessionId => _sessionId;
        internal long StartTimestamp => _startTimestamp;
        internal long LastUpdateTimestamp => _lastUpdateTimestamp;
        internal string UserIdentity => _userIdentity;

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