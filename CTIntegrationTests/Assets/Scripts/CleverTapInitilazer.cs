using CleverTapSDK;
using CleverTapSDK.Utilities;
using UnityEngine;

public class CleverTapTest : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        CleverTap.SetLogLevel(LogLevel.Debug);
        CleverTap.SetDebugLevel(4);
        CleverTap.LaunchWithCredentialsForRegion("468-RZW-ZK6Z", "012-b64", "sk1");
        CleverTap.SetOptOut(false);
    }
}
