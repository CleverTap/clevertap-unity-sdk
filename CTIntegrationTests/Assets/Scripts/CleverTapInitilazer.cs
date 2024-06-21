using CleverTapSDK;
using CleverTapSDK.Utilities;
using UnityEngine;

public class CleverTapTest : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        CleverTap.SetLogLevel(LogLevel.Debug);
        CleverTap.SetDebugLevel(4);
        CleverTap.LaunchWithCredentialsForRegion(GetId(), "WFK-ISB-OXUL", "sk1");
        CleverTap.SetOptOut(false);
    }

    string GetId()
    {
        return PlayerPrefs.GetString("clever_tap_id", "67Z-RRK-696Z");
    }
}
