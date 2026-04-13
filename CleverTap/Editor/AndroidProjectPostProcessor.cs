#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Private
{
    public class AndroidProjectPostProcessor : IPostGenerateGradleAndroidProject
    {
        private static readonly string ANDROID_XML_NS_URI = "http://schemas.android.com/apk/res/android";

        public int callbackOrder => 99;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            string androidProjectPath = path + "/clevertap-android-wrapper.androidlib";

            CopyAssetsToAndroidProject(androidProjectPath);
            CopyGoogleServicesJson(path);
            bool isDevelopmentBuild = EditorUserBuildSettings.development;
            CopySettingsToAndroidManifest(androidProjectPath, isDevelopmentBuild);

#if UNITY_6000_0_OR_NEWER
            EnsureNamespaceInBuildGradle(androidProjectPath);
            StripUnityClassesFromAndroidlibJar(androidProjectPath);
            CleanAndroidlibJavaSources(androidProjectPath);
            CopyWrapperJavaSourcesToUnityLibrary(path);
#else
            PatchBuildGradlesForLegacyUnity(path);
#endif
        }

        private static void CopyAssetsToAndroidProject(string androidProjectPath)
        {
            var unityAssetsPath = Path.Combine(Application.dataPath, EditorUtils.CLEVERTAP_ASSETS_FOLDER);
            if (!Directory.Exists(unityAssetsPath))
            {
                return;
            }
            var androidAssetsPath = Path.Combine(androidProjectPath, $"assets/{EditorUtils.CLEVERTAP_APP_ASSETS_FOLDER}");
            EditorUtils.DirectoryCopy(unityAssetsPath, androidAssetsPath, true, true,
                new HashSet<string>() { EditorUtils.CLEVERTAP_CUSTOM_TEMPLATES_FOLDER });
        }

        private static void CopyGoogleServicesJson(string unityLibraryPath)
        {
            string srcPath = Path.Combine(Application.dataPath, "google-services.json");
            if (!File.Exists(srcPath))
                return;

            // The launcher Gradle module sits alongside unityLibrary in the generated project
            string launcherPath = Path.Combine(Path.GetDirectoryName(unityLibraryPath), "launcher");
            if (!Directory.Exists(launcherPath))
                return;

            File.Copy(srcPath, Path.Combine(launcherPath, "google-services.json"), overwrite: true);
        }

        private static void StripUnityClassesFromAndroidlibJar(string androidProjectPath)
        {
            string classesJarPath = Path.Combine(androidProjectPath, "libs", "classes.jar");
            if (!File.Exists(classesJarPath))
                return;

            // Unity 6 pre-compiles the androidlib Java sources and bundles Unity player classes
            // (com.unity3d.player, bitter.jnibridge, org.fmod) into classes.jar alongside the
            // CleverTap wrapper classes. AGP 8.x checkDuplicateClasses then fails because those
            // same Unity classes are also in unity-classes.jar. Strip the Unity-provided classes
            // here — they remain available at runtime from unity-classes.jar.
            string tempPath = classesJarPath + ".tmp";
            using (var srcStream = File.OpenRead(classesJarPath))
            using (var srcArchive = new ZipArchive(srcStream, ZipArchiveMode.Read))
            using (var dstStream = File.Open(tempPath, FileMode.Create))
            using (var dstArchive = new ZipArchive(dstStream, ZipArchiveMode.Create))
            {
                foreach (var entry in srcArchive.Entries)
                {
                    if (entry.FullName.StartsWith("com/unity3d/") ||
                        entry.FullName.StartsWith("bitter/") ||
                        entry.FullName.StartsWith("org/fmod/"))
                        continue;

                    var dstEntry = dstArchive.CreateEntry(entry.FullName);
                    using var rs = entry.Open();
                    using var ws = dstEntry.Open();
                    rs.CopyTo(ws);
                }
            }

            File.Delete(classesJarPath);
            File.Move(tempPath, classesJarPath);
        }

        private static void PatchBuildGradlesForLegacyUnity(string unityLibraryPath)
        {
            string unityLibGradle = Path.Combine(unityLibraryPath, "build.gradle");
            if (File.Exists(unityLibGradle))
                PatchLegacyGradleFile(unityLibGradle);

            string launcherGradle = Path.Combine(Path.GetDirectoryName(unityLibraryPath), "launcher", "build.gradle");
            if (File.Exists(launcherGradle))
                PatchLegacyGradleFile(launcherGradle);
        }

        private static void PatchLegacyGradleFile(string gradlePath)
        {
            string content = File.ReadAllText(gradlePath);

            // Remove Unity symbol script apply lines that may not exist in all environments
            content = Regex.Replace(content, @"apply from: 'setupSymbols\.gradle'\r?\n?", "");
            content = Regex.Replace(content, @"apply from: '\.\./shared/keepUnitySymbols\.gradle'\r?\n?", "");

            // Java 17 required for AGP 8.x / latest Gradle
            content = content.Replace("JavaVersion.VERSION_11", "JavaVersion.VERSION_17");

            // Enforce SDK versions regardless of what Unity set.
            // Match both old-style (compileSdkVersion 33) and new-style (compileSdk 33).
            // The \b word boundary ensures 'compileSdk' doesn't match inside 'compileSdkVersion'.
            content = Regex.Replace(content, @"\bcompileSdkVersion\s+\d+", "compileSdkVersion 36");
            content = Regex.Replace(content, @"\bcompileSdk\b\s+\d+", "compileSdk 36");
            content = Regex.Replace(content, @"\bminSdkVersion\s+\d+", "minSdkVersion 26");
            content = Regex.Replace(content, @"\bminSdk\b\s+\d+", "minSdk 26");
            content = Regex.Replace(content, @"\btargetSdkVersion\s+\d+", "targetSdkVersion 36");
            content = Regex.Replace(content, @"\btargetSdk\b\s+\d+", "targetSdk 36");

            File.WriteAllText(gradlePath, content);
        }

        private static void EnsureNamespaceInBuildGradle(string androidProjectPath)
        {
            string buildGradlePath = Path.Combine(androidProjectPath, "build.gradle");
            if (!File.Exists(buildGradlePath))
                return;

            string content = File.ReadAllText(buildGradlePath);

            // Unity 6 generates build.gradle without namespace - inject it for AGP 8.x compatibility
            if (!content.Contains("namespace"))
                content = content.Replace("android {", "android {\n    namespace 'com.clevertap.unity'");

            File.WriteAllText(buildGradlePath, content);
        }

        private static void CleanAndroidlibJavaSources(string androidProjectPath)
        {
            // Wrapper Java sources are compiled in unityLibrary (not this androidlib) so they can
            // access UnityPlayerActivity which is only available as source there. Remove any sources
            // left here by previous builds to prevent stale compilations.
            string javaSrcDir = Path.Combine(androidProjectPath, "src", "main", "java");
            if (Directory.Exists(javaSrcDir))
            {
                Directory.Delete(javaSrcDir, recursive: true);
            }
        }

        private static void CopyWrapperJavaSourcesToUnityLibrary(string unityLibraryPath)
        {
            string packageRoot = FindPackageResolvedPath();
            if (packageRoot == null)
            {
                Debug.LogWarning("[CleverTap] Could not find package path; wrapper Java sources will not be copied to the generated project.");
                return;
            }

            string javaRoot = Path.Combine(packageRoot, "Plugins", "Android",
                "clevertap-android-wrapper.androidlib", "src", "main", "java");
            if (!Directory.Exists(javaRoot))
            {
                Debug.LogWarning($"[CleverTap] Wrapper Java sources not found at: {javaRoot}");
                return;
            }

            // Copy wrapper Java sources into unityLibrary (not the androidlib) so they compile
            // alongside UnityPlayerActivity which is only available as source in unityLibrary.
            string destRoot = Path.Combine(unityLibraryPath, "src", "main", "java");
            CopyDirectory(javaRoot, destRoot);
            Debug.Log("[CleverTap] Copied wrapper Java sources to unityLibrary.");
        }

        private static string FindPackageResolvedPath()
        {
            // Find this script's asset path to resolve the package root directory
            var guids = AssetDatabase.FindAssets($"t:Script {nameof(AndroidProjectPostProcessor)}");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var info = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(assetPath);
                if (info != null)
                    return info.resolvedPath;
            }
            return null;
        }

        private static void CopyDirectory(string srcDir, string dstDir)
        {
            Directory.CreateDirectory(dstDir);
            foreach (var file in Directory.GetFiles(srcDir))
                File.Copy(file, Path.Combine(dstDir, Path.GetFileName(file)), overwrite: true);
            foreach (var dir in Directory.GetDirectories(srcDir))
                CopyDirectory(dir, Path.Combine(dstDir, Path.GetFileName(dir)));
        }

        private static void CopySettingsToAndroidManifest(string androidProjectPath, bool isDevelopmentBuild)
        {
            var settings = AssetDatabase.LoadAssetAtPath<CleverTapSettings>(CleverTapSettings.settingsPath);
            if (settings == null)
            {
                Debug.Log($"CleverTapSettings have not been set.\n" +
                $"Please update them from {CleverTapSettingsWindow.ITEM_NAME} or " +
                $"set them manually in the project's AndroidManifest.xml.");
                return;
            }

            if (settings.Environments == null || settings.Environments.Items == null)
            {
                Debug.LogError("[CTExample] CleverTapSettings - Environments are not configured.");
                return;
            }

            Dictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential> environmentCredentials = settings.Environments.ToDictionary();

            if (environmentCredentials.Count == 0)
            {
                Debug.LogError("[CTExample] CleverTapSettings - Environments are not configured.");
                return;
            }

            if (!environmentCredentials.TryGetValue(settings.DefaultEnvironment, out CleverTapEnvironmentCredential environmentCredential))
            {
                Debug.LogError($"[CTExample] CleverTapSettings - Environment is null or not configured for {settings.DefaultEnvironment}");
                return;
            }

            string manifestFilePath = androidProjectPath + "/src/main/AndroidManifest.xml";

            // Unity 6 does not copy src/main/AndroidManifest.xml to the generated project - create it
            if (!File.Exists(manifestFilePath))
            {
                string manifestDir = Path.GetDirectoryName(manifestFilePath);
                if (!Directory.Exists(manifestDir))
                    Directory.CreateDirectory(manifestDir);
                File.WriteAllText(manifestFilePath,
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<manifest xmlns:android=\"http://schemas.android.com/apk/res/android\" />");
            }

            var manifestXml = new XmlDocument();
            manifestXml.Load(manifestFilePath);
            var manifestNode = manifestXml.SelectSingleNode("/manifest");
            if (manifestNode == null)
            {
                Debug.LogError("Failed to find manifest node in AndroidManifest.xml");
                return;
            }

#if UNITY_2021_3_OR_NEWER
            RemovePackageAttribute(manifestNode, manifestXml, manifestFilePath);
#endif

            if (manifestNode.Attributes["xmlns:android"] == null)
            {
                var nsAttribute = manifestXml.CreateAttribute("xmlns:android");
                nsAttribute.Value = ANDROID_XML_NS_URI;
                manifestNode.Attributes.Append(nsAttribute);
            }

            var namespaceManager = new XmlNamespaceManager(manifestXml.NameTable);
            if (!namespaceManager.HasNamespace("android"))
            {
                namespaceManager.AddNamespace("android", ANDROID_XML_NS_URI);
            }
            var applicationNode = manifestXml.SelectSingleNode("/manifest/application");
            if (applicationNode == null)
            {
                applicationNode = manifestXml.CreateElement("application");
                manifestNode.AppendChild(applicationNode);
            }

            UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_ACCOUNT_ID", environmentCredential.CleverTapAccountId);
            UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_TOKEN", environmentCredential.CleverTapAccountToken);
            UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_REGION", environmentCredential.CleverTapAccountRegion);
            UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_PROXY_DOMAIN", environmentCredential.CleverTapProxyDomain);
            UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_SPIKY_PROXY_DOMAIN", environmentCredential.CleverTapSpikyProxyDomain);

            if (isDevelopmentBuild)
            {
                Debug.Log($"[CleverTap] Development Build - Writing {environmentCredentials.Count} additional environment(s) to AndroidManifest.xml");
                UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_DEFAULT_ENV", settings.DefaultEnvironment.ToString());
                Debug.Log($"[CleverTap] Setting default environment to: {settings.DefaultEnvironment}");

                foreach (KeyValuePair<CleverTapEnvironmentKey, CleverTapEnvironmentCredential> cred in environmentCredentials)
                {
                    string suffix = "_" + cred.Key.ToString();
                    UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_ACCOUNT_ID" + suffix, cred.Value.CleverTapAccountId);
                    UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_TOKEN" + suffix, cred.Value.CleverTapAccountToken);
                    UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_REGION" + suffix, cred.Value.CleverTapAccountRegion);
                    UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_PROXY_DOMAIN" + suffix, cred.Value.CleverTapProxyDomain);
                    UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_SPIKY_PROXY_DOMAIN" + suffix, cred.Value.CleverTapSpikyProxyDomain);
                    Debug.Log($"[CleverTap] Added environment: {cred.Key} with AccountID: {cred.Value.CleverTapAccountId}");
                }
            }

            manifestXml.Save(manifestFilePath);
        }

        private static void UpdateMetaDataNode(XmlDocument manifestXml, XmlNode applicationNode, XmlNamespaceManager nsManager, string name, string value)
        {
            var hasNewValue = !string.IsNullOrWhiteSpace(value);
            var existingNode = applicationNode.SelectSingleNode($"meta-data[@android:name='{name}']", nsManager);
            if (existingNode != null)
            {
                if (hasNewValue)
                {
                    existingNode.Attributes["android:value"].Value = value;
                }
                else
                {
                    applicationNode.RemoveChild(existingNode);
                }
                return;
            }

            if (hasNewValue)
            {
                var newElement = manifestXml.CreateElement("meta-data");
                newElement.SetAttribute("name", ANDROID_XML_NS_URI, name);
                newElement.SetAttribute("value", ANDROID_XML_NS_URI, value);
                applicationNode.AppendChild(newElement);
            }
        }

        private static void RemovePackageAttribute(XmlNode manifestNode, XmlDocument manifestXml, string manifestFilePath)
        {
            if (manifestNode != null && manifestNode.Attributes?["package"] != null)
            {
                manifestNode.Attributes.Remove(manifestNode.Attributes["package"]);
                manifestXml.Save(manifestFilePath);
            }
        }
    }
}
#endif