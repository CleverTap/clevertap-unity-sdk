using System.Collections;
using System.Collections.Generic;
using System.Text;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeVarCache
	{
        private readonly IDictionary<string, IVar> vars = new Dictionary<string, IVar>();
        private IDictionary<string, object> diffs = new Dictionary<string, object>();
        private object merged = null;
        private IDictionary<string, object> valuesFromClient = new Dictionary<string, object>();

        internal int VariablesCount { get => vars.Count; }

        internal void RegisterVariable(IVar variable)
		{
            if (variable == null)
            {
                CleverTapLogger.LogError("RegisterVariable called with null variable.");
                return;
            }

            vars[variable.Name] = variable;

            object defaultValue = variable.DefaultObjectValue;
            if (defaultValue is IDictionary valueDictionary)
            {
                defaultValue = UnityNativeVariableUtils.CopyDictionary(valueDictionary);
            }
            UnityNativeVariableUtils.UpdateValues(variable.Name, variable.NameComponents, defaultValue, (IDictionary)valuesFromClient);

            MergeVariable(variable);
		}

        internal Var<T> GetVariable<T>(string name)
        {
            if (vars.ContainsKey(name))
            {
                if (vars[name] is Var<T> typedVar)
                {
                    return typedVar;
                }
                CleverTapLogger.Log($"GetVariable: Variable '{name}' exists but is not of type {typeof(T).Name}");
            }
            return null;
        }

        internal object GetMergedValue(string name)
        {
            string[] components = UnityNativeVariableUtils.GetNameComponents(name);
            object mergedValue = GetMergedValueFromComponentArray(components);
            if (mergedValue is IDictionary valueDictionary)
            {
                return UnityNativeVariableUtils.CopyDictionary(valueDictionary);
            }
            return mergedValue;
        }

        internal object GetMergedValueFromComponentArray(object[] components)
        {
            return GetMergedValueFromComponentArray(components, merged ?? valuesFromClient);
        }

        internal object GetMergedValueFromComponentArray(object[] components, object values)
        {
            object mergedPtr = values;
            foreach (var component in components)
            {
                mergedPtr = UnityNativeVariableUtils.Traverse(mergedPtr, component, false);
            }
            return mergedPtr;
        }

        /// <summary>
        /// Merge default variable value with VarCache.merged value.
        /// If invoked with a.b.c.d, updates a, a.b, a.b.c, but a.b.c.d is left for the Var.Define.
        /// This is neccessary if variable was registered after VarCache.applyVariableDiffs.
        /// </summary>
        /// <param name="variable"></param>
        private void MergeVariable(IVar variable)
        {
            if (merged == null)
            {
                CleverTapLogger.Log("MergeVariable called, but `merged` member is null.");
                return;
            }
            if (merged is not IDictionary mergedDictionary)
            {
                CleverTapLogger.Log("MergeVariable called, but `merged` member is not of Dictionary type.");
                return;
            }

            string firstComponent = variable.NameComponents[0];
            valuesFromClient.TryGetValue(firstComponent, out object defaultValue);
            mergedDictionary.TryGetValue(firstComponent, out object mergedValue);

            bool shouldMerge = defaultValue != null && !defaultValue.Equals(mergedValue);
            if (shouldMerge)
            {
                object newValue = UnityNativeVariableUtils.MergeHelper(defaultValue, mergedValue);

                mergedDictionary[firstComponent] = newValue;

                // Build the name progressively to find and update all affected parent variables
                StringBuilder name = new StringBuilder(firstComponent);
                for (int i = 1; i < variable.NameComponents.Length; i++)
                {
                    // Try to update any existing variable with the current name path
                    vars.TryGetValue(name.ToString(), out IVar existing);
                    existing?.Update();
                    // Add the next component to the path
                    name.Append(UnityNativeVariableUtils.DOT)
                        .Append(variable.NameComponents[i]);
                }
            }
        }

        internal void ApplyVariableDiffs(IDictionary<string, object> diffs)
        {
            if (diffs == null)
                return;

            this.diffs = diffs;
            merged = UnityNativeVariableUtils.MergeHelper(valuesFromClient, this.diffs);

            foreach (var kv in vars)
            {
                var variable = kv.Value;
                variable?.Update();
            }
        }
    }
}