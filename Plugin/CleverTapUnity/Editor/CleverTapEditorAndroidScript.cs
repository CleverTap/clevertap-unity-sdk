using System;
using System.IO;
using UnityEditor;


#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;

[InitializeOnLoad]
public class CleverTapEditorAndroidScript : AssetPostprocessor {

	public static object svcSupport;

	private static readonly string PluginName = "CleverTapUnity";
	private static readonly string PLAY_SERVICES_VERSION = "16.0.1";
	private static readonly string ANDROID_SUPPORT_VERSION = "28.0.0";
	
	static CleverTapEditorAndroidScript()
	{
		SetupDeps();
	}

	static void SetupDeps()
	{
		// Setup the resolver using reflection as the module may not be
		// available at compile time.
		Type playServicesSupport = Google.VersionHandler.FindClass(
			"Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
		if (playServicesSupport == null) {
			return;
		}

		svcSupport = svcSupport ?? Google.VersionHandler.InvokeStaticMethod(
			playServicesSupport, "CreateInstance",
			new object[] {
				PluginName,
				EditorPrefs.GetString("AndroidSdkRoot"),
				"ProjectSettings"
			});

		Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "DependOn",
			new object[] {
				"com.google.android.gms",
				"play-services-base",
				PLAY_SERVICES_VERSION
			},
			namedArgs: new Dictionary<string, object>() {
				{"packageIds", new string[] { "extra-google-m2repository" } }
			});
				

		Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "DependOn",
			new object[] {
				"com.google.android.gms",
				"play-services-basement",
				PLAY_SERVICES_VERSION
			},
			namedArgs: new Dictionary<string, object>() {
				{"packageIds", new string[] { "extra-google-m2repository" } }
			});
		
		Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "DependOn",
			new object[] {
				"com.google.android.gms",
				"play-services-gcm",
				PLAY_SERVICES_VERSION
			},
			namedArgs: new Dictionary<string, object>() {
				{"packageIds", new string[] { "extra-google-m2repository" } }
			});
		Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "DependOn",
			new object[] {
				"com.android.support",
				"support-v4",
				ANDROID_SUPPORT_VERSION
			},
			namedArgs: new Dictionary<string, object>() {
				{"packageIds", new string[] { "extra-android-m2repository" } }
			});
				
	}

	// Handle delayed loading of the dependency resolvers.
	private static void OnPostprocessAllAssets(
		string[] importedAssets, string[] deletedAssets,
		string[] movedAssets, string[] movedFromPath) {
		foreach (string asset in importedAssets) {
			if (asset.Contains("JarResolver")) {
				SetupDeps();
				break;
			}
		}
	}
}

#endif