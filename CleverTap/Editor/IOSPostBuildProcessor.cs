#if (UNITY_IOS || UNITY_ANDROID) && UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace CleverTapSDK.Private
{
    public static class IOSPostBuildProcessor
	{
		private static readonly string rn = "\n";

		private static readonly string CODE_LIB_IMPORT =
		"#import \"CleverTapUnityManager.h\"" + rn;

		private static readonly string CODE_USER_NOTIFICATIONS_IMPORT =
		"#import <UserNotifications/UserNotifications.h>" + rn;

		private static readonly string PATH_CONTROLLER = "/Classes/UnityAppController.mm";

		private static readonly string SIGNATURE_URL =
			"- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation";
		private static readonly string CODE_URL = rn +
			"    [[CleverTapUnityManager sharedInstance] handleOpenURL:url sourceApplication:sourceApplication];" + rn;

		private static readonly string SIGNATURE_PUSH_TOKEN =
			"- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken";
		private static readonly string CODE_PUSH_TOKEN = rn +
			"    [[CleverTapUnityManager sharedInstance] setPushToken:deviceToken];" + rn;

		private static readonly string SIGNATURE_DID_FINISH_LAUNCH =
			"- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions";

		private static readonly string SIGNATURE_NOTIF_LOCAL =
			"- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification";
		private static readonly string SIGNATURE_NOTIF_REMOTE =
			"- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo";
		private static readonly string SIGNATURE_NOTIF_REMOTE_BG =
			"- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler";
		private static readonly string CODE_NOTIF_LOCAL = rn +
			"    [[CleverTapUnityManager sharedInstance] registerApplication:application didReceiveRemoteNotification:notification.userInfo];" + rn;
		private static readonly string CODE_NOTIF = rn +
			"    [[CleverTapUnityManager sharedInstance] registerApplication:application didReceiveRemoteNotification:userInfo];" + rn;
		private static readonly string CODE_ENABLE_PERSONALIZATION = rn +
			"    [CleverTapUnityManager enablePersonalization];" + rn;
		private static readonly string CODE_ADD_USER_NOTIFICATION_FRAMEWORK = rn +
			"    [UNUserNotificationCenter currentNotificationCenter].delegate = (id <UNUserNotificationCenterDelegate>)self;" + rn;

        private enum Position { Begin, End };

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

            CleverTapSettings settings = AssetDatabase.LoadAssetAtPath<CleverTapSettings>(CleverTapSettings.settingsPath);
            if (settings != null)
            {
                UpdatePlistWithSettings(path, settings);
            }
            else
            {
                Debug.Log($"CleverTapSettings have not been set.\n" +
                    $"Please update them from {CleverTapSettingsWindow.ITEM_NAME} or " +
                    $"set them manually in the project Info.plist.");
            }

            // Update AppController
            bool personalizationEnabled = settings == null || settings.CleverTapEnablePersonalization;
            InsertCodeIntoControllerClass(path, personalizationEnabled);

            AddCleverTapFolder(path, proj);

            File.WriteAllText(projPath, proj.WriteToString());
        }

		/// <summary>
		/// Writes the CleverTapSettings (account id, token etc.) to the Info.plist file.
		/// </summary>
		/// <param name="path">The project path.</param>
		/// <param name="settings">The settings to use.</param>
        private static void UpdatePlistWithSettings(string path, CleverTapSettings settings)
        {
            if (settings == null)
                return;

            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            rootDict.SetString("CleverTapAccountID", settings.CleverTapAccountId);
            rootDict.SetString("CleverTapToken", settings.CleverTapAccountToken);
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
        private static void AddCleverTapFolder(string path, PBXProject proj)
        {
			// Copy CleverTap folder
			string cleverTapAssetsFolder = "CleverTap";
			string cleverTapIOSBuildFolder = "CleverTapSDK";
            string sourceFolderPath = Path.Combine(Application.dataPath, cleverTapAssetsFolder);
            string destinationFolderPath = Path.Combine(path, cleverTapIOSBuildFolder);
            EditorUtils.DirectoryCopy(sourceFolderPath, destinationFolderPath);

            string mainTargetGuid = proj.GetUnityMainTargetGuid();
            // Add CleverTap folder reference and target membership
            string folderGuid = proj.AddFolderReference(destinationFolderPath, cleverTapIOSBuildFolder);
            proj.AddFileToBuild(mainTargetGuid, folderGuid);
        }

        private static void InsertCodeIntoControllerClass(string projectPath, bool personalizationEnabled)
		{
			string filepath = projectPath + PATH_CONTROLLER;
			string[] methodSignatures = { SIGNATURE_PUSH_TOKEN, SIGNATURE_URL, SIGNATURE_NOTIF_LOCAL, SIGNATURE_NOTIF_REMOTE, SIGNATURE_NOTIF_REMOTE_BG };
			string[] valuesToAppend = { CODE_PUSH_TOKEN, CODE_URL, CODE_NOTIF_LOCAL, CODE_NOTIF, CODE_NOTIF };
			Position[] positionsInMethod = new Position[] { Position.End, Position.Begin, Position.End, Position.End, Position.Begin };
			InsertCodeIntoClass(filepath, methodSignatures, valuesToAppend, positionsInMethod);

			string[] methodSignaturesRegPush = { SIGNATURE_DID_FINISH_LAUNCH };
			if (personalizationEnabled)
			{
				string[] valuesToAppendRegPush = { CODE_ENABLE_PERSONALIZATION };
				Position[] positionsInMethodRegPush = new Position[] { Position.Begin };
				InsertCodeIntoClass(filepath, methodSignaturesRegPush, valuesToAppendRegPush, positionsInMethodRegPush);
			}

			string[] valuesToAppendUserNotifications = { CODE_ADD_USER_NOTIFICATION_FRAMEWORK };
			Position[] positionsInMethodRegUserNotifications = new Position[] { Position.Begin };
			InsertCodeIntoClass(filepath, methodSignaturesRegPush, valuesToAppendUserNotifications, positionsInMethodRegUserNotifications);

		}

		private static void InsertCodeIntoClass(string filepath, string[] methodSignatures, string[] valuesToAppend, Position[] positionsInMethod)
		{
			if (!File.Exists(filepath))
			{
				return;
			}

			string fileContent = File.ReadAllText(filepath);
			List<int> ignoredIndices = new List<int>();

			for (int i = 0; i < valuesToAppend.Length; i++)
			{
				string val = valuesToAppend[i];

				if (fileContent.Contains(val))
				{
					ignoredIndices.Add(i);
				}
			}

			string[] fileLines = File.ReadAllLines(filepath);
			List<string> newContents = new List<string>();
			bool found = false;
			int foundIndex = -1;

			newContents.Add(CODE_LIB_IMPORT);
			newContents.Add(CODE_USER_NOTIFICATIONS_IMPORT);

			foreach (string line in fileLines)
			{
				if (line.Trim().Contains(CODE_USER_NOTIFICATIONS_IMPORT.Trim()))
				{
					continue;
				}

				if (line.Trim().Contains(CODE_LIB_IMPORT.Trim()))
				{
					continue;
				}

				newContents.Add(line + rn);
				for (int j = 0; j < methodSignatures.Length; j++)
				{
					if ((line.Trim().Equals(methodSignatures[j])) && !ignoredIndices.Contains(j))
					{
						foundIndex = j;
						found = true;
					}
				}

				if (found)
				{
					if ((positionsInMethod[foundIndex] == Position.Begin) && line.Trim().Equals("{"))
					{
						newContents.Add(valuesToAppend[foundIndex] + rn);
						found = false;
					}
					else if ((positionsInMethod[foundIndex] == Position.End) && line.Trim().Equals("}"))
					{
						newContents = newContents.GetRange(0, newContents.Count - 1);
						newContents.Add(valuesToAppend[foundIndex] + rn + "}" + rn);
						found = false;
					}
				}
			}
			string output = string.Join("", newContents.ToArray());
			File.WriteAllText(filepath, output);
		}
	}
}
#endif