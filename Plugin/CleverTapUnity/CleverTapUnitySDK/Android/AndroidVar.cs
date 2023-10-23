#if UNITY_ANDROID
using CleverTap.Common;
using CleverTap.Utilities;
using System;
using System.Collections;
using System.Linq;

namespace CleverTap.Android {
    internal class AndroidVar<T> : Var<T> {
        internal AndroidVar(string name, string kind, T defaultValue, string fileName = "") : base(name, kind, defaultValue, fileName) {
        }

        public override string[] NameComponents {
            get {
                string jsonRepresentation = CleverTapAndroidJNI.CleverTapClass.CallStatic<string>("getVariableNameComponents", name);
                string[] result = new string[jsonRepresentation.Count(x => x == ',') + 1];
                Util.FillInValues(Json.Deserialize(jsonRepresentation), result);
                return result;
            }
        }

        public override T Value {
            get {
                string jsonRepresentation = CleverTapAndroidJNI.CleverTapClass.CallStatic<string>("getVariableValue", name);
                if (jsonRepresentation == Json.Serialize(value)) {
                    return value;
                }

                object newValue = Json.Deserialize(jsonRepresentation);
                if (newValue is IDictionary || newValue is IList) {
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