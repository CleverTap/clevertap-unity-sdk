#if !UNITY_IOS && !UNITY_ANDROID
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeAccountManager {
        private static readonly Lazy<UnityNativeAccountManager> instance = new Lazy<UnityNativeAccountManager>(() => new UnityNativeAccountManager());

        private UnityNativeAccountInfo _accountInfo;

        private UnityNativeAccountManager() {
            new UnityNativeAccountInfo();
        }

        internal static UnityNativeAccountManager Instance => instance.Value;

        internal UnityNativeAccountInfo AccountInfo => _accountInfo;

        internal UnityNativeAccountInfo SetAccountInfo(string accountId, string accountToken, string region = null) {
            _accountInfo = new UnityNativeAccountInfo(accountId, accountToken, region);
            return _accountInfo;
        }
    }
}
#endif