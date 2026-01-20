#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
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
            bool isDevelopmentBuild = EditorUserBuildSettings.development;
            CopySettingsToAndroidManifest(androidProjectPath, isDevelopmentBuild);
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

            Dictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential> environmentCredentials = settings.Environments.ToDictionary();

            if (environmentCredentials == null || environmentCredentials.Count == 0)
            {
                Debug.LogError("[CTExample] CleverTapSettings - Environments are not configured.");
                return;
            }

            if (!environmentCredentials.TryGetValue(settings.DefaultEnvionment, out CleverTapEnvironmentCredential environmentCredential))
            {
                Debug.LogError($"[CTExample] CleverTapSettings - Environment is null or not configured for {settings.DefaultEnvionment}");
                return;
            }

            string manifestFilePath = androidProjectPath + "/src/main/AndroidManifest.xml";
            var manifestXml = new XmlDocument();
            manifestXml.Load(manifestFilePath);
            var manifestNode = manifestXml.SelectSingleNode("/manifest");
            if (manifestNode == null)
            {
                Debug.LogError("Failed to find manifest node in AndroidManifest.xml");
                return;
            }
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
                UpdateMetaDataNode(manifestXml, applicationNode, namespaceManager, "CLEVERTAP_DEFAULT_ENV", settings.DefaultEnvionment.ToString());
                Debug.Log($"[CleverTap] Setting default environment to: {settings.DefaultEnvionment}");

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
    }
}
#endif
