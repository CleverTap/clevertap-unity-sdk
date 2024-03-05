#if !UNITY_IOS && !UNITY_ANDROID 
using CleverTapSDK.Common;
using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativePlatformBinding : CleverTapPlatformBindings {
        private readonly UnityNativeWrapper _unityNativeWrapper;

        internal UnityNativePlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>(CleverTapGameObjectName.UNITY_NATIVE_CALLBACK_HANDLER);
            _unityNativeWrapper = new UnityNativeWrapper();
            CleverTapLogger.Log("Start: no-op CleverTap binding for non iOS/Android.");
        }

        internal override void LaunchWithCredentials(string accountID, string token) {
            _unityNativeWrapper.LaunchWithCredentials(accountID, token);
        }

        internal override void LaunchWithCredentialsForRegion(string accountID, string token, string region) {
            _unityNativeWrapper.LaunchWithCredentials(accountID, token, region);
        }

        internal override void OnUserLogin(Dictionary<string, object> properties) {
            _unityNativeWrapper.OnUserLogin(properties);
        }

        internal override void ProfilePush(Dictionary<string, object> properties) {
            _unityNativeWrapper.ProfilePush(properties);
        }

        internal override void RecordEvent(string eventName) {
            _unityNativeWrapper.RecordEvent(eventName);
        }

        internal override void RecordEvent(string eventName, Dictionary<string, object> properties) {
            _unityNativeWrapper.RecordEvent(eventName, properties);
        }

        internal override void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            _unityNativeWrapper.RecordChargedEventWithDetailsAndItems(details, items);
        }
    }
}
#endif
