﻿#if UNITY_ANDROID
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;

namespace CleverTapSDK.Android {
    internal class AndroidTemplateContext : CleverTapTemplateContext
    {
        public AndroidTemplateContext(string templateName) : base(templateName) { }

        public override bool? GetBoolean(string name)
        {
           return CleverTapAndroidJNI.CleverTapJNIInstance.Call<bool?>("customTemplateGetBooleanArg", TemplateName);
        }

        public override Dictionary<string, object> GetDictionary(string name)
        {
            string json = CleverTapAndroidJNI.CleverTapJNIInstance.Call<string>("customTemplateGetDictionaryJsonArg", TemplateName);

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
                UnityEngine.Debug.Log($"CustomTemplates: Error getting dictionary value for name: {name}. Exception: {ex.Message}");
            }

            return default;
        }

        public override string GetFile(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<string>("customTemplateGetFileArg", TemplateName);
        }

        public override byte? GetByte(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<byte?>("customTemplateGetByteArg", TemplateName);
        }

        public override short? GetShort(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<short?>("customTemplateGetShortArg", TemplateName);
        }

        public override int? GetInt(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<int?>("customTemplateGetIntArg", TemplateName);
        }

        public override long? GetLong(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<long?>("customTemplateGetLongArg", TemplateName);
        }

        public override float? GetFloat(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<float?>("customTemplateGetFloatArg", TemplateName);
        }

        public override double? GetDouble(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<double?>("customTemplateGetDoubleArg", TemplateName);
        }

        public override string GetString(string name)
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<string>("customTemplateGetStringArg", TemplateName);
        }

        public override void SetDismissed()
        {
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("customTemplateSetDismissed", TemplateName);
        }

        public override void SetPresented()
        {
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("customTemplateSetPresented", TemplateName);
        }

        public override void TriggerAction(string name)
        {
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("customTemplateTriggerAction", TemplateName);
        }

        internal override string GetTemplateString()
        {
            return CleverTapAndroidJNI.CleverTapJNIInstance.Call<string>("customTemplateContextToString", TemplateName);
        }
    }
}
#endif