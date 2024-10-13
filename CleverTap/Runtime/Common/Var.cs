using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Common {
    internal interface IVar {
        string Name { get; }
        string Kind { get; }
        void ValueChanged();
        bool IsFileReady { get; }
    }

    public abstract class Var<T> : IVar {
        protected string name;
        protected string kind;
        protected T value;
        protected T defaultValue;
        protected bool isFileReady;
        
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
        public virtual string StringValue => Kind == CleverTapVariableKind.DICTIONARY ? Json.Serialize(Value) : Value.ToString();
        public virtual bool IsFileReady => Kind == CleverTapVariableKind.FILE && isFileReady;
        public string FileValue => Kind == CleverTapVariableKind.FILE ? StringValue : null;
        
        public virtual void ValueChanged()
        {
            if (Kind.Equals(CleverTapVariableKind.FILE))
                isFileReady = true;
            
            if (OnValueChanged != null) {
                OnValueChanged();
            }
        }

        public static implicit operator T(Var<T> var) {
            return var.Value;
        }
    }
}
