#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeMetaEventBuilder {
        internal UnityNativeMetaEventBuilder() { }

        internal Dictionary<string, object> BuildMeta() {
            var deviceInfo = UnityNativeDeviceManager.Instance.DeviceInfo;
            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;

            var metaDetails = new Dictionary<string, object>
            {
                { UnityNativeConstants.EventMeta.GUID, deviceInfo.DeviceId },
                { UnityNativeConstants.EventMeta.TYPE, UnityNativeConstants.EventMeta.TYPE_NAME },
                { UnityNativeConstants.EventMeta.APPLICATION_FIELDS, new UnityNativeEventBuilder().BuildAppFields() },
                { UnityNativeConstants.EventMeta.ACCOUNT_ID, accountInfo.AccountId },
                { UnityNativeConstants.EventMeta.ACCOUNT_TOKEN, accountInfo.AccountToken },
                { UnityNativeConstants.EventMeta.FIRST_REQUEST_IN_SESSION, UnityNativeSessionManager.Instance.IsFirstSession() }
            };

            return metaDetails;
        }
    }
}
#endif