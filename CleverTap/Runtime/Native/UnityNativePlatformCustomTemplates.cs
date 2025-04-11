#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;
using static CleverTapSDK.Native.UnityNativeConstants;

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
            Debug.Log($"files: {string.Join(",", files.Select(f => f.Name).ToList())}");
            HashSet<CustomTemplate> templates = new HashSet<CustomTemplate>();
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".json")
                    continue;

                string json = File.ReadAllText(file.FullName);
                JsonTemplateProducer producer = new JsonTemplateProducer(json);
                templates.UnionWith(producer.DefineTemplates());
            }

            var payload = SyncPayload(templates);
            unityNativeEventManager.SyncCustomTemplates(payload);
        }

        private Dictionary<string, object> SyncPayload(HashSet<CustomTemplate> templates)
        {
            var payload = new Dictionary<string, object>();
            payload["type"] = "templatePayload";

            var definitions = new Dictionary<string, object>();

            foreach (var template in templates)
            {
                var templateData = new Dictionary<string, object>();
                templateData["type"] = template.TemplateType;

                var groupedMap = new Dictionary<string, List<TemplateArgument>>();
                foreach (var arg in template.Arguments)
                {
                    var components = arg.Name.Split(CustomTemplates.DOT);
                    var firstComponent = components[0];
                    if (!groupedMap.ContainsKey(firstComponent))
                    {
                        groupedMap[firstComponent] = new List<TemplateArgument>();
                    }
                    groupedMap[firstComponent].Add(arg);
                }

                var ordered = new HashSet<string>();
                int order = 0;
                var arguments = new Dictionary<string, object>();

                foreach (var arg in template.Arguments)
                {
                    if (!arg.Name.Contains(CustomTemplates.DOT))
                    {
                        var argument = ArgumentPayload(arg, order);
                        arguments[arg.Name] = argument;
                        order++;
                    }
                    else
                    {
                        order = AddGroupArguments(groupedMap, ordered, order, arguments, arg);
                    }
                }

                templateData["vars"] = arguments;
                definitions[template.Name] = templateData;
            }

            payload["definitions"] = definitions;
            return payload;
        }

        private int AddGroupArguments(Dictionary<string, List<TemplateArgument>> groupedMap, HashSet<string> ordered, int order, Dictionary<string, object> arguments, TemplateArgument arg)
        {
            var prefix = arg.Name.Split(CustomTemplates.DOT)[0];
            if (!ordered.Contains(prefix))
            {
                ordered.Add(prefix);
                var groupedArguments = groupedMap[prefix];
                groupedArguments.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

                foreach (var nestedArg in groupedArguments)
                {
                    var argument = ArgumentPayload(nestedArg, order);
                    arguments[nestedArg.Name] = argument;
                    order++;
                }
            }

            return order;
        }

        private IDictionary<string, object> ArgumentPayload(TemplateArgument arg, int order)
        {
            var argument = new Dictionary<string, object>();
            if (arg.Value != null)
            {
                argument["defaultValue"] = arg.Value;
            }

            argument["type"] = arg.Type;
            argument["order"] = order;
            return argument;
        }
    }
}
#endif
