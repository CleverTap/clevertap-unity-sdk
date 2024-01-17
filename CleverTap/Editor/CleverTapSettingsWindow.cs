﻿using UnityEngine;
using UnityEditor;

namespace CleverTapSDK.Private
{
    public class CleverTapSettingsWindow : EditorWindow
    {
        internal const string ITEM_NAME = "Assets/CleverTap Settings";
        private static readonly string windowName = "CleverTap Settings";
        private static readonly string settingsPath = "Assets/CleverTapSettings.asset";

        private CleverTapSettings settings;

        // Add menu item to open the window
        [MenuItem(ITEM_NAME)]
        public static void ShowWindow()
        {
            GetWindow<CleverTapSettingsWindow>(windowName);
        }

        private void OnEnable()
        {
            settings = LoadCleverTapSettings();
        }

        private void OnGUI()
        {
            GUILayout.Label(windowName, EditorStyles.boldLabel);

            // Input text fields
            settings.CleverTapAccountId = EditorGUILayout.TextField("CleverTapAccountId", settings.CleverTapAccountId);
            settings.CleverTapAccountToken = EditorGUILayout.TextField("CleverTapAccountToken", settings.CleverTapAccountToken);
            settings.CleverTapAccountRegion = EditorGUILayout.TextField("CleverTapAccountRegion", settings.CleverTapAccountRegion);

            // Checkboxes
            settings.CleverTapEnablePersonalization = EditorGUILayout.Toggle("CleverTapEnablePersonalization", settings.CleverTapEnablePersonalization);
            settings.CleverTapDisableIDFV = EditorGUILayout.Toggle("CleverTapDisableIDFV", settings.CleverTapDisableIDFV);

            // Save button
            if (GUILayout.Button("Save Settings"))
            {
                // Save settings to EditorPrefs
                SaveCleverTapSettings();

                Debug.Log($"{windowName} saved!");
            }
        }

        private static CleverTapSettings LoadCleverTapSettings()
        {
            // Load settings from .asset file
            CleverTapSettings settings = AssetDatabase.LoadAssetAtPath<CleverTapSettings>(CleverTapSettings.settingsPath);

            if (settings == null)
            {
                Debug.Log("Asset not found. Creating asset.");
                // Create a new instance if it doesn't exist
                settings = CreateInstance<CleverTapSettings>();
                AssetDatabase.CreateAsset(settings, settingsPath);
                AssetDatabase.SaveAssets();
                // Refresh the database to make sure the new asset is recognized
                AssetDatabase.Refresh();
            }
            return settings;
        }

        private void SaveCleverTapSettings()
        {
            // Save settings to .asset file
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssetIfDirty(settings);
        }
    }
}