#if UNITY_ANDROID
namespace CleverTapSDK.Android {
    internal abstract class AndroidPluginCallback : UnityEngine.AndroidJavaProxy {

        internal AndroidPluginCallback(): base("com.clevertap.unity.PluginCallback") {}

        internal abstract void Invoke(string message);
    }
}
#endif
