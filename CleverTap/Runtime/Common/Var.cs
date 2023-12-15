<<<<<<< HEAD
﻿using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Common {
=======
﻿namespace CleverTapSDK.Common {
>>>>>>> 58f5a2b (Feature/sdk 3323/variables bindings (#54))
    internal interface IVar {
        string Name { get; }
        string Kind { get; }
        void ValueChanged();
    }

    public abstract class Var<T> : IVar {
        protected string name;
        protected string kind;
        protected T value;
        protected T defaultValue;

        public virtual event CleverTapCallbackDelegate OnValueChanged;

        public Var(string name, string kind, T defaultValue) {
            this.name = name;
            this.kind = kind;
            this.defaultValue = value = defaultValue;
        }

        public virtual string Kind => kind;
        public virtual string Name => name;
        public virtual T Value => value;
        public virtual T DefaultValue => defaultValue;
<<<<<<< HEAD
        public virtual string StringValue => Kind == CleverTapVariableKind.DICTIONARY ? Json.Serialize(Value) : Value.ToString();
=======
>>>>>>> 58f5a2b (Feature/sdk 3323/variables bindings (#54))

        public virtual void ValueChanged() {
            if (OnValueChanged != null) {
                OnValueChanged();
            }
        }

        public static implicit operator T(Var<T> var) {
            return var.Value;
        }
    }
}
