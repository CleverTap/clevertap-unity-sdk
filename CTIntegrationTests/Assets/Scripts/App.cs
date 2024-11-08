using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
{
    public class App : MonoBehaviour
    {
        public string accountName = "ACCOUNT_NAME";
        public string accountId = "ACCOUNT_ID";
        [SerializeField] private string accountToken = "ACCOUNT_TOKEN";
        [SerializeField] private string accountRegion = "ACCOUNT_REGION";

        void Awake()
        {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Logger.Log($"Setting targetFrameRate to: {(int)Screen.currentResolution.refreshRateRatio.value}");
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
            CleverTap.SetDebugLevel(3);
            CleverTap.LaunchWithCredentialsForRegion(accountId, accountToken, accountRegion);
            Logger.Log($"Launching \"{accountName}\" with accountId: {accountId}, accountToken: {accountToken}, accountRegion: {accountRegion}");
        }
    }
}