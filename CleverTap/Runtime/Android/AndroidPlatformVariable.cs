#if UNITY_ANDROID
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Android {
    internal class AndroidPlatformVariable : CleverTapPlatformVariable {
        internal override void SyncVariables() =>
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("syncVariables");

        internal override void SyncVariables(bool isProduction) =>
            SyncVariables();

        internal override void FetchVariables(int callbackId) =>
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("fetchVariables", callbackId);

        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue) {
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("defineVar", name, kind, Json.Serialize(defaultValue));
            Var<T> result = new AndroidVar<T>(name, kind, defaultValue);
            varCache.Add(name, result);
            return result;
        }
        
        protected override Var<string> DefineFileVariable(string name) {
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("defineFileVariable", name);
            Var<string> result = new AndroidVar<string>(name, CleverTapVariableKind.FILE,string.Empty);
            varCache.Add(name, result);
            return result;
        }
    }
}
#endif