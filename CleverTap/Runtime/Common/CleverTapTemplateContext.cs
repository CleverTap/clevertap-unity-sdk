using System.Collections.Generic;

namespace CleverTapSDK.Common
{
    public abstract class CleverTapTemplateContext
    {
        public abstract string TemplateName { get; }

        public abstract void SetPresented();
        public abstract void SetDismissed();
        public abstract void TriggerAction(string name);

        public abstract string GetString(string name);

        public abstract bool? GetBoolean(string name);

        public abstract Dictionary<string, object> GetDictionary(string name);

        public abstract string GetFile(string name);

        public abstract T GetNumber<T>(string name);

        internal abstract string GetTemplateString();
    }
}

