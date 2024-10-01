#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal class UnityNativePlatformVariable : CleverTapPlatformVariable {
        internal override void SyncVariables() {
            CleverTapLogger.LogError("CleverTap Error: SyncVariables is not supported for this platform.");
        }

        internal override void SyncVariables(bool isProduction) {
            CleverTapLogger.LogError("CleverTap Error: SyncVariables is not supported for this platform.");
        }

        internal override void FetchVariables(int callbackId) {
            CleverTapLogger.LogError("CleverTap Error: FetchVariables is not supported for this platform.");
        }

        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue)
        {
            Var<T> var = GetOrDefineVariable(name,defaultValue);
            varCache.Add(name, var);
            return var;
        }
        
    }
}
#endif
