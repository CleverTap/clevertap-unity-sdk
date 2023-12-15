#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal class UnityNativePlatformVariable : CleverTapPlatformVariable {
        internal override void SyncVariables() {
            CleverTapLogger.LogError("CleverTap Error: SyncVariables is not supported for this platform.");
        }

<<<<<<< HEAD
        internal override void SyncVariables(bool isProduction) {
            CleverTapLogger.LogError("CleverTap Error: SyncVariables is not supported for this platform.");
        }

=======
>>>>>>> 58f5a2b (Feature/sdk 3323/variables bindings (#54))
        internal override void FetchVariables(int callbackId) {
            CleverTapLogger.LogError("CleverTap Error: FetchVariables is not supported for this platform.");
        }

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
