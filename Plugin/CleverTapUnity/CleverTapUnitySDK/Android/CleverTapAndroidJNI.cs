﻿#if UNITY_ANDROID
using UnityEngine;

namespace CleverTap.Android {
    internal static class CleverTapAndroidJNI {
        private const string UNITY_PLAYER_CLASS = "com.unity3d.player.UnityPlayer";
        private const string CLEVERTAP_UNITY_PLUGIN_CLASS = "com.clevertap.unity.CleverTapUnityPlugin";
        private const string GET_APPLICATION_CONTEXT_METHOD = "getApplicationContext";
        private const string GET_INSTANCE_METHOD = "getInstance";

        private static AndroidJavaObject _unityActivity;
        private static AndroidJavaObject _cleverTapJNI;
        private static AndroidJavaObject _cleverTapClass;

        public static AndroidJavaObject UnityActivity {
            get {
                if (_unityActivity == null) {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass(UNITY_PLAYER_CLASS)) {
                        _unityActivity = unityPlayer.GetStatic<AndroidJavaObject>(UNITY_PLAYER_CLASS);
                    }
                }
                return _unityActivity;
            }
        }

        public static AndroidJavaObject CleverTapClass {
            get {
                if (_cleverTapClass == null) {
                    _cleverTapClass = new AndroidJavaClass(CLEVERTAP_UNITY_PLUGIN_CLASS);
                }
                return _cleverTapClass;
            }
        }

        public static AndroidJavaObject ApplicationContext =>
            UnityActivity.Call<AndroidJavaObject>(GET_APPLICATION_CONTEXT_METHOD);

        public static AndroidJavaObject CleverTapJNI {
            get {
                if (_cleverTapJNI == null) {
                    _cleverTapJNI = CleverTapClass.CallStatic<AndroidJavaObject>(GET_INSTANCE_METHOD, ApplicationContext);
                }
                return _cleverTapJNI;
            }
        }
    }
}
#endif