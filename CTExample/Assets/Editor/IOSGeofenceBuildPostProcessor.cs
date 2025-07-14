using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace CTExample
{
    public static class IOSGeofenceBuildPostProcessor
    {
        private static readonly string GCC_PREPROCESSOR_DEFINITIONS = "GCC_PREPROCESSOR_DEFINITIONS";
        private static readonly string InfoPropertyList = "Info.plist";

        private static readonly string CleverTapGeofenceSDKPod = "CleverTap-Geofence-SDK";
        // Specify the CleverTap-Geofence-SDK version, path or git.
        // Leave empty to use the latest version.
        private static readonly string CleverTapGeofenceSDKPodVersion = string.Empty;

        private static readonly string NSLocationAlwaysAndWhenInUseUsageDescription = "NSLocationAlwaysAndWhenInUseUsageDescription";
        private static readonly string NSLocationWhenInUseUsageDescription = "NSLocationWhenInUseUsageDescription";
        private static readonly string NSLocationAlwaysUsageDescription = "NSLocationAlwaysUsageDescription";

        private static readonly string LocationAlwaysAndWhenInUseUsageDescription = "App needs your location to provide enhanced features";
        private static readonly string LocationWhenInUseUsageDescription = "App needs your location to provide enhanced features";
        private static readonly string LocationAlwaysUsageDescription = "App needs your location to provide enhanced features";

        // Must be between 40 and 50 to ensure Pods are installed, see EDM4U
        [PostProcessBuild(46)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS)
            {
                try
                {
                    Debug.Log("[CTExample] IOSGeofenceBuildPostProcessor");
                    ConfigureGeofence(path);
                }
                catch (Exception e)
                {
                    throw new BuildFailedException($"[CTExample] Failed to configure iOS Geofence: {e.Message}");
                }
            }
        }

        private static void ConfigureGeofence(string path)
        {
            // Add the Geofence SDK to Podfile for the UnityFramework target
            AddPodsForGeofence(path);

            // Add the Location Updates to the Background Modes Capability
            AddBackgroundModeLocation(path);

            // Set the Location descriptions in Info.plist
            UpdatePlistWithLocationDescriptions(path);

            // Disable the CleverTapUnityAppController in favor of the CTExampleUnityAppController which inherits from it.
            // The CTExampleUnityAppController starts the CleverTapGeofence monitoring and requests location permission.
            DisableCleverTapAppController(path);
        }

        private static void AddBackgroundModeLocation(string path)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);
            string appTargetGuid = proj.GetUnityMainTargetGuid();
            string entitlements = proj.GetEntitlementFilePathForTarget(appTargetGuid);

            ProjectCapabilityManager projectCapabilityManager = new ProjectCapabilityManager(projPath, entitlements, targetGuid: appTargetGuid);
            projectCapabilityManager.AddBackgroundModes(BackgroundModesOptions.LocationUpdates);
            projectCapabilityManager.WriteToFile();
        }

        private static void DisableCleverTapAppController(string pathToBuildProject)
        {
            string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);
            proj.UpdateBuildProperty(proj.GetUnityFrameworkTargetGuid(), GCC_PREPROCESSOR_DEFINITIONS, new string[] { "$(inherited)", "CT_NO_APP_CONTROLLER_SUBCLASS" }, null);
            proj.WriteToFile(projPath);
        }

        private static void AddPodsForGeofence(string pathToBuildProject)
        {
            string podfilePath = Path.Combine(pathToBuildProject, "Podfile");
            if (!File.Exists(podfilePath))
            {
                throw new BuildFailedException($"[CTExample] Podfile does not exist at path: {podfilePath}");
            }

            var lines = File.ReadAllLines(podfilePath).ToList();
            const string targetString = "target 'UnityFramework' do";
            var targetLineIndex = lines.FindIndex(line => line.Contains(targetString));
            if (targetLineIndex == -1)
            {
                throw new BuildFailedException($"[CTExample] Could not find '{targetString}' in Podfile.");
            }

            var dependency = $"  pod '{CleverTapGeofenceSDKPod}'";
            if (!string.IsNullOrEmpty(CleverTapGeofenceSDKPodVersion))
            {
                dependency = $"{dependency}, {CleverTapGeofenceSDKPodVersion}";
            }

            int insertionIndex = targetLineIndex + 1;
            if (lines[insertionIndex].Trim() == dependency.Trim())
            {
                // Dependency already added
                return;
            }

            if (lines[insertionIndex].TrimStart().StartsWith($"pod '{CleverTapGeofenceSDKPod}'"))
            {
                // Update the dependency
                lines[insertionIndex] = dependency;
            }
            else
            {
                // Add the dependency
                lines.Insert(insertionIndex, dependency);
            }

            File.WriteAllLines(podfilePath, lines);
        }

        private static void UpdatePlistWithLocationDescriptions(string path)
        {
            string plistPath = Path.Combine(path, InfoPropertyList);
            if (!File.Exists(plistPath))
            {
                throw new BuildFailedException($"[CTExample] Info.plist not found at: {plistPath}");
            }

            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            if (!string.IsNullOrWhiteSpace(PlayerSettings.iOS.locationUsageDescription))
            {
                rootDict.SetString(NSLocationAlwaysAndWhenInUseUsageDescription, PlayerSettings.iOS.locationUsageDescription);
            }
            else if (!string.IsNullOrWhiteSpace(LocationAlwaysAndWhenInUseUsageDescription))
            {
                rootDict.SetString(NSLocationAlwaysAndWhenInUseUsageDescription, LocationAlwaysAndWhenInUseUsageDescription);
            }
            else
            {
                throw new BuildFailedException($"{NSLocationAlwaysAndWhenInUseUsageDescription} not provided");
            }

            if (!string.IsNullOrWhiteSpace(LocationWhenInUseUsageDescription))
            {
                rootDict.SetString(NSLocationWhenInUseUsageDescription, LocationWhenInUseUsageDescription);
            }
            else
            {
                throw new BuildFailedException($"{NSLocationWhenInUseUsageDescription} not provided");
            }

            if (!string.IsNullOrWhiteSpace(LocationAlwaysUsageDescription))
            {
                rootDict.SetString(NSLocationAlwaysUsageDescription, LocationAlwaysUsageDescription);
            }
            else
            {
                throw new BuildFailedException($"{NSLocationAlwaysUsageDescription} not provided");
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}