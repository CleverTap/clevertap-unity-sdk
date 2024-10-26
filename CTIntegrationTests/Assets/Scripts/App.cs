using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
{
    public class App : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            CleverTap.SetDebugLevel(3);
            CleverTap.LaunchWithCredentials("ACCOUNT_ID", "ACCOUNT_TOKEN");
        }
    }
}