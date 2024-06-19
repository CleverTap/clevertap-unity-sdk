using CleverTapSDK;
using CleverTapSDK.Utilities;
using UnityEngine;

public class CleverTapTest : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        CleverTap.SetLogLevel(LogLevel.Debug);
        CleverTap.SetDebugLevel(4);
        CleverTap.LaunchWithCredentialsForRegion(GetId(), "012-b64", "sk1");
        CleverTap.SetOptOut(false);
    }

    string GetId()
    {
        return PlayerPrefs.GetString("clever_tap_id", "468-RZW-ZK6Z");
    }
}
