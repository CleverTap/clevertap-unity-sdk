using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
{
    public class App : MonoBehaviour
    {
        public string accountId = "ACCOUNT_ID";
        [SerializeField] private string accountToken = "ACCOUNT_TOKEN";

        void Awake()
        {
            CleverTap.SetDebugLevel(3);
            CleverTap.LaunchWithCredentials(accountId, accountToken);
        }
    }
}