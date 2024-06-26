#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeMetaEventBuilder {
        internal UnityNativeMetaEventBuilder() { }

        internal Dictionary<string, object> BuildMeta() {
            var deviceInfo = UnityNativeDeviceManager.Instance.DeviceInfo;
            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;

            var metaDetails = new Dictionary<string, object>();
            metaDetails.Add(UnityNativeConstants.EventMeta.GUID, deviceInfo.DeviceId);
            metaDetails.Add(UnityNativeConstants.EventMeta.TYPE, UnityNativeConstants.EventMeta.TYPE_NAME);
            metaDetails.Add(UnityNativeConstants.EventMeta.APPLICATION_FIELDS, new UnityNativeEventBuilder().BuildAppFields());
            metaDetails.Add(UnityNativeConstants.EventMeta.ACCOUNT_ID, accountInfo.AccountId);
            metaDetails.Add(UnityNativeConstants.EventMeta.ACCOUNT_TOKEN, accountInfo.AccountToken);
            //TODO: metaDetails.Add(UnityNativeConstants.EventMeta.STORED_DEVICE_TOKEN, null); Add this implementation if needed
            metaDetails.Add(UnityNativeConstants.EventMeta.FIRST_REQUEST_IN_SESSION, UnityNativeSessionManager.Instance.IsFirstSession());
            metaDetails.Add(UnityNativeConstants.EventMeta.FIRST_REQUEST_TIMESTAMP, DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // Add this implementation
            metaDetails.Add(UnityNativeConstants.EventMeta.LAST_REQUEST_TIMESTAMP, DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // Add this implementation
           // TODO: 
            //metaDetails.Add(UnityNativeConstants.EventMeta.DEBUG_LEVEL, 3); // Add this implementation if needed
            //metaDetails.Add(UnityNativeConstants.EventMeta.SOURCE, null); // Add this implementation if needed
            //metaDetails.Add(UnityNativeConstants.EventMeta.MEDIUM, null); // Add this implementation if needed
            //metaDetails.Add(UnityNativeConstants.EventMeta.CAMPAIGN, null); // Add this implementation if needed
            //metaDetails.Add(UnityNativeConstants.EventMeta.SOURCE, null); // Add this implementation if needed
            //metaDetails.Add(UnityNativeConstants.EventMeta.REF, null); // Add this implementation if needed
            //metaDetails.Add(UnityNativeConstants.EventMeta.WZRK_REF, null); // Add this implementation if needed

            return metaDetails;
        }
    }
}
#endif