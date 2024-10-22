#if UNITY_IOS
using CleverTapSDK.Common;

namespace CleverTapSDK.IOS
{
    internal class IOSPlatformCustomTemplates : CleverTapPlatformCustomTemplates
    {
        internal override CleverTapTemplateContext CreateContext(string name)
        {
            return new IOSTemplateContext(name);
        }

        internal override void SyncCustomTemplates()
        {
            throw new System.NotImplementedException();
        }

        internal override void SyncCustomTemplates(bool isProduction)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif