#if !UNITY_IOS && !UNITY_ANDROID
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeSessionManager {
        private const long SESSION_LENGTH_SECONDS = 20 * 60;

        private UnityNativeSession _currentSession;

        internal UnityNativeSessionManager() {
            _currentSession = new UnityNativeSession();
        }

        internal Guid GetSessionId() {
            if (IsSessionExpired()) {
                _currentSession = new UnityNativeSession(_currentSession.UserIdentity);
            }

            return _currentSession.SessionId;
        }

        internal string GetSessionUserIdentity() {
            if (IsSessionExpired()) {
                _currentSession = new UnityNativeSession(_currentSession.UserIdentity);
            }

            return _currentSession.UserIdentity;
        }

        internal void UpdateSessionUserIdentity(string userIdentity) {
            if (IsSessionExpired()) {
                _currentSession = new UnityNativeSession(_currentSession.UserIdentity);
            }

            if (_currentSession.UserIdentity == null) {
                _currentSession.SetUserIdentity(userIdentity);
                _currentSession.UpdateTimestamp();
            } else if (_currentSession.UserIdentity != null && _currentSession.UserIdentity != userIdentity) {
                _currentSession = new UnityNativeSession(userIdentity);
            }
        }

        internal void UpdateSessionTimestamp() {
            if (IsSessionExpired()) {
                _currentSession = new UnityNativeSession(_currentSession.UserIdentity);
            }

            _currentSession?.UpdateTimestamp();
        }

        private bool IsSessionExpired() {
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return currentTimestamp - _currentSession.LastUpdateTimestamp > SESSION_LENGTH_SECONDS;
        }
    }
}
#endif