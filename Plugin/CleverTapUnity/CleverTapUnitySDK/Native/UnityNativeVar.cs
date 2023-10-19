#if !UNITY_IOS && !UNITY_ANDROID
using CleverTap.Common;

namespace CleverTap.Native {
    internal class UnityNativeVar<T> : Var<T> {
        internal UnityNativeVar(string name, string kind, T defaultValue, string fileName = "") : base(name, kind, defaultValue, fileName) {
        }
    }
}
#endif