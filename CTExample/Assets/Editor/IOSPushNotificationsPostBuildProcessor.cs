using System.Collections.Generic;
using System.IO;
using System.Linq;
using CleverTapSDK.Private;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using UnityEngine;

public static class IOSPushNotificationsPostBuildProcessor
{
    private static readonly string UserNotificationsFramework = "UserNotifications.framework";
    private static readonly string UserNotificationsUIFramework = "UserNotificationsUI.framework";
    private static readonly string IPHONEOS_DEPLOYMENT_TARGET = "IPHONEOS_DEPLOYMENT_TARGET";
    private static readonly string GCC_PREPROCESSOR_DEFINITIONS = "GCC_PREPROCESSOR_DEFINITIONS";
    private static readonly string InfoPropertyList = "Info.plist";

    private static readonly string CTExampleNotificationService = "CTExampleNotificationService";
    private static readonly string CTExampleNotificationContent = "CTExampleNotificationContent";

    private static string TeamID => PlayerSettings.iOS.appleDeveloperTeamID;
    private static string TargetVersion => PlayerSettings.iOS.targetOSVersionString;
    private static string BundleId => Application.identifier;

    private static readonly bool SetAppGroup = true;
    private static readonly string AppGroupName = $"group.{BundleId}";

    private static readonly bool EnablePushImpressions = true;
    private static readonly string RecordPushImpressionsMacro = "RECORD_PUSH_IMPRESSIONS";

    // Must be between 40 and 50 to ensure Pods are installed, see EDM4U
    [PostProcessBuild(45)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            Debug.Log("[CTExample] IOSPushNotificationsPostBuildProcessor");
            ConfigurePushNotificationExtensions(path);
        }
    }

    private static void ConfigurePushNotificationExtensions(string pathToBuildProject)
    {
        string notificationServiceTargetGuid = AddNotificationService(pathToBuildProject);
        string notificationContentTargetGuid = AddNotificationContent(pathToBuildProject);
        AddPodsForPushNotificationExtensions(pathToBuildProject);
        // Disable the CleverTapUnityAppController in favor of the CTExampleUnityAppController which inherits from it
        DisableCleverTapAppController(pathToBuildProject);

        AddAppGroups(pathToBuildProject, notificationServiceTargetGuid, notificationContentTargetGuid);

        ConfigurePushImpressionsCredentials(pathToBuildProject);
    }

    private static void DisableCleverTapAppController(string pathToBuildProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        proj.UpdateBuildProperty(proj.GetUnityFrameworkTargetGuid(), GCC_PREPROCESSOR_DEFINITIONS, new string[] { "$(inherited)", "CT_NO_APP_CONTROLLER_SUBCLASS" }, null);
        proj.WriteToFile(projPath);
    }

    private static void ConfigurePushImpressionsCredentials(string pathToBuildProject)
    {
        if (EnablePushImpressions)
        {
            CleverTapSettings settings = AssetDatabase.LoadAssetAtPath<CleverTapSettings>(CleverTapSettings.settingsPath);
            UpdatePlistWithSettings(Path.Combine(pathToBuildProject, CTExampleNotificationService), settings);
        }
    }

    private static void AddAppGroups(string pathToBuildProject, string notificationServiceTargetGuid, string notificationContentTargetGuid)
    {
        if (SetAppGroup && !string.IsNullOrEmpty(AppGroupName))
        {
            string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);
            string appTargetGuid = proj.GetUnityMainTargetGuid();

            AddAppGroupToProjectCapabilities(AppGroupName, projPath, proj.GetEntitlementFilePathForTarget(appTargetGuid), appTargetGuid);

            AddAppGroup(AppGroupName, pathToBuildProject, CTExampleNotificationService, notificationServiceTargetGuid);
            AddAppGroup(AppGroupName, pathToBuildProject, CTExampleNotificationContent, notificationContentTargetGuid);
        }
    }

    private static void AddPodsForPushNotificationExtensions(string pathToBuildProject)
    {
        string podfilePath = Path.Combine(pathToBuildProject, "Podfile");
        if (!File.Exists(podfilePath))
        {
            throw new BuildFailedException($"[CTExample] Podfile does not exist at path: {podfilePath}");
        }

        var lines = File.ReadAllLines(podfilePath).ToList();
        const string targetString = "target 'Unity-iPhone' do";
        var targetLineIndex = lines.FindIndex(line => line.Contains(targetString));
        if (targetLineIndex == -1)
        {
            throw new BuildFailedException($"[CTExample] Could not find '{targetString}' in Podfile.");
        }

        var notificationServiceDependencies = new List<string>
        {
            $"  target '{CTExampleNotificationService}' do",
            "    pod 'CTNotificationService', '0.1.7'",
            "  end",
        };

        if (EnablePushImpressions)
        {
            string iOSSDKVersion = GetCleverTapIOSSDKVersion(lines);
            if (string.IsNullOrEmpty(iOSSDKVersion))
            {
                throw new BuildFailedException($"[CTExample] Could not find 'CleverTap-iOS-SDK' version in Podfile.");
            }

            notificationServiceDependencies.Insert(1, $"    pod 'CleverTap-iOS-SDK', {iOSSDKVersion}");
        }

        var dependencies = new List<string>
        {
            $"  target '{CTExampleNotificationContent}' do",
            "    pod 'CTNotificationContent', '0.2.7'",
            "  end"
        };
        dependencies.AddRange(notificationServiceDependencies);

        int insertionIndex = targetLineIndex + 1;
        lines.InsertRange(insertionIndex, dependencies);

        File.WriteAllLines(podfilePath, lines);
    }

    private static string GetCleverTapIOSSDKVersion(List<string> podfileLines)
    {
        int targetLineIndex = podfileLines.FindIndex(line => line.Contains("pod 'CleverTap-iOS-SDK'"));
        if (targetLineIndex == -1)
        {
            return string.Empty;
        }

        string line = podfileLines[targetLineIndex];
        string[] parts = line.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return string.Empty;
        }

        string version = parts[1].Trim();
        return version;
    }

    private static string AddNotificationService(string pathToBuildProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string targetGuid = proj.TargetGuidByName(CTExampleNotificationService);
        if (!string.IsNullOrEmpty(targetGuid))
        {
            return targetGuid;
        }

        string appTargetGuid = proj.GetUnityMainTargetGuid();
        string extensionTargetGuid = proj.AddAppExtension(appTargetGuid, CTExampleNotificationService, $"{BundleId}.{CTExampleNotificationService}", $"{CTExampleNotificationService}/{InfoPropertyList}");

        proj.SetTeamId(extensionTargetGuid, TeamID);

        string path = Path.Combine(pathToBuildProject, CTExampleNotificationService);
        CopyFilesToBuildFolder(path, CTExampleNotificationService);

        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationService}/{InfoPropertyList}", $"{CTExampleNotificationService}/{InfoPropertyList}", false);
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationService}/NotificationService.h", $"{CTExampleNotificationService}/NotificationService.h");
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationService}/NotificationService.m", $"{CTExampleNotificationService}/NotificationService.m");

        proj.AddFrameworkToProject(extensionTargetGuid, UserNotificationsFramework, true);

        proj.AddBuildProperty(extensionTargetGuid, IPHONEOS_DEPLOYMENT_TARGET, TargetVersion);

        if (EnablePushImpressions)
        {
            proj.UpdateBuildProperty(extensionTargetGuid, GCC_PREPROCESSOR_DEFINITIONS, new string[] { "$(inherited)", RecordPushImpressionsMacro }, null);
        }
        else
        {
            proj.UpdateBuildProperty(extensionTargetGuid, GCC_PREPROCESSOR_DEFINITIONS, null, new string[] { RecordPushImpressionsMacro });
        }

        proj.WriteToFile(projPath);

        return extensionTargetGuid;
    }

    private static string AddNotificationContent(string pathToBuildProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string targetGuid = proj.TargetGuidByName(CTExampleNotificationContent);
        if (!string.IsNullOrEmpty(targetGuid))
        {
            return targetGuid;
        }

        string appTargetGuid = proj.GetUnityMainTargetGuid();
        string extensionTargetGuid = proj.AddAppExtension(appTargetGuid, CTExampleNotificationContent, $"{BundleId}.{CTExampleNotificationContent}", $"{CTExampleNotificationContent}/{InfoPropertyList}");

        proj.SetTeamId(extensionTargetGuid, TeamID);

        string path = Path.Combine(pathToBuildProject, CTExampleNotificationContent);
        CopyFilesToBuildFolder(path, CTExampleNotificationContent);

        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/{InfoPropertyList}", $"{CTExampleNotificationContent}/{InfoPropertyList}", false);
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/NotificationViewController.h", $"{CTExampleNotificationContent}/NotificationViewController.h");
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/NotificationViewController.m", $"{CTExampleNotificationContent}/NotificationViewController.m");
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/Base.lproj/MainInterface.storyboard", $"{CTExampleNotificationContent}/Base.lproj/MainInterface.storyboard");

        proj.AddFrameworkToProject(extensionTargetGuid, UserNotificationsFramework, true);
        proj.AddFrameworkToProject(extensionTargetGuid, UserNotificationsUIFramework, true);

        proj.AddBuildProperty(extensionTargetGuid, IPHONEOS_DEPLOYMENT_TARGET, TargetVersion);

        proj.WriteToFile(projPath);

        return extensionTargetGuid;
    }

    private static void AddFile(PBXProject project, string targetGuid, string source, string target, bool addToBuild = true)
    {
        string guid = project.AddFile(source, target);
        if (addToBuild)
        {
            string sectionGuid = project.GetSourcesBuildPhaseByTarget(targetGuid);
            project.AddFileToBuildSection(targetGuid, sectionGuid, guid);
        }
    }

    private static void CopyFilesToBuildFolder(string destinationPath, string sourcePath)
    {
        if (!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);

        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
        }

        foreach (string newPath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }
    }

    private static void AddAppGroup(string appGroup, string pathToBuildProject, string productName, string targetGuid)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        string entitlementFile = string.Format("{0}.entitlements", productName);

        AddAppGroupToProjectCapabilities(appGroup, projPath, entitlementFile, targetGuid);
    }

    private static void AddAppGroupToProjectCapabilities(string appGroup, string projPath, string entitlementFile, string targetGuid)
    {
        ProjectCapabilityManager projectCapabilityManager = new ProjectCapabilityManager(projPath, entitlementFile, targetGuid: targetGuid);
        projectCapabilityManager.AddAppGroups(new string[] { appGroup });
        projectCapabilityManager.WriteToFile();
    }

    private static void UpdatePlistWithSettings(string path, CleverTapSettings settings)
    {
        if (settings == null)
            return;

        string plistPath = Path.Combine(path, InfoPropertyList);
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        PlistElementDict rootDict = plist.root;
        if (!string.IsNullOrWhiteSpace(settings.CleverTapAccountId))
        {
            rootDict.SetString("CleverTapAccountID", settings.CleverTapAccountId);
        }
        else
        {
            Debug.LogError($"CleverTapAccountID has not been set.\n" +
                $"SDK initialization will fail without this. " +
                $"Please set it from {CleverTapSettingsWindow.ITEM_NAME} or " +
                $"manually in the project Info.plist.");
        }

        if (!string.IsNullOrWhiteSpace(settings.CleverTapAccountToken))
        {
            rootDict.SetString("CleverTapToken", settings.CleverTapAccountToken);
        }
        else
        {
            Debug.LogError($"CleverTapToken has not been set.\n" +
                $"SDK initialization will fail without this. " +
                $"Please set it from {CleverTapSettingsWindow.ITEM_NAME} or " +
                $"manually in the project Info.plist.");
        }

        if (!string.IsNullOrWhiteSpace(settings.CleverTapAccountRegion))
        {
            rootDict.SetString("CleverTapRegion", settings.CleverTapAccountRegion);
        }
        if (!string.IsNullOrWhiteSpace(settings.CleverTapProxyDomain))
        {
            rootDict.SetString("CleverTapProxyDomain", settings.CleverTapProxyDomain);
        }
        if (!string.IsNullOrWhiteSpace(settings.CleverTapSpikyProxyDomain))
        {
            rootDict.SetString("CleverTapSpikyProxyDomain", settings.CleverTapSpikyProxyDomain);
        }
        rootDict.SetBoolean("CleverTapDisableIDFV", settings.CleverTapDisableIDFV);

        File.WriteAllText(plistPath, plist.WriteToString());
    }
}