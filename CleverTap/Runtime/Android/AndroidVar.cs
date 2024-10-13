﻿#if UNITY_ANDROID
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using System;
using System.Collections;
using CleverTapSDK.Constants;

namespace CleverTapSDK.Android {
    internal class AndroidVar<T> : Var<T> {
        internal AndroidVar(string name, string kind, T defaultValue) : base(name, kind, defaultValue) {}

        public override T Value {
            get {
                string jsonRepresentation = CleverTapAndroidJNI.CleverTapJNIInstance.Call<string>("getVariableValue", name);
                if (jsonRepresentation == Json.Serialize(value)) {
                    return value;
                }

                object newValue = Json.Deserialize(jsonRepresentation);
                if (kind.Equals(CleverTapVariableKind.FILE))
                {
                    if (typeof(T) != typeof(string)) {
                        return (T)(object)jsonRepresentation;
                    }
                    CleverTapLogger.LogError("Expected type string for FILE kind, but got " + typeof(T));
                }
                else if (newValue is IDictionary) {
                    Util.FillInValues(newValue, value);
                } else if (newValue == null) {
                    value = defaultValue;
                } else {
                    value = (T)Convert.ChangeType(newValue, typeof(T));
                }
                return value;
            }
        }
    }
}
#endif