#if UNITY_IOS
using CleverTap.Common;
using CleverTap.Constants;
using CleverTap.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace CleverTap.IOS {
    internal class IOSVar<T> : Var<T> {
        internal IOSVar(string name, string kind, T defaultValue, string fileName = "") : base(name, kind, defaultValue, fileName) {            
        }

        public override T Value {
            get {
                string jsonRepresentation = IOSDllImport.CleverTap_getVariableValue(Name, Kind);

                if (jsonRepresentation == null) {
                    return defaultValue;
                }

                if (jsonRepresentation == Json.Serialize(value)) {
                    return value;
                }

                if (Kind == CleverTapVariableKind.FILE) {
                    if (jsonRepresentation != fileName) {
                        fileName = jsonRepresentation;
                    }

                    return (T)Convert.ChangeType(AssetBundle.LoadFromFile(FileName), typeof(T));
                }

                object newValue = Json.Deserialize(jsonRepresentation);

                if (newValue is IDictionary || newValue is IList) {
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