using System.Linq;
using System.Text.RegularExpressions;

namespace CleverTap.Common {
    internal interface IVar {
        string Name { get; }
        string Kind { get; }
        string FileName { get; }
        void ValueChanged();
    }

    public abstract class Var<T> : IVar {
        protected const string NAME_COMPONENT_PATTERN = "(?:[^\\.\\[.(\\\\]+|\\\\.)+";

        protected string name;
        protected string kind;
        protected string fileName;
        protected T value;
        protected T defaultValue;

        public virtual event CleverTapCallbackDelegate OnValueChanged;

        public Var(string name, string kind, T defaultValue, string fileName = "") {
            this.name = name;
            this.kind = kind;
            this.fileName = fileName;
            this.defaultValue = value = defaultValue;
        }

        public virtual string Kind => kind;
        public virtual string Name => name;
        public virtual string FileName => fileName;
        public virtual T Value => value;
        public virtual T DefaultValue => defaultValue;
        public virtual string[] NameComponents =>
            Regex.Matches(name, NAME_COMPONENT_PATTERN)
                 .Select(x => x.ToString())
                 .ToArray();

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
