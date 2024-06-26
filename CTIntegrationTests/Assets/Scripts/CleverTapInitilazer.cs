using CleverTapSDK;
using CleverTapSDK.Utilities;
using UnityEngine;

public class CleverTapTest : MonoBehaviour
{
    public string CLEVERTAP_ACCOUNT_ID = "67Z-RRK-696Z";
    public string CLEVETAP_ACCOUNT_TOKEN = "WFK-ISB-OXUL";
    public string CLEVERTAP_REGION_CODE = "sk1";
    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        CleverTap.SetLogLevel(LogLevel.Debug);
        CleverTap.SetDebugLevel(4);
        CleverTap.LaunchWithCredentialsForRegion(CLEVERTAP_ACCOUNT_ID,CLEVETAP_ACCOUNT_TOKEN , CLEVERTAP_REGION_CODE);
        CleverTap.SetOptOut(false);
    }

}
