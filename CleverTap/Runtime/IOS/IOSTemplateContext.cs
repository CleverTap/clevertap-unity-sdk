#if UNITY_IOS
using System;
using System.Collections.Generic;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.IOS
{
    internal class IOSTemplateContext : CleverTapTemplateContext
    {
        private readonly string templateName;

        internal IOSTemplateContext(string name)
        {
            templateName = name;
        }

        public override string TemplateName => templateName;

        public override void SetDismissed()
        {
            IOSDllImport.CleverTap_customTemplateSetDismissed(TemplateName);
        }

        public override void SetPresented()
        {
            IOSDllImport.CleverTap_customTemplateSetPresented(TemplateName);
        }

        public override string GetString(string name)
        {
            return IOSDllImport.CleverTap_customTemplateGetStringArg(TemplateName, name);
        }

        public override bool? GetBoolean(string name)
        {
            return IOSDllImport.CleverTap_customTemplateGetBooleanArg(TemplateName, name);
        }

        public override Dictionary<string, object> GetDictionary(string name)
        {
            string json = IOSDllImport.CleverTap_customTemplateGetDictionaryArg(TemplateName, name);

            try
            {
                if (json != null)
                {
                    var value = Json.Deserialize(json);
                    // Defaults to (T)value if not a collection
                    return value as Dictionary<string, object>;
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"Leanplum: Error getting dictionary value for name: {name}. Exception: {ex.Message}");
            }

            return default;
        }

        public override string GetFile(string name)
        {
            return IOSDllImport.CleverTap_customTemplateGetFileArg(TemplateName, name);
        }

        public override T GetNumber<T>(string name)
        {
            Type t = typeof(T);
            if (t == typeof(int))
            {
                return (T)(object)IOSDllImport.CleverTap_customTemplateGetIntArg(TemplateName, name);
            }
            else if (t == typeof(double))
            {
                return (T)(object)IOSDllImport.CleverTap_customTemplateGetDoubleArg(TemplateName, name);
            }
            else if (t == typeof(float))
            {
                return (T)(object)IOSDllImport.CleverTap_customTemplateGetFloatArg(TemplateName, name);
            }
            else if (t == typeof(long))
            {
                return (T)(object)IOSDllImport.CleverTap_customTemplateGetLongArg(TemplateName, name);
            }
            else if (t == typeof(short))
            {
                return (T)(object)IOSDllImport.CleverTap_customTemplateGetShortArg(TemplateName, name);
            }
            else if (t == typeof(byte))
            {
                return (T)(object)IOSDllImport.CleverTap_customTemplateGetByteArg(TemplateName, name);
            }

            return default;
        }

        public override void TriggerAction(string name)
        {
            IOSDllImport.CleverTap_customTemplateTriggerAction(TemplateName, name);
        }

        internal override string GetTemplateString()
        {
            return IOSDllImport.CleverTap_customTemplateContextToString(TemplateName);
        }

        public override string ToString()
        {
            return GetTemplateString();
        }
    }
}
#endif

