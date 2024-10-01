#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using CleverTapSDK.Common;

namespace CleverTapSDK.Native {
    internal class UnityNativeVar<T> : Var<T> {
        internal string[] nameComponents;

        public string stringValue;

        private double numberValue;

        private T defaultValue;

        private T value;
        internal UnityNativeVar(string name, string kind, T defaultValue) : base(name, kind, defaultValue) {
        }
    }
}
#endif