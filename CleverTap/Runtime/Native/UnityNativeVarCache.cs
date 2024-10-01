using CleverTapSDK.Native;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Native
{
    internal class UnityNativeVarCache : UnityNativePlatformVariable
    {


    private readonly Dictionary<string, object> valuesFromClient = new Dictionary<string, object>();
    private readonly Dictionary<string, Var> vars = new Dictionary<string, Var>();
    private readonly Dictionary<string, string> defaultKinds = new Dictionary<string, string>();

    private Dictionary<string, object> diffs = new Dictionary<string, object>();
    public object merged = null;

    private Action globalCallbacks = null;

    // Constructor
    public UnityNativeVarCache()
    {
        // Initialization logic, if needed
    }

    /// <summary>
    /// Logs messages for debugging.
    /// </summary>
    private static void Log(string msg)
    {
        Debug.Log("[VarCache]: " + msg);
    }

    /// <summary>
    /// Registers a variable into the cache.
    /// If a variable already exists, it is updated with the new value.
    /// </summary>
    public void RegisterVariable<T>(Var<T> var)
    {
        Log($"Registering variable: {var.Name}");

        vars[var.Name] = var;

        object defaultValue = var.DefaultValue;
        if (defaultValue is Dictionary<string, object> dict)
        {
            defaultValue = DeepCopyMap(dict);
        }

        UpdateValuesAndKinds(var.Name, var.NameComponents, defaultValue, var.Kind, valuesFromClient, defaultKinds);
        MergeVariable(var);
    }

    /// <summary>
    /// Merges a variable with the diffs from the server.
    /// </summary>
    private void MergeVariable<T>(Var<T> var)
    {
        if (merged == null)
        {
            Log("Merged object is null.");
            return;
        }

        string firstComponent = var.NameComponents[0];
        object defaultValue = valuesFromClient.ContainsKey(firstComponent) ? valuesFromClient[firstComponent] : null;
        var mergedMap = merged as Dictionary<string, object>;
        object mergedValue = mergedMap != null && mergedMap.ContainsKey(firstComponent) ? mergedMap[firstComponent] : null;

        bool shouldMerge = (defaultValue == null && mergedValue != null) || (defaultValue != null && !defaultValue.Equals(mergedValue));

        if (shouldMerge)
        {
            object newValue = MergeHelper(defaultValue, mergedValue);
            mergedMap[firstComponent] = newValue;

            // Update variables with new values
            string fullName = firstComponent;
            for (int i = 1; i < var.NameComponents.Length; i++)
            {
                if (vars.ContainsKey(fullName))
                {
                    vars[fullName].Update();
                }
                fullName += "." + var.NameComponents[i];
            }
        }
    }

    /// <summary>
    /// Retrieves a merged variable value.
    /// </summary>
    public object GetMergedValue(string variableName)
    {
        string[] components = variableName.Split('.');
        return GetMergedValueFromComponentArray(components);
    }

    /// <summary>
    /// Retrieves a merged value based on its components.
    /// </summary>
    public object GetMergedValueFromComponentArray(string[] components)
    {
        return GetMergedValueFromComponentArray(components, merged ?? valuesFromClient);
    }

    /// <summary>
    /// Retrieves a merged value based on its components and values.
    /// </summary>
    public object GetMergedValueFromComponentArray(string[] components, object values)
    {
        object mergedPtr = values;
        foreach (var component in components)
        {
            mergedPtr = Traverse(mergedPtr, component);
        }
        return mergedPtr;
    }

    /// <summary>
    /// Loads diffs and applies them.
    /// </summary>
    public void LoadDiffs()
    {
        try
        {
            string cacheData = LoadDataFromCache();
            var variablesAsMap = FromJson<Dictionary<string, object>>(cacheData);
            ApplyVariableDiffs(variablesAsMap);
        }
        catch (Exception e)
        {
            Log("Error loading variable diffs: " + e.Message);
        }
    }

    /// <summary>
    /// Applies variable diffs received from the server.
    /// </summary>
    private void ApplyVariableDiffs(Dictionary<string, object> newDiffs)
    {
        if (newDiffs == null) return;

        diffs = newDiffs;
        merged = MergeHelper(valuesFromClient, diffs);

        // Update registered variables
        foreach (var kvp in vars)
        {
            kvp.Value.Update();
        }

        Log("Variable diffs applied.");
    }

    /// <summary>
    /// Triggers global callbacks after variable changes.
    /// </summary>
    private void TriggerGlobalCallbacks()
    {
        globalCallbacks?.Invoke();
    }

    /// <summary>
    /// Sets a global callback that is triggered when variables change.
    /// </summary>
    public void SetGlobalCallback(Action callback)
    {
        globalCallbacks = callback;
    }

    /// <summary>
    /// Saves diffs to persistent storage.
    /// </summary>
    private void SaveDiffsAsync()
    {
        // Saving diffs asynchronously using Unity's PlayerPrefs as an example
        PlayerPrefs.SetString("VariableDiffs", ToJson(diffs));
        PlayerPrefs.Save();
        Log("Variable diffs saved asynchronously.");
    }

    /// <summary>
    /// Loads data from persistent storage.
    /// </summary>
    private string LoadDataFromCache()
    {
        // Example of using PlayerPrefs to load cache
        return PlayerPrefs.GetString("VariableDiffs", "{}");
    }

    /// <summary>
    /// Clears all user content from the cache.
    /// </summary>
    public void ClearUserContent()
    {
        Log("Clearing user content from cache.");
        ApplyVariableDiffs(new Dictionary<string, object>());
        SaveDiffsAsync();
    }

    /// <summary>
    /// Utility methods for deep copying maps, merging helpers, and traversing values
    /// </summary>
    private Dictionary<string, object> DeepCopyMap(Dictionary<string, object> map)
    {
        return new Dictionary<string, object>(map); // Simplified deep copy for the sake of this example
    }

    private object MergeHelper(object defaultValue, object mergedValue)
    {
        // Simplified merging logic
        return mergedValue ?? defaultValue;
    }

    private object Traverse(object values, string component)
    {
        if (values is Dictionary<string, object> dict && dict.ContainsKey(component))
        {
            return dict[component];
        }
        return null;
    }

    private string ToJson(object obj)
    {
        return JsonUtility.ToJson(obj); // Simplified JSON serialization
    }

    private T FromJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json); // Simplified JSON deserialization
    }

    private void UpdateValuesAndKinds(string name, string[] nameComponents, object defaultValue, string kind, Dictionary<string, object> values, Dictionary<string, string> kinds)
    {
        // This function updates values and kinds for caching.
        // Implement this based on how you want to manage default kinds and values in your system.
    }
}

            
    }
}
