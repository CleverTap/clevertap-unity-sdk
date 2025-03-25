#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Native
{
    internal class UnityNativePlatformVariable : CleverTapPlatformVariable
    {
        private readonly UnityNativeVarCache nativeVarCache = new UnityNativeVarCache();
        private readonly UnityNativeWrapper unityNativeWrapper;

        internal UnityNativePlatformVariable(UnityNativeWrapper unityNativeWrapper) : base()
        {
            this.unityNativeWrapper = unityNativeWrapper;
        }

        internal override void SyncVariables()
        {
            SyncVariables(false);
        }

        internal override void SyncVariables(bool isProduction)
        {
            if (isProduction)
            {
                if (Debug.isDebugBuild)
                {
                    CleverTapLogger.Log("Calling SyncVariables(true) from Debug build. Do not use (isProduction: true) in this case.");
                }
                else
                {
                    CleverTapLogger.Log("Calling SyncVariables(true) from Release build. Do not release this build and use with caution.");
                }
            }
            else
            {
                if (!Debug.isDebugBuild)
                {
                    CleverTapLogger.Log("SyncVariables() can only be called from Debug builds.");
                    return;
                }
            }

            if (unityNativeWrapper != null)
            {
                if (nativeVarCache.VariablesCount == 0)
                {
                    CleverTapLogger.Log("CleverTap: No Variables defined.");
                }
                unityNativeWrapper.SyncVariables(nativeVarCache.GetDefineVarsPayload());
            }
            else
            {
                CleverTapLogger.LogError("CleverTap Error: Cannot sync variables. The unityNativeWrapper is null.");
            }
        }

        internal override void FetchVariables(int callbackId)
        {
            CleverTapLogger.LogError("CleverTap Error: FetchVariables is not supported for this platform.");
        }

        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue)
        {
            UnityNativeVar<T> result = new UnityNativeVar<T>(name, kind, defaultValue, nativeVarCache);
            varCache.Add(name, result);
            return result;
        }

        protected override Var<T> GetOrDefineVariable<T>(string name, T defaultValue)
        {
            var variable = base.GetOrDefineVariable<T>(name, defaultValue);
            return variable;
        }

        protected override Var<string> GetOrDefineFileVariable(string name)
        {
            CleverTapLogger.LogError("CleverTap Error: File Variables are not supported for this platform.");
            return null;
        }
    }
}
#endif
