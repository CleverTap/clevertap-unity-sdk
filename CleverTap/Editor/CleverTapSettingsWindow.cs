using CleverTapSDK.Utilities;
using UnityEditor;
using UnityEngine;

namespace CleverTapSDK.Private
{
    public class CleverTapSettingsWindow : EditorWindow
    {
        internal const string ITEM_NAME = "Assets/CleverTap Settings";
        private static readonly string windowName = "CleverTap Settings";

        private CleverTapSettings settings;
        private SerializedObject serializedSettings;
        private Vector2 scrollPosition;

        // Add menu item to open the window
        [MenuItem(ITEM_NAME)]
        public static void ShowWindow()
        {
            GetWindow<CleverTapSettingsWindow>(windowName);
        }

        private void OnEnable()
        {
            // Load or create the ScriptableObject
            settings = LoadCleverTapSettings();

            if (settings != null)
            {
                serializedSettings = new SerializedObject(settings);
            }
        }

        private void OnGUI()
        {
            if (settings == null || serializedSettings == null)
            {
                GUILayout.Label("Error loading settings", EditorStyles.boldLabel);
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            GUILayout.Label(windowName, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Update the SerializedObject to get latest values
            serializedSettings.Update();

            // Display all properties of the ScriptableObject
            SerializedProperty property = serializedSettings.GetIterator();
            property.NextVisible(true); // Skip the script property

            while (property.NextVisible(false))
            {
                // Special handling for boolean properties to prevent overlapping
                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    EditorGUILayout.BeginHorizontal();

                    // Label with fixed width to create space
                    EditorGUILayout.LabelField(property.displayName, GUILayout.Width(300));

                    // Checkbox with proper spacing
                    property.boolValue = EditorGUILayout.Toggle(property.boolValue, GUILayout.Width(50));

                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(property, new GUIContent(property.displayName), true);
                }
            }


            // Apply any changes
            if (serializedSettings.ApplyModifiedProperties())
            {
                EditorUtility.SetDirty(settings);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            // Save button
            if (GUILayout.Button("Save Settings"))
            {
                SaveCleverTapSettings();
            }
        }

        private CleverTapSettings LoadCleverTapSettings()
        {
            try
            {
                // Load settings from .asset file
                CleverTapSettings settings = AssetDatabase.LoadAssetAtPath<CleverTapSettings>(CleverTapSettings.settingsPath);

                if (settings == null)
                {
                    Debug.Log("CleverTapSettings asset not found. Creating new asset.");
                    // Create a new instance if it doesn't exist
                    settings = CreateInstance<CleverTapSettings>();

                    // Initialize with default environment values
                    var defaultDict = new System.Collections.Generic.Dictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential>();
                    foreach (CleverTapEnvironmentKey env in System.Enum.GetValues(typeof(CleverTapEnvironmentKey)))
                    {
                        defaultDict[env] = new CleverTapEnvironmentCredential();
                    }

                    settings.Environments = SerializableDictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential>.FromDictionary(defaultDict);

                    AssetDatabase.CreateAsset(settings, CleverTapSettings.settingsPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return settings;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }

        private void SaveCleverTapSettings()
        {
            // Save the ScriptableObject asset
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            Debug.Log($"CleverTapSettings saved to {CleverTapSettings.settingsPath}");

            // Save to JSON if enabled
            if (settings.CleverTapSettingsSaveToJSON)
            {
                SaveSettingsToJson();
            }
            else
            {
                DeleteSettingsJson();
            }
        }

        private void SaveSettingsToJson()
        {
            try
            {
                string json = JsonUtility.ToJson(settings, true);
                if (!System.IO.Directory.Exists(Application.streamingAssetsPath))
                {
                    System.IO.Directory.CreateDirectory(Application.streamingAssetsPath);
                }
                System.IO.File.WriteAllText(CleverTapSettings.jsonPath, json);
                Debug.Log($"CleverTap settings saved to {CleverTapSettings.jsonPath}");
                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save settings to JSON: {ex.Message}");
                EditorUtility.DisplayDialog("Error",
                    "Failed to save settings to JSON. Check the console for details.", "OK");
            }
        }

        private void DeleteSettingsJson()
        {
            if (System.IO.File.Exists(CleverTapSettings.jsonPath))
            {
                try
                {
                    System.IO.File.Delete(CleverTapSettings.jsonPath);
                    Debug.Log($"CleverTap settings deleted from: {CleverTapSettings.jsonPath}");
                    AssetDatabase.Refresh();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to delete settings JSON: {ex.Message}");
                    EditorUtility.DisplayDialog("Error",
                        "Failed to delete settings JSON. Check the console for details.", "OK");
                }
            }
        }
    }
}