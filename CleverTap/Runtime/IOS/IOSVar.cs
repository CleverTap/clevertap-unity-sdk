#if UNITY_IOS
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;
using System;
using System.Collections;

namespace CleverTapSDK.IOS {
    internal class IOSVar<T> : Var<T> {
        internal IOSVar(string name, string kind, T defaultValue) : base(name, kind, defaultValue) {}

        public override T Value {
            get {
                if (kind.Equals(CleverTapVariableKind.FILE)) {
                    string fileValue = IOSDllImport.CleverTap_getFileVariableValue(name);
                    if (typeof(T) == typeof(string))
                    {
                        return (T)Convert.ChangeType(fileValue, typeof(T));
                    }
                    else
                    {
                        CleverTapLogger.LogError("File variables must be of string type");
                        return value;
                    }
                }

                string jsonRepresentation = IOSDllImport.CleverTap_getVariableValue(name);

                if (jsonRepresentation == null) {
                    return defaultValue;
                }

                if (jsonRepresentation == Json.Serialize(value)) {
                    return value;
                }

                object newValue = Json.Deserialize(jsonRepresentation);

                if (typeof(T) == typeof(string)) {
                    // getVariableValue always returns a JSON fragment, so a string variable comes
                    // back quoted and escaped and deserializes to the raw string. Anything else
                    // means the native value was not a string - keep the last known good value
                    // instead of exposing the escaped representation.
                    if (newValue is string stringValue) {
                        value = (T)(object)stringValue;
                    }
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