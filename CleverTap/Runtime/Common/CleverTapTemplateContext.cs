using System.Collections.Generic;

namespace CleverTapSDK.Common
{
    public abstract class CleverTapTemplateContext
    {
        public readonly string TemplateName;
        public CleverTapTemplateContext(string templateName)
        {
            TemplateName = templateName;
        }

        public abstract void SetPresented();
        public abstract void SetDismissed();
        public abstract void TriggerAction(string name);

        public abstract string GetString(string name);

        public abstract bool? GetBoolean(string name);

        public abstract Dictionary<string, object> GetDictionary(string name);

        public abstract string GetFile(string name);

        public abstract byte? GetByte(string name);

        public abstract short? GetShort(string name);

        public abstract int? GetInt(string name);

        public abstract long? GetLong(string name);

        public abstract float? GetFloat(string name);

        public abstract double? GetDouble(string name);

        internal abstract string GetTemplateString();

        public override string ToString()
        {
            return GetTemplateString();
        }
    }
}

