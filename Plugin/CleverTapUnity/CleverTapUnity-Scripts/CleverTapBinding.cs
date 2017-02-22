using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CleverTap.Utilities;

/// <summary>
/// These methods can be called by Unity applications to record
/// events and set and get user profile attributes. 
/// </summary>

namespace CleverTap {
  public class CleverTapBinding : MonoBehaviour {
    public const string Version = "1.0.2";

#if UNITY_IOS
    void Start() {
        Debug.Log("Start: CleverTap binding for iOS.");
    }

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_launchWithCredentials(string accountID, string token);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_onUserLogin(string properties);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profilePush(string properties);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profilePushGraphUser(string user);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profilePushGooglePlusUser(string user);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string CleverTap_profileGet(string key);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string CleverTap_profileGetCleverTapAttributionIdentifier();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string CleverTap_profileGetCleverTapID();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profileRemoveValueForKey(string key);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profileSetMultiValuesForKey(string key, string[] array, int size);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profileAddMultiValuesForKey(string key, string[] array, int size);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profileRemoveMultiValuesForKey(string key, string[] array, int size);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profileAddMultiValueForKey(string key, string val);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_profileRemoveMultiValueForKey(string key, string val);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_recordEvent(string eventName, string properties);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_recordChargedEventWithDetailsAndItems(string details, string items);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_registerPush();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_setApplicationIconBadgeNumber(int num);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_setDebugLevel(int level);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_enablePersonalization();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_disablePersonalization();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void CleverTap_setLocation(double lat, double lon);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_eventGetFirstTime(string eventName);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_eventGetLastTime(string eventName);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_eventGetOccurrences(string eventName);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string CleverTap_userGetEventHistory();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string CleverTap_sessionGetUTMDetails();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_sessionGetTimeElapsed();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string CleverTap_eventGetDetail(string eventName);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_userGetTotalVisits();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_userGetScreenCount();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int CleverTap_userGetPreviousVisitTime();

    public static void LaunchWithCredentials(string accountID, string token) {
        CleverTap_launchWithCredentials(accountID, token);
    }
        
    public static void OnUserLogin(Dictionary<string, string> properties) {
        var propertiesString = Json.Serialize(properties);
        CleverTap_onUserLogin(propertiesString);
    }

    public static void ProfilePush(Dictionary<string, string> properties) {
        var propertiesString = Json.Serialize(properties);
        CleverTap_profilePush(propertiesString);
    }

    public static void ProfilePushGraphUser(Dictionary<string, string> user) {
        var userString = Json.Serialize(user);
        CleverTap_profilePushGraphUser(userString);
    }

    public static void ProfilePushGooglePlusUser(Dictionary<string, string> user) {
        var userString = Json.Serialize(user);
        CleverTap_profilePushGooglePlusUser(userString);
    }

    public static string ProfileGet(string key) {
        string ret = CleverTap_profileGet(key);
        return ret;
    }

    public static string ProfileGetCleverTapAttributionIdentifier() {
        string ret = CleverTap_profileGetCleverTapAttributionIdentifier();
        return ret;
    }

    public static string ProfileGetCleverTapID() {
        string ret = CleverTap_profileGetCleverTapID();
        return ret;
    }

    public static void ProfileRemoveValueForKey(string key) {
        CleverTap_profileRemoveValueForKey(key);
    }

    public static void ProfileSetMultiValuesForKey(string key, List<string> values) {
        CleverTap_profileSetMultiValuesForKey(key, values.ToArray(), values.Count);
    }

    public static void ProfileAddMultiValuesForKey(string key, List<string> values) {
        CleverTap_profileAddMultiValuesForKey(key, values.ToArray(), values.Count);
    }

    public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
        CleverTap_profileRemoveMultiValuesForKey(key, values.ToArray(), values.Count);
    }

    public static void ProfileAddMultiValueForKey(string key, string val) {
        CleverTap_profileAddMultiValueForKey(key, val);
    }

    public static void ProfileRemoveMultiValueForKey(string key, string val) {
        CleverTap_profileRemoveMultiValueForKey(key, val);
    }

    public static void RecordEvent(string eventName) {
        CleverTap_recordEvent(eventName, null);
    }
        
    public static void RecordEvent(string eventName, Dictionary<string, object> properties) {
        var propertiesString = Json.Serialize(properties);
        CleverTap_recordEvent(eventName, propertiesString);
    }

    public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>>items) {
        var detailsString = Json.Serialize(details);
        var itemsString = Json.Serialize(items);
        CleverTap_recordChargedEventWithDetailsAndItems(detailsString, itemsString);
    }

    public static int EventGetFirstTime(string eventName) {
        return CleverTap_eventGetFirstTime(eventName);
    }

    public static int EventGetLastTime(string eventName) {
        return CleverTap_eventGetLastTime(eventName);
    }

    public static int EventGetOccurrences(string eventName) {
        return CleverTap_eventGetOccurrences(eventName);
    }

    public static JSONClass UserGetEventHistory() {
        string jsonString = CleverTap_userGetEventHistory();
        JSONClass json;
        try {
            json = (JSONClass)JSON.Parse(jsonString);
        } catch {
            Debug.Log("Unable to parse user event history json");
            json = new JSONClass();
        }
        return json;
    }

    public static JSONClass SessionGetUTMDetails() {
        string jsonString = CleverTap_sessionGetUTMDetails();
        JSONClass json;
        try {
            json = (JSONClass)JSON.Parse(jsonString);
        } catch {
            Debug.Log("Unable to parse session utm details json");
            json = new JSONClass();
        }
        return json;
    }

    public static int SessionGetTimeElapsed() {
        return CleverTap_sessionGetTimeElapsed();
    }

    public static JSONClass EventGetDetail(string eventName) {
        string jsonString = CleverTap_eventGetDetail(eventName);
        JSONClass json;
        try {
            json = (JSONClass)JSON.Parse(jsonString);
        } catch {
            Debug.Log("Unable to parse event detail json");
            json = new JSONClass();
        }
        return json;
    }

    public static int UserGetTotalVisits() {
        return CleverTap_userGetTotalVisits();
    }

    public static int UserGetScreenCount() {
        return CleverTap_userGetScreenCount();
    }

    public static int UserGetPreviousVisitTime() {
        return CleverTap_userGetPreviousVisitTime();
    }
        
    public static void RegisterPush() {
        CleverTap_registerPush();
    }

    public static void SetApplicationIconBadgeNumber(int num) {
        CleverTap_setApplicationIconBadgeNumber(num);
    }
        
    public static void SetDebugLevel(int level) {
        CleverTap_setDebugLevel(level);
    }

    public static void EnablePersonalization() {
        CleverTap_enablePersonalization();
    }

    public static void DisablePersonalization() {
        CleverTap_disablePersonalization();
    }

    public static void SetLocation(double lat, double lon) {
        CleverTap_setLocation(lat, lon);
    }
        
#elif UNITY_ANDROID
    private static AndroidJavaObject unityActivity;
    private static AndroidJavaObject clevertap;
	private static AndroidJavaObject CleverTapClass;
  
    void Start() {
        Debug.Log("Start: CleverTap binding for Android.");
    }

    #region Properties
    public static AndroidJavaObject unityCurrentActivity {
        get {
            if (unityActivity == null) {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
            }
            return unityActivity;
        }
    }

	public static AndroidJavaObject CleverTapAPI {
        get {
            if (CleverTapClass == null) {
                CleverTapClass = new AndroidJavaClass("com.clevertap.unity.CleverTapUnityPlugin");
            }
            return CleverTapClass;
        }
	}

    public static AndroidJavaObject CleverTap {
        get {
            if (clevertap == null) {
                AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
                clevertap = CleverTapAPI.CallStatic<AndroidJavaObject>("getInstance", context);
            }
            return clevertap;
        }
    }
    #endregion

    public static void SetDebugLevel(int level) {
        CleverTapAPI.CallStatic("setDebugLevel", level);
    }
    
    public static void Initialize(string accountID, string accountToken) {
        CleverTapAPI.CallStatic("initialize", accountID, accountToken, unityCurrentActivity);
    }
    
    public static void EnablePersonalization() {
        CleverTap.Call("enablePersonalization");
    }
    
    public static void DisablePersonalization() {
        CleverTap.Call("disablePersonalization");
    }
    
    public static void SetLocation(double lat, double lon) {
        CleverTap.Call("setLocation", lat, lon);
    }
    
    public static void OnUserLogin(Dictionary<string, string> properties) {
        CleverTap.Call("onUserLogin", Json.Serialize(properties));
    }
    
    public static void ProfilePush(Dictionary<string, string> properties) {
        CleverTap.Call("profilePush", Json.Serialize(properties));
    }
    
    public static void ProfilePushFacebookUser(Dictionary<string, string> user) {
        CleverTap.Call("profilePushFacebookUser", Json.Serialize(user));
    }
    
    public static string ProfileGet(string key) {
        return CleverTap.Call<string>("profileGet", key);
    }
    
    public static string ProfileGetCleverTapAttributionIdentifier() {
        return CleverTap.Call<string>("profileGetCleverTapAttributionIdentifier");
    }
    
    public static string ProfileGetCleverTapID() {
        return CleverTap.Call<string>("profileGetCleverTapID");
    }
    
    public static void ProfileRemoveValueForKey(string key) {
        CleverTap.Call("profileRemoveValueForKey", key);
    }
    
    public static void ProfileSetMultiValuesForKey(string key, List<string> values) {
        CleverTap.Call("profileSetMultiValuesForKey", key, values.ToArray());
    }

    public static void ProfileAddMultiValuesForKey(string key, List<string> values) {
        CleverTap.Call("profileAddMultiValuesForKey", key, values.ToArray());	
    }
    
    public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
        CleverTap.Call("profileRemoveMultiValuesForKey", key, values.ToArray());
    }
    
    public static void ProfileAddMultiValueForKey(string key, string val) {
        CleverTap.Call("profileAddMultiValueForKey", key, val);
    }
    
    public static void ProfileRemoveMultiValueForKey(string key, string val) {
        CleverTap.Call("profileRemoveMultiValueForKey", key, val);
    }
    
    public static void RecordEvent(string eventName) {
        CleverTap.Call("recordEvent", eventName, null);
    }
    
    public static void RecordEvent(string eventName, Dictionary<string, object> properties) {
        CleverTap.Call("recordEvent", eventName, Json.Serialize(properties));
    }
    
    public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>>items) {
        CleverTap.Call("recordChargedEventWithDetailsAndItems", Json.Serialize(details), Json.Serialize(items));
    }
    
    public static int EventGetFirstTime(string eventName) {
        return CleverTap.Call<int>("eventGetFirstTime", eventName);
    }
    
    public static int EventGetLastTime(string eventName) {
        return CleverTap.Call<int>("eventGetLastTime", eventName);
    }
    
    public static int EventGetOccurrences(string eventName) {
        return CleverTap.Call<int>("eventGetOccurrences", eventName);
    }
    
    public static JSONClass EventGetDetail(string eventName) {
        string jsonString = CleverTap.Call<string>("eventGetDetail", eventName);
        JSONClass json;
        try {
            json = (JSONClass)JSON.Parse(jsonString);
        } catch {
            Debug.Log("Unable to event detail json");
            json = new JSONClass();
        }
        return json;
    }
    
    public static JSONClass UserGetEventHistory() {
        string jsonString = CleverTap.Call<string>("userGetEventHistory");
        JSONClass json;
        try {
            json = (JSONClass)JSON.Parse(jsonString);
        } catch {
            Debug.Log("Unable to parse user event history json");
            json = new JSONClass();
        }
        return json;
    }
    
    public static JSONClass SessionGetUTMDetails() {
        string jsonString = CleverTap.Call<string>("sessionGetUTMDetails");
        JSONClass json;
        try {
            json = (JSONClass)JSON.Parse(jsonString);
        } catch {
            Debug.Log("Unable to parse session utm details json");
            json = new JSONClass();
        }
        return json;
    }
    
    public static int SessionGetTimeElapsed() {
        return CleverTap.Call<int>("sessionGetTimeElapsed");
    }
    
    public static int UserGetTotalVisits() {
        return CleverTap.Call<int>("userGetTotalVisits");
    }
    
    public static int UserGetScreenCount() {
        return CleverTap.Call<int>("userGetScreenCount");
    }
    
    public static int UserGetPreviousVisitTime() {
        return CleverTap.Call<int>("userGetPreviousVisitTime");
    }
    
#else

   // Empty implementations of the API, in case the application is being compiled for a platform other than iOS or Android.
    void Start() {
        Debug.Log("Start: no-op CleverTap binding for non iOS/Android.");
    }

    public static void LaunchWithCredentials(string accountID, string token) {
    }
        
    public static void OnUserLogin(Dictionary<string, string> properties) {
    }

    public static void ProfilePush(Dictionary<string, string> properties) {
    }

    public static void ProfilePushGraphUser(Dictionary<string, string> user) {
    }

    public static void ProfilePushGooglePlusUser(Dictionary<string, string> user) {
    }

    public static string ProfileGet(string key) {
        return "test";
    }

    public static string ProfileGetCleverTapAttributionIdentifier() {
        return "testAttributionIdentifier";
    }

    public static string ProfileGetCleverTapID() {
        return "testCleverTapID";
    }

    public static void ProfileRemoveValueForKey(string key) {
    }

    public static void ProfileSetMultiValuesForKey(string key, List<string> values) {
    }

    public static void ProfileAddMultiValuesForKey(string key, List<string> values) {
    }

    public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
    }

    public static void ProfileAddMultiValueForKey(string key, string val) {
    }

    public static void ProfileRemoveMultiValueForKey(string key, string val) {
    }

    public static void RecordEvent(string eventName) {
    }
        
    public static void RecordEvent(string eventName, Dictionary<string, object> properties) {
    }

    public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>>items) {
    }

    public static int EventGetFirstTime(string eventName) {
        return -1;
    }

    public static int EventGetLastTime(string eventName) {
        return -1;
    }

    public static int EventGetOccurrences(string eventName) {
        return -1;
    }

    public static JSONClass EventGetDetail(string eventName) {
        return new JSONClass();
    }

    public static JSONClass UserGetEventHistory() {
        return new JSONClass();
    }

    public static JSONClass SessionGetUTMDetails() {
        return new JSONClass();
    }

    public static int SessionGetTimeElapsed() {
        return -1;
    }

    public static int UserGetTotalVisits() {
        return -1;
    }

    public static int UserGetScreenCount() {
        return -1;
    }

    public static int UserGetPreviousVisitTime() {
        return -1;
    }
        
    public static void EnablePersonalization() {
    }

    public static void DisablePersonalization() {
    }

    public static void RegisterPush() {
    }

    public static void SetApplicationIconBadgeNumber(int num) {
    }
        
    public static void SetDebugLevel(int level) {
    }

    public static void SetLocation(double lat, double lon) {
    }

#endif
  }
}
