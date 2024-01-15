using UnityEngine;
using UnityEditor;

namespace CleverTapSDK.Private
{
    // Custom editor window class
    public class CleverTapSettingsWindow : EditorWindow
    {
        private const string NAME = "CleverTap Settings";

        private IOSConfigSettings config;

        // Add menu item to open the window
        [MenuItem("Assets/" + NAME)]
        public static void ShowWindow()
        {
            GetWindow<CleverTapSettingsWindow>(NAME);
        }

        private void OnEnable()
        {
            // Load settings from EditorPrefs
            config = IOSConfigSettings.Load();
        }

        private void OnGUI()
        {
            // iOS Settings
            GUILayout.Label("iOS Settings", EditorStyles.boldLabel);

            // Input text fields
            config.CleverTapAccountId = EditorGUILayout.TextField("CleverTapAccountId", config.CleverTapAccountId);
            config.CleverTapAccountToken = EditorGUILayout.TextField("CleverTapAccountToken", config.CleverTapAccountToken);
            config.CleverTapAccountRegion = EditorGUILayout.TextField("CleverTapAccountRegion", config.CleverTapAccountRegion);

            // Checkboxes
            config.CleverTapEnablePersonalization = EditorGUILayout.Toggle("CleverTapEnablePersonalization", config.CleverTapEnablePersonalization);
            config.CleverTapDisableIDFV = EditorGUILayout.Toggle("CleverTapDisableIDFV", config.CleverTapDisableIDFV);

            // Save button
            if (GUILayout.Button("Save Settings"))
            {
                // Save settings to EditorPrefs
                config.Save();

                Debug.Log($"{NAME} saved!");
            }
        }
    }
}