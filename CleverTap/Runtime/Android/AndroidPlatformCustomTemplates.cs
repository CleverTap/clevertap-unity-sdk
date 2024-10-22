#if UNITY_ANDROID
using CleverTapSDK.Common;

namespace CleverTapSDK.Android {
    internal class AndroidPlatformCustomTemplates : CleverTapPlatformCustomTemplates
    {
        internal override CleverTapTemplateContext CreateContext(string name)
        {
            throw new System.NotImplementedException();
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
