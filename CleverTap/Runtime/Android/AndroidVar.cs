#if UNITY_ANDROID
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using System;
using System.Collections;

namespace CleverTapSDK.Android {
    internal class AndroidVar<T> : Var<T> {
        internal AndroidVar(string name, string kind, T defaultValue) : base(name, kind, defaultValue) {}

        public override T Value {
            get {
                string jsonRepresentation = CleverTapAndroidJNI.CleverTapJNIInstance.Call<string>("getVariableValue", name);
                if (jsonRepresentation == null) {
                    return defaultValue;
                }

                if (jsonRepresentation == Json.Serialize(value)) {
                    return value;
                }

                object newValue = Json.Deserialize(jsonRepresentation);

                if (typeof(T) == typeof(string)) {
                    // The Android wrapper returns a string variable unquoted when its content
                    // itself parses as JSON, so it deserializes to a dictionary or a list rather
                    // than to a string. Keep the raw representation in that case - it is the
                    // string that was defined.
                    value = (T)(object)(newValue as string ?? jsonRepresentation);
                } else if (newValue == null) {
                    value = defaultValue;
                } else if (newValue is IDictionary && value is IDictionary) {
                    Util.FillInValues(newValue, value);
                } else {
                    value = (T)Convert.ChangeType(newValue, typeof(T));
                }

                return value;
            }
        }
    }
}
#endif