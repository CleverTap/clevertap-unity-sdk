#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativePlatformVariable : CleverTapPlatformVariable
    {
        internal override void SyncVariables()
        {
            CleverTapLogger.LogError("CleverTap Error: SyncVariables is not supported for this platform.");
        }

        internal override void SyncVariables(bool isProduction)
        {
            CleverTapLogger.LogError("CleverTap Error: SyncVariables is not supported for this platform.");
        }

        internal override void FetchVariables(int callbackId)
        {
            CleverTapLogger.LogError("CleverTap Error: FetchVariables is not supported for this platform.");
        }

        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue)
        {
            UnityNativeVar<T> result = new UnityNativeVar<T>(name, kind, defaultValue);
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
