#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeVar<T> : Var<T>
    {
        private bool hadStarted;
        internal string[] nameComponents;

        internal UnityNativeVar(string name, string kind, T defaultValue) : base(name, kind, defaultValue)
        {
            // TODO: VarCache registerVariable, update values, merge variable
            nameComponents = GetNameComponents(name);
            Update();
        }

        private CleverTapCallbackDelegate _OnValueChanged;
        public override event CleverTapCallbackDelegate OnValueChanged
        {
            add
            {
                _OnValueChanged += value;
                if (HasVarsRequestCompleted())
                {
                    value();
                }
            }
            remove
            {
                _OnValueChanged -= value;
            }
        }

        internal void Update()
        {
            T oldValue = value;
            // TODO: Set value to VarCache merged value 

            if (value == null && oldValue == null)
            {
                return;
            }
            if (value != null && value.Equals(oldValue) && hadStarted)
            {
                return;
            }

            if (HasVarsRequestCompleted())
            {
                hadStarted = true;
                ValueChanged();
            }
        }

        public override void ValueChanged()
        {
            _OnValueChanged?.Invoke();
        }

        // TODO: Move to Variables or VarCache
        private bool HasVarsRequestCompleted()
        {
            return false;
        }

        // TODO: Move to VarCache or Utils
        internal static string[] GetNameComponents(string name)
        {
            return name.Split(".");
        }

        #region File Variables
        public override event CleverTapCallbackDelegate OnFileReady
        {
            add
            {
                CleverTapLogger.LogError("CleverTap Error: File Variables are not supported for this platform.");
            }
            remove
            {
                CleverTapLogger.LogError("CleverTap Error: File Variables are not supported for this platform.");
            }
        }

        public override void FileIsReady()
        {
            CleverTapLogger.LogError("CleverTap Error: File Variables are not supported for this platform.");
        }

        public override bool IsFileReady => false;
        #endregion

        public override string ToString()
        {
            return $"Var({name}, {value})";
        }
    }
}
#endif