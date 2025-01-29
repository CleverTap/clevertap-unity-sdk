#if UNITY_ANDROID
namespace CleverTapSDK.Android
{
    internal abstract class AndroidPluginCallback : UnityEngine.AndroidJavaProxy
    {

        internal AndroidPluginCallback() : base("com.clevertap.unity.callback.PluginCallback") { }

        internal abstract void Invoke(string message);
    }

    internal abstract class AndroidPluginIntCallback : UnityEngine.AndroidJavaProxy
    {
        internal AndroidPluginIntCallback() : base("com.clevertap.unity.callback.PluginIntCallback") { }

        internal abstract void Invoke(int value);
    }

    internal abstract class AndroidPluginLongCallback : UnityEngine.AndroidJavaProxy
    {
        internal AndroidPluginLongCallback() : base("com.clevertap.unity.callback.PluginLongCallback") { }

        internal abstract void Invoke(long value);
    }
}
#endif
