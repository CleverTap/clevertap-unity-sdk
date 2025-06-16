using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using UnityEngine;

public static class IOSPushNotificationsPostBuildProcessor
{
    private static string TeamID => PlayerSettings.iOS.appleDeveloperTeamID;
    private static string BundleId => Application.identifier;

    private static readonly string CTExampleNotificationService = "CTExampleNotificationService";
    private static readonly string CTExampleNotificationContent = "CTExampleNotificationContent";

    // Must be between 40 and 50 to ensure Pods are installed, see EDM4U
    [PostProcessBuild(45)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            PostProcess(path);
        }
    }

    private static void PostProcess(string path)
    {
        Debug.Log("[CTExample] IOSPushNotificationsPostBuildProcessor");
        AddPushNotificationExtensions(path);
    }

    private static void AddPushNotificationExtensions(string pathToBuildProject)
    {
        AddNotificationService(pathToBuildProject);
        AddNotificationContent(pathToBuildProject);

        AddPodsForPushNotificationExtensions(pathToBuildProject);
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

        var dependencies = new[]
        {
            $"  target '{CTExampleNotificationContent}' do",
            "    pod 'CTNotificationContent', '0.2.7'",
            "  end",
            $"  target '{CTExampleNotificationService}' do",
            "    pod 'CTNotificationService', '0.1.7'",
            "  end",
        };

        int insertionIndex = targetLineIndex + 1;
        lines.InsertRange(insertionIndex, dependencies);

        File.WriteAllLines(podfilePath, lines);
    }

    private static string AddNotificationService(string pathToBuildProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string appTargetGuid = proj.GetUnityMainTargetGuid();
        string extensionTargetGuid = proj.AddAppExtension(appTargetGuid, CTExampleNotificationService, $"{BundleId}.{CTExampleNotificationService}", $"{CTExampleNotificationService}/Info.plist");

        proj.SetTeamId(extensionTargetGuid, TeamID);

        string path = Path.Combine(pathToBuildProject, CTExampleNotificationService);
        CopyFilesToBuildFolder(path, CTExampleNotificationService);

        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationService}/Info.plist", $"{CTExampleNotificationService}/Info.plist", false);
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationService}/NotificationService.h", $"{CTExampleNotificationService}/NotificationService.h");
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationService}/NotificationService.m", $"{CTExampleNotificationService}/NotificationService.m");

        proj.AddFrameworkToProject(extensionTargetGuid, "UserNotifications.framework", true);

        proj.AddBuildProperty(extensionTargetGuid, "IPHONEOS_DEPLOYMENT_TARGET", "13.0");

        proj.WriteToFile(projPath);

        return extensionTargetGuid;
    }

    private static string AddNotificationContent(string pathToBuildProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string appTargetGuid = proj.GetUnityMainTargetGuid();
        string extensionTargetGuid = proj.AddAppExtension(appTargetGuid, CTExampleNotificationContent, $"{BundleId}.{CTExampleNotificationContent}", $"{CTExampleNotificationContent}/Info.plist");

        proj.SetTeamId(extensionTargetGuid, TeamID);

        string path = Path.Combine(pathToBuildProject, CTExampleNotificationContent);
        CopyFilesToBuildFolder(path, CTExampleNotificationContent);

        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/Info.plist", $"{CTExampleNotificationContent}/Info.plist", false);
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/NotificationViewController.h", $"{CTExampleNotificationContent}/NotificationViewController.h");
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/NotificationViewController.m", $"{CTExampleNotificationContent}/NotificationViewController.m");
        AddFile(proj, extensionTargetGuid, $"{CTExampleNotificationContent}/Base.lproj/MainInterface.storyboard", $"{CTExampleNotificationContent}/Base.lproj/MainInterface.storyboard");

        proj.AddFrameworkToProject(extensionTargetGuid, "UserNotifications.framework", true);
        proj.AddFrameworkToProject(extensionTargetGuid, "UserNotificationsUI.framework", true);

        proj.AddBuildProperty(extensionTargetGuid, "IPHONEOS_DEPLOYMENT_TARGET", "13.0");

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
}