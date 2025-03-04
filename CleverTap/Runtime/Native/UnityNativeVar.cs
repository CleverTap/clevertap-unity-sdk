﻿#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using System.Collections;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeVar<T> : Var<T>
    {
        private bool hadStarted;
        private readonly UnityNativeVarCache varCache;
        internal string[] nameComponents;

        public override string[] NameComponents => nameComponents;

        internal UnityNativeVar(string name, string kind, T defaultValue, UnityNativeVarCache varCache) : base(name, kind, defaultValue)
        {
            nameComponents = UnityNativeVariableUtils.GetNameComponents(name);
            if (defaultValue is IDictionary dictionary)
            {
                // Copy defaultValue and set to value.
                // This prevents modifying the defaultValue when updating and merging the value.
                var dest = Util.CreateNewDictionary(dictionary);
                Util.FillInValues(dictionary, dest);
                value = (T)(object)dest;
            }
            this.varCache = varCache;
            this.varCache.RegisterVariable(this);
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

        public override void Update()
        {
            T oldValue = value;
            object newValue = varCache.GetMergedValueFromComponentArray(nameComponents);
            if (newValue == null && oldValue == null)
            {
                return;
            }
            if (newValue != null && newValue.Equals(oldValue) && hadStarted)
            {
                return;
            }

            if (newValue is IDictionary)
            {
                // If the value is a dictionary, copy all the values from the newValue to Value. 
                Util.FillInValues(newValue, value);
            }
            else
            {
                value = (T)Convert.ChangeType(newValue, typeof(T));
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
            return $"UnityNativeVar({name}, {value})";
        }
    }
}
#endif