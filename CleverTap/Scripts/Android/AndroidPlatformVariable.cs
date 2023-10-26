#if UNITY_ANDROID
using CleverTap.Common;
using CleverTap.Utilities;

namespace CleverTap.Android {
    internal class AndroidPlatformVariable : CleverTapPlatformVariable {
        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("defineVar", name, kind, Json.Serialize(defaultValue));
            Var<T> result = new AndroidVar<T>(name, kind, defaultValue);
            varCache.Add(name, result);
            return result;
        }
    }
}
#endif