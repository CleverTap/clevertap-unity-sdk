#if UNITY_IOS
using System;
using CleverTapSDK.Common;

namespace CleverTapSDK.IOS {
    public class IOSCallbackHandler : CleverTapCallbackHandler
    {
        internal override void OnCallbackAdded(Action<string> callbackMethod)
        {
            IOSDllImport.CleverTap_onCallbackAdded(callbackMethod.Method.Name);
        }
    }
}
#endif