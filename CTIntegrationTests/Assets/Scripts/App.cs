using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
{
    public class App : MonoBehaviour
    {
        public string accountName = "ACCOUNT_NAME";
        public string accountId = "ACCOUNT_ID";
        [SerializeField] private string accountToken = "ACCOUNT_TOKEN";

        void Awake()
        {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Logger.Log($"Setting targetFrameRate to: {(int)Screen.currentResolution.refreshRateRatio.value}");
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
            CleverTap.SetDebugLevel(3);
            CleverTap.LaunchWithCredentials(accountId, accountToken);
        }
    }
}