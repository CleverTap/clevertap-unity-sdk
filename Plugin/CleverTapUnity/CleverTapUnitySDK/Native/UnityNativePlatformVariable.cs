#if !UNITY_IOS && !UNITY_ANDROID
using CleverTap.Common;
using CleverTap.Utilities;

namespace CleverTap.Native {
    internal class UnityNativePlatformVariable : CleverTapPlatformVariable {
        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue) {
            CleverTapLogger.LogError("CleverTap Error: Define is not supported for this platform.");
            return null;
        }

        protected override Var<T> GetOrDefineVariable<T>(string name, T defaultValue) {
            CleverTapLogger.LogError("CleverTap Error: Define is not supported for this platform.");
            return null;
        }
    }
}
#endif
