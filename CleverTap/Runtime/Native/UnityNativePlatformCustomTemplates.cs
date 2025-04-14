#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Native
{
    internal class UnityNativePlatformCustomTemplates : CleverTapPlatformCustomTemplates
    {
        private static readonly string CLEVERTAP_ASSETS_FOLDER = "CleverTap";
        private static readonly string CLEVERTAP_CUSTOM_TEMPLATES_FOLDER = "CustomTemplates";

        private UnityNativeEventManager unityNativeEventManager;

        internal override CleverTapTemplateContext CreateContext(string name)
        {
            CleverTapLogger.LogError("CleverTap Error: CreateContext is not supported for this platform.");
            return null;
        }

        /// <summary>
        /// Sets the dependencies.
        /// Call this method as soon as the SDK initializes.
        /// </summary>
        /// <param name="unityNativeEventManager">The event manager instance.</param>
        internal void Load(UnityNativeEventManager unityNativeEventManager)
        {
            if (unityNativeEventManager == null)
            {
                CleverTapLogger.LogError("Cannot load with null unityNativeEventManager.");
                return;
            }
            this.unityNativeEventManager = unityNativeEventManager;
        }

        internal override void SyncCustomTemplates() => SyncCustomTemplates(false);

        internal override void SyncCustomTemplates(bool isProduction)
        {
            if (isProduction)
            {
                if (Debug.isDebugBuild)
                {
                    CleverTapLogger.Log("Calling SyncCustomTemplates(true) from Debug build. Do not use (isProduction: true) in this case.");
                }
                else
                {
                    CleverTapLogger.Log("Calling SyncCustomTemplates(true) from Release build. Do not release this build and use with caution.");
                }
            }
            else
            {
                if (!Debug.isDebugBuild)
                {
                    CleverTapLogger.Log("SyncCustomTemplates() can only be called from Debug builds.");
                    return;
                }
            }

            if (unityNativeEventManager == null)
            {
                CleverTapLogger.LogError("CleverTap Error: Cannot sync custom templates." +
                    " The platform is not loaded.");
                return;
            }

            string sourceFolderPath = Path.Combine(Application.dataPath, CLEVERTAP_ASSETS_FOLDER, CLEVERTAP_CUSTOM_TEMPLATES_FOLDER);
            DirectoryInfo dir = new DirectoryInfo(sourceFolderPath);
            if (!dir.Exists)
            {
                return;
            }

            FileInfo[] files = dir.GetFiles();
            HashSet<CustomTemplate> templates = new HashSet<CustomTemplate>();
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".json")
                    continue;

                string json = File.ReadAllText(file.FullName);
                JsonTemplateProducer producer = new JsonTemplateProducer(json);
                templates.UnionWith(producer.DefineTemplates());
            }

            var payload = CustomTemplateUtils.GetSyncTemplatesPayload(templates);
            unityNativeEventManager.SyncCustomTemplates(payload);
        }
    }
}
#endif
