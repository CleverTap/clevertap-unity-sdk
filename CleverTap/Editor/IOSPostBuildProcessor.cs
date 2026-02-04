#if UNITY_IOS && UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using CleverTapSDK.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace CleverTapSDK.Private
{
    public static class IOSPostBuildProcessor
    {
        [PostProcessBuild(99)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS)
            {
                IOSPostProcess(path);
            }
        }

        private static void IOSPostProcess(string path)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

#if UNITY_2019_3_OR_NEWER
            var projectTarget = proj.GetUnityFrameworkTargetGuid();
#else
			var projectTarget = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif

            // Add linker flags
            proj.AddBuildProperty(projectTarget, "OTHER_LDFLAGS", "-ObjC");

            // Update project based on CleverTap Settings
            CleverTapSettings settings = AssetDatabase.LoadAssetAtPath<CleverTapSettings>(CleverTapSettings.settingsPath);
            UpdateProjectWithSettings(path, proj, projectTarget, settings);

            // Copy Assets to iOS project
            CopyAssetsToIOSResources(path, proj);

            File.WriteAllText(projPath, proj.WriteToString());
        }

        private static void UpdateProjectWithSettings(string path, PBXProject proj, string projectTarget, CleverTapSettings settings)
        {
            if (settings != null)
            {
                bool isDevelopmentBuild = EditorUserBuildSettings.development;
                UpdatePlistWithSettings(path, settings, isDevelopmentBuild);
                UpdatePreprocessorMacros(proj, projectTarget, settings, isDevelopmentBuild);
            }
            else
            {
                Debug.Log($"CleverTapSettings have not been set.\n" +
                    $"Please update them from {CleverTapSettingsWindow.ITEM_NAME} or " +
                    $"set them manually in the project Info.plist.");
            }
        }

        /// <summary>
        /// Updates the "GCC_PREPROCESSOR_DEFINITIONS" build property.
        /// Adds or removes preprocessor macros of the project Build Settings.
        /// </summary>
        /// <param name="proj">The project to update.</param>
        /// <param name="projectTarget">The project target guid.</param>
        /// <param name="settings">The CleverTapSettings to use.</param>
        private static void UpdatePreprocessorMacros(PBXProject proj, string projectTarget, CleverTapSettings settings, bool isDevelopmentBuild)
        {
            // The UpdateBuildProperty set the property value if no values are present. This overrides the $(inherited) value.
            // Add the $(inherited) value as a workaround.
            string preprocessorMacros = "GCC_PREPROCESSOR_DEFINITIONS";

            // Add DEBUG macro for development builds to enable environment switching
            if (isDevelopmentBuild)
            {
                proj.UpdateBuildProperty(projectTarget, preprocessorMacros, new string[] { "$(inherited)", "DEBUG=1" }, null);
                Debug.Log("[CleverTap] Development Build - Added DEBUG preprocessor macro");
            }
            else
            {
                proj.UpdateBuildProperty(projectTarget, preprocessorMacros, null, new string[] { "DEBUG", "DEBUG=1" });
            }

            if (!settings.CleverTapIOSUseAutoIntegrate)
            {
                proj.UpdateBuildProperty(projectTarget, preprocessorMacros, new string[] { "$(inherited)", "NO_AUTOINTEGRATE" }, null);
            }
            else
            {
                proj.UpdateBuildProperty(projectTarget, preprocessorMacros, null, new string[] { "NO_AUTOINTEGRATE" });
            }

            if (!settings.CleverTapIOSUseUNUserNotificationCenter)
            {
                proj.UpdateBuildProperty(projectTarget, preprocessorMacros, new string[] { "$(inherited)", "NO_UNUSERNOTIFICATIONCENTER" }, null);
            }
            else
            {
                proj.UpdateBuildProperty(projectTarget, preprocessorMacros, null, new string[] { "NO_UNUSERNOTIFICATIONCENTER" });
            }
        }

        /// <summary>
        /// Writes the CleverTapSettings (account id, token etc.) to the Info.plist file.
        /// </summary>
        /// <param name="path">The project path.</param>
        /// <param name="settings">The settings to use.</param>
        private static void UpdatePlistWithSettings(string path, CleverTapSettings settings, bool isDevelopmentBuild)
        {
            if (settings == null)
                return;

            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            if (settings.Environments == null || settings.Environments.Items == null)
            {
                Debug.LogError("[CTExample] CleverTapSettings - Environments are not configured.");
                return;
            }

            Dictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential> environments = settings.Environments.ToDictionary();

            if (environments == null || environments.Count == 0)
            {
                Debug.LogError("[CTExample] CleverTapSettings - Environments are not configured.");
                return;
            }

            if (!environments.TryGetValue(settings.DefaultEnvironment, out CleverTapEnvironmentCredential environmentValue))
            {
                Debug.LogError($"[CTExample] CleverTapSettings - Environment is null or not configured for {settings.DefaultEnvironment}");
                return;
            }

            if (!string.IsNullOrWhiteSpace(environmentValue.CleverTapAccountId))
            {
                rootDict.SetString("CleverTapAccountID", environmentValue.CleverTapAccountId);
            }
            else
            {
                Debug.Log($"CleverTapAccountID has not been set.\n" +
                    $"SDK initialization will fail without this. " +
                    $"Please set it from {CleverTapSettingsWindow.ITEM_NAME} or " +
                    $"manually in the project Info.plist.");
            }

            if (!string.IsNullOrWhiteSpace(environmentValue.CleverTapAccountToken))
            {
                rootDict.SetString("CleverTapToken", environmentValue.CleverTapAccountToken);
            }
            else
            {
                Debug.Log($"CleverTapToken has not been set.\n" +
                    $"SDK initialization will fail without this. " +
                    $"Please set it from {CleverTapSettingsWindow.ITEM_NAME} or " +
                    $"manually in the project Info.plist.");
            }

            if (!string.IsNullOrWhiteSpace(environmentValue.CleverTapAccountRegion))
            {
                rootDict.SetString("CleverTapRegion", environmentValue.CleverTapAccountRegion);
            }
            if (!string.IsNullOrWhiteSpace(environmentValue.CleverTapProxyDomain))
            {
                rootDict.SetString("CleverTapProxyDomain", environmentValue.CleverTapProxyDomain);
            }
            if (!string.IsNullOrWhiteSpace(environmentValue.CleverTapSpikyProxyDomain))
            {
                rootDict.SetString("CleverTapSpikyProxyDomain", environmentValue.CleverTapSpikyProxyDomain);
            }

            if (isDevelopmentBuild)
            {
                Debug.Log($"[CleverTap] Development Build - Writing {environments.Count} additional environment(s) to Info.plist");
                rootDict.SetString("CleverTapDefaultEnv", settings.DefaultEnvironment.ToString());

                foreach (KeyValuePair<CleverTapEnvironmentKey, CleverTapEnvironmentCredential> env in environments)
                {
                    string suffix = "_" + env.Key.ToString();

                    if (!string.IsNullOrWhiteSpace(env.Value.CleverTapAccountId))
                        rootDict.SetString("CleverTapAccountID" + suffix, env.Value.CleverTapAccountId);

                    if (!string.IsNullOrWhiteSpace(env.Value.CleverTapAccountToken))
                        rootDict.SetString("CleverTapToken" + suffix, env.Value.CleverTapAccountToken);

                    if (!string.IsNullOrWhiteSpace(env.Value.CleverTapAccountRegion))
                        rootDict.SetString("CleverTapRegion" + suffix, env.Value.CleverTapAccountRegion);

                    if (!string.IsNullOrWhiteSpace(env.Value.CleverTapProxyDomain))
                        rootDict.SetString("CleverTapProxyDomain" + suffix, env.Value.CleverTapProxyDomain);

                    if (!string.IsNullOrWhiteSpace(env.Value.CleverTapSpikyProxyDomain))
                        rootDict.SetString("CleverTapSpikyProxyDomain" + suffix, env.Value.CleverTapSpikyProxyDomain);

                    Debug.Log($"[CleverTap] Added environment: {env.Key} with AccountID: {env.Value.CleverTapAccountId}");
                }
            }

            rootDict.SetBoolean("CleverTapDisableIDFV", settings.CleverTapDisableIDFV);
            rootDict.SetBoolean("CleverTapPresentNotificationForeground", settings.CleverTapIOSPresentNotificationOnForeground);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }

        /// <summary>
        /// Copies the CleverTap folder (Assets/CleverTap) to the iOS exported project.
        /// The copied folder has "Build Rules" - "Apply Once to Folder".
        /// The folder is added to the "Copy Bundle Resources" Build Phase.
        /// The folder is with "Location" - "Relative to Group" and appears blue in Xcode.
        /// The folder has "Target Membership" the main target - "Unity-iPhone".
        /// </summary>
        /// <remarks>
        /// The folder must be copied with a different name than the original one in the Assets (CleverTap -> CleverTapSDK),
        /// otherwise its contents do not appear correctly in Xcode but will still be copied into the bundle.
        /// </remarks>
        /// <param name="path">The project path.</param>
        /// <param name="proj">The Xcode project.</param>
        private static void CopyAssetsToIOSResources(string path, PBXProject proj)
        {
            string sourceFolderPath = Path.Combine(Application.dataPath, EditorUtils.CLEVERTAP_ASSETS_FOLDER);
            if (!Directory.Exists(sourceFolderPath))
            {
                return;
            }

            // Copy CleverTap folder
            string destinationFolderPath = Path.Combine(path, EditorUtils.CLEVERTAP_APP_ASSETS_FOLDER);
            HashSet<string> folderNamesToCopy = new HashSet<string>() { EditorUtils.CLEVERTAP_CUSTOM_TEMPLATES_FOLDER };
            try
            {
                EditorUtils.DirectoryCopy(sourceFolderPath, destinationFolderPath, true, true, folderNamesToCopy);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to Copy assets to iOS project. " +
                    $"This affects Custom Templates and App Functions. Exception: {ex}");
                return;
            }

            string mainTargetGuid = proj.GetUnityMainTargetGuid();
            // Add CleverTap folder reference and target membership
            string folderGuid = proj.AddFolderReference(EditorUtils.CLEVERTAP_APP_ASSETS_FOLDER, EditorUtils.CLEVERTAP_APP_ASSETS_FOLDER);
            proj.AddFileToBuild(mainTargetGuid, folderGuid);
        }
    }
}
#endif