#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeSessionManager {
        private static readonly Lazy<UnityNativeSessionManager> instance = new Lazy<UnityNativeSessionManager>(() => new UnityNativeSessionManager());
        
        private const long SESSION_LENGTH_SECONDS = 20 * 60;
        
        private UnityNativeSession _currentSession;

        private UnityNativeSessionManager() {
            _currentSession = new UnityNativeSession();
        }

        internal static UnityNativeSessionManager Instance => instance.Value;

        public UnityNativeSession CurrentSession {
            get {
                if (_currentSession.HasInitialized && IsSessionExpired()) {
                    ResetSession();
                }

                return _currentSession;
            }
        }

        /// <summary>
        /// Initializes the current session.
        /// Requires AccountInfo to be set.
        /// </summary>
        internal void InitializeSession() {
            CurrentSession.Initialize();
        }

        internal void ResetSession() {
            _currentSession = new UnityNativeSession();
            _currentSession.Initialize();
        }

        internal bool IsFirstSession() {
            return _currentSession.IsFirstSession;
        }

        /// <summary>
        /// Used for Page events only.
        /// Increment when RecordScreenView is called.
        /// Not supported yet.
        /// </summary>
        /// <returns>The screens count.</returns>
        internal int GetScreenCount() {
            return 1;
        }

        /// <summary>
        /// The current screen name.
        /// Equivalent to the current Activity name on Android
        /// and the current ViewController on iOS.
        /// Not supported yet.
        /// </summary>
        /// <returns>The name of the current screen.</returns>
        internal string GetScreenName()
        {
            return string.Empty;
        }

        internal long GetLastSessionLength() {
            return _currentSession.LastSessionLength;
        }

        internal void UpdateSessionTimestamp() {
            if (IsSessionExpired()) {
                ResetSession();
            }

            _currentSession?.UpdateTimestamp();
        }

        private bool IsSessionExpired() {
            return _currentSession.GetNow() - _currentSession.LastUpdateTimestamp > SESSION_LENGTH_SECONDS;
        }
    }
}
#endif