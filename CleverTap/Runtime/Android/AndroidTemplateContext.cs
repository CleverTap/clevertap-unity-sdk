#if UNITY_ANDROID
using CleverTapSDK.Common;
using System;
using System.Collections.Generic;

namespace CleverTapSDK.Android {
    internal class AndroidTemplateContext : CleverTapTemplateContext
    {
        public override string TemplateName => throw new NotImplementedException();

        public override bool? GetBoolean(string name)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> GetDictionary(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetFile(string name)
        {
            throw new NotImplementedException();
        }

        public override T GetNumber<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetString(string name)
        {
            throw new NotImplementedException();
        }

        public override void SetDismissed()
        {
            throw new NotImplementedException();
        }

        public override void SetPresented()
        {
            throw new NotImplementedException();
        }

        public override void TriggerAction(string name)
        {
            throw new NotImplementedException();
        }

        internal override string GetTemplateString()
        {
            throw new NotImplementedException();
        }
    }
}
#endif