using CleverTapSDK;
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class CleverTapExample : MonoBehaviour {
    
    public String CLEVERTAP_ACCOUNT_ID = "YOUR_CLEVERTAP_ACCOUNT_ID";
    public String CLEVERTAP_ACCOUNT_TOKEN = "YOUR_CLEVERTAP_ACCOUNT_TOKEN";
    public int CLEVERTAP_DEBUG_LEVEL = 0;
    public bool CLEVERTAP_ENABLE_PERSONALIZATION = true;

    void Awake() {
        OnInit();
    }

    /* --------------------------------------------------------------------------------
     *                          INITIALISATION
     * -------------------------------------------------------------------------------- */
    // Put all initialization, registeration code here
    void OnInit() {
        DontDestroyOnLoad(gameObject);

        CleverTap.OnCleverTapDeepLinkCallback += CleverTapDeepLinkCallback;
        CleverTap.OnCleverTapProfileInitializedCallback += CleverTapProfileInitializedCallback;
        CleverTap.OnCleverTapProfileUpdatesCallback += CleverTapProfileUpdatesCallback;
        CleverTap.OnCleverTapPushOpenedCallback += CleverTapPushOpenedCallback;
        CleverTap.OnCleverTapInitCleverTapIdCallback += CleverTapInitCleverTapIdCallback;
        CleverTap.OnCleverTapInAppNotificationDismissedCallback += CleverTapInAppNotificationDismissedCallback;
        CleverTap.OnCleverTapInAppNotificationShowCallback += CleverTapInAppNotificationShowCallback;
        CleverTap.OnCleverTapOnPushPermissionResponseCallback += CleverTapOnPushPermissionResponseCallback;
        CleverTap.OnCleverTapInAppNotificationButtonTapped += CleverTapInAppNotificationButtonTapped;
        CleverTap.OnCleverTapInboxDidInitializeCallback += CleverTapInboxDidInitializeCallback;
        CleverTap.OnCleverTapInboxMessagesDidUpdateCallback += CleverTapInboxMessagesDidUpdateCallback;
        CleverTap.OnCleverTapInboxCustomExtrasButtonSelect += CleverTapInboxCustomExtrasButtonSelect;
        CleverTap.OnCleverTapInboxItemClicked += CleverTapInboxItemClicked;
        CleverTap.OnCleverTapNativeDisplayUnitsUpdated += CleverTapNativeDisplayUnitsUpdated;
        CleverTap.OnCleverTapProductConfigFetched += CleverTapProductConfigFetched;
        CleverTap.OnCleverTapProductConfigActivated += CleverTapProductConfigActivated;
        CleverTap.OnCleverTapProductConfigInitialized += CleverTapProductConfigInitialized;
        CleverTap.OnCleverTapFeatureFlagsUpdated += CleverTapFeatureFlagsUpdated;
        CleverTap.OnVariablesChanged += CleverTapVariablesChanged;

        //Disable CleverTap unity logger
        //CleverTap.SetLogLevel(LogLevel.None);

        CleverTap.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);

        CleverTap.LaunchWithCredentials(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN);

        //Add Platform Specific Init Code here
#if (UNITY_IPHONE && !UNITY_EDITOR)
        OniOSInit();
#endif

#if (UNITY_ANDROID && !UNITY_EDITOR)
        OnAndroidInit();
#endif

        // set to true to stop sending events to CleverTap
        CleverTap.SetOptOut(false);
        // set to true to enable Device Network information to be sent to CleverTap
        CleverTap.EnableDeviceNetworkInfoReporting(true);
        if (CLEVERTAP_ENABLE_PERSONALIZATION) {
            CleverTap.EnablePersonalization();
        }

        //app inbox
        CleverTap.InitializeInbox();
        Debug.Log("InboxInit started");

        CleverTap.RecordEvent("Test Unity Event");
        //Invoke("LaunchInbox",30.0f);

        //Push primer APIs usage
        //Half-Interstial Local InApp
        Dictionary<string, object> item = new Dictionary<string, object> {
            { "inAppType", "half-interstitial" },
            { "titleText", "Get Notified" },
            { "messageText", "Please enable notifications on your device to use Push Notifications." },
            { "followDeviceOrientation", true },
            { "positiveBtnText", "Allow" },
            { "negativeBtnText", "Cancel" },
            { "backgroundColor", "#FFFFFF" },
            { "btnBorderColor", "#0000FF" },
            { "titleTextColor", "#0000FF" },
            { "messageTextColor", "#000000" },
            { "btnTextColor", "#FFFFFF" },
            { "btnBackgroundColor", "#0000FF" },
            { "imageUrl", "https://icons.iconarchive.com/icons/treetog/junior/64/camera-icon.png" },
            { "btnBorderRadius", "2" },
            { "fallbackToSettings", true }
        };
        CleverTap.PromptPushPrimer(item);

        //Alert Local InApp
        //Dictionary<string, object> item2 = new Dictionary<string, object>() {
        //    { "inAppType", "half-interstitial" },
        //    {"titleText", "Get Notified" },
        //    {"messageText", "Please enable notifications on your device to use Push Notifications." },
        //    {"followDeviceOrientation", true },
        //    { "fallbackToSettings", true }
        //};
        //CleverTap.PromptPushPrimer(item2);

        /*Prompt to show hard notification permission dialog
          true - fallbacks to app's notification settings if permission is denied
          false - does not fallback to app's notification settings if permission is denied
        */
        CleverTap.PromptForPushPermission(false);
    }

    /* --------------------------------------------------------------------------------
     *                          INITIALISATION IOS SPECIFIC
     * -------------------------------------------------------------------------------- */

    //Add iOS Platform Specific Init Code here
    void OniOSInit() {
        // register for push notifications
        CleverTap.RegisterPush();
        // set to 0 to remove icon badge
        CleverTap.SetApplicationIconBadgeNumber(0);
        //Will check whether notification permission is granted or not
        CleverTap.IsPushPermissionGranted();
    }

    /* --------------------------------------------------------------------------------
     *                          INITIALISATION ANDROID SPECIFIC
     * -------------------------------------------------------------------------------- */
    //Add Android Platform Specific Init Code here
    void OnAndroidInit() {
        //For Android 13+ do ensure to grant notification permission to receive push notifications.
        CleverTap.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);

        //CleverTap.CreateNotificationChannelWithSound("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true, "Your raw sound file");
        //CleverTap.CreateNotificationChannelGroup("YourGroupId", "Your Group Name");
        //CleverTap.CreateNotificationChannelWithGroup("YourChannelId", "Your Channel Name", "Your Channel Description", 5, "YourGroupId", true);
        //CleverTap.CreateNotificationChannelWithGroupAndSound("YourChannelId", "Your Channel Name", "Your Channel Description", 5, "YourGroupId", true, "Your raw sound file");
        //CleverTap.DeleteNotificationChannel("YourChannelId");
        //CleverTap.DeleteNotificationChannelGroup("YourGroupId");

        //CleverTap.GetCleverTapID();
        //CleverTap.ProfileIncrementValueForKey("add_int", 2);
        //CleverTap.ProfileIncrementValueForKey("add_double", 3.5);
        //CleverTap.ProfileDecrementValueForKey("minus_int", 2);
        //CleverTap.ProfileDecrementValueForKey("minus_double", 3.5);
        //CleverTap.SuspendInAppNotifications();
        //CleverTap.DiscardInAppNotifications();
        //CleverTap.ResumeInAppNotifications();

        //Returns a boolean to indicate whether notification permission is granted or not
        bool isPushPermissionGranted = CleverTap.IsPushPermissionGranted();
        Debug.Log("isPushPermissionGranted" + isPushPermissionGranted);
    }

    void Start() {
        OnStartCommon();

#if (UNITY_IPHONE && !UNITY_EDITOR)
       OniOSStart();
#endif

#if (UNITY_ANDROID && !UNITY_EDITOR)
       OnAndroidStart();
#endif

    }

    // CleverTap API usage examples
    // Just for illustration here in Start
    /* --------------------------------------------------------------------------------
     *                          CLEVERTAP API USAGE
     * -------------------------------------------------------------------------------- */

    //Add common feature codes here
    void OnStartCommon() {
        // record special Charged event
        Dictionary<string, object> chargeDetails = new Dictionary<string, object> {
            { "Amount", 500 },
            { "Currency", "USD" },
            { "Payment Mode", "Credit card" }
        };

        Dictionary<string, object> item = new Dictionary<string, object> {
            { "price", 50 },
            { "Product category", "books" },
            { "Quantity", 1 }
        };

        Dictionary<string, object> item2 = new Dictionary<string, object> {
            { "price", 100 },
            { "Product category", "plants" },
            { "Quantity", 10 }
        };

        List<Dictionary<string, object>> items = new List<Dictionary<string, object>> { item, item2 };

        CleverTap.RecordChargedEventWithDetailsAndItems(chargeDetails, items);

        // record basic event with no properties
        CleverTap.RecordEvent("testEvent");

        // record event with properties
        Dictionary<string, object> props = new Dictionary<string, object> { { "testKey", "testValue" } };
        CleverTap.RecordEvent("testEventWithProps", props);

        // set user location
        CleverTap.SetLocation(34.147785, -118.144516);

        // reset the user profile after a login with a new Identity
        Dictionary<string, string> newProps = new Dictionary<string, string> {
            { "email", "test@test.com" },
            { "Identity", "123456" }
        };
        CleverTap.OnUserLogin(newProps);


        // get the CleverTap unique install attributionidentifier
        string CleverTapAttributionIdentifier = CleverTap.ProfileGetCleverTapAttributionIdentifier();
        Debug.Log("CleverTapAttributionIdentifier is: " + (!String.IsNullOrEmpty(CleverTapAttributionIdentifier) ? CleverTapAttributionIdentifier : "NULL"));

        // get the CleverTap unique profile identifier
        string CleverTapID = CleverTap.ProfileGetCleverTapID();
        Debug.Log("CleverTapID is: " + (!String.IsNullOrEmpty(CleverTapID) ? CleverTapID : "NULL"));

        // get event and session data
        int firstTime = CleverTap.EventGetFirstTime("App Launched");
        int lastTime = CleverTap.EventGetLastTime("App Launched");
        int occurrences = CleverTap.EventGetOccurrences("App Launched");
        Debug.Log(String.Format("App Launched first time is {0} last time is {1} occurrences is {2}",
            firstTime, lastTime, occurrences));

        JSONClass history = CleverTap.UserGetEventHistory();
        Debug.Log(String.Format("User event history is {0}", history));

        int elapsedTime = CleverTap.SessionGetTimeElapsed();
        int totalVisits = CleverTap.UserGetTotalVisits();
        int screenCount = CleverTap.UserGetScreenCount();
        int previousVisitTime = CleverTap.UserGetPreviousVisitTime();
        Debug.Log(String.Format("session stats: elapsed time: {0}, total visits: {1}, screen count: {2}, previous visit time: {3}",
            elapsedTime, totalVisits, screenCount, previousVisitTime));

        // get session referrer utm values
        JSONClass utmDetail = CleverTap.SessionGetUTMDetails();
        Debug.Log(String.Format("session utm details is {0}", utmDetail));

        // get event data for a specific event
        JSONClass eventDetail = CleverTap.EventGetDetail("App Launchedd");
        Debug.Log(String.Format("event detail is {0}", eventDetail));

        // get user profile attributes
        // scalar value
        string profileName = CleverTap.ProfileGet("Name");
        Debug.Log("profileName is: " + (!String.IsNullOrEmpty(profileName) ? profileName : "NULL"));

        // multi-value (array)
        string multiValueProperty = CleverTap.ProfileGet("multiAndroid");
        Debug.Log("multiValueProperty is: " + (!String.IsNullOrEmpty(multiValueProperty) ? multiValueProperty : "NULL"));
        if (!String.IsNullOrEmpty(multiValueProperty)) {
            try {
                JSONArray values = (JSONArray)JSON.Parse(multiValueProperty);
            } catch {
                Debug.Log("unable to parse json");
            }
        }

        //Delete multiple inbox ids
        string[] arr = { "1608798445_1676289520", "1548315289_1676289520", "1608798445_1676289268", "1548315289_1676289268" };
        CleverTap.DeleteInboxMessagesForIDs(arr);
        //Marks read for multiple inbox ids
        CleverTap.MarkReadInboxMessagesForIDs(arr);

        //Delete a single inbox id
        string inboxId = "1608798445_1676289520";
        CleverTap.DeleteInboxMessageForID(inboxId);
        //Marks read for a single inbox id
        CleverTap.MarkReadInboxMessageForID(inboxId);
    }

    /* --------------------------------------------------------------------------------
     *                          CLEVERTAP API USAGE IOS SPECIFIC
     * -------------------------------------------------------------------------------- */
    // Add code for iOS only features here
    void OniOSStart() {
        //CleverTap.RecordScreenView("TestScreen");

        // set a scalar user profile property
        Dictionary<string, string> props = new Dictionary<string, string> { { "RegistrationSource", "iOS" } };
        CleverTap.ProfilePush(props);

        // remove a user profile property
        CleverTap.ProfileRemoveValueForKey("foo");

        // set, add, remove user multi-value (array<string>) property
        List<string> stringList = new List<string> {
            "one",
            "two"
        };
        CleverTap.ProfileSetMultiValuesForKey("multiIOS", stringList);

        List<string> stringList1 = new List<string> {
            "three",
            "four"
        };
        CleverTap.ProfileAddMultiValuesForKey("multiIOS", stringList1);

        List<string> stringList2 = new List<string> { "two" };
        CleverTap.ProfileRemoveMultiValuesForKey("multiIOS", stringList2);

        CleverTap.ProfileAddMultiValueForKey("multiIOS", "five");
        CleverTap.ProfileRemoveMultiValueForKey("multiIOS", "four");

        // Define Varible
        var iosDoubleVariable = CleverTap.Define("iOS_double_variable", 2.5);
        iosDoubleVariable.OnValueChanged += () => {
            Debug.Log("unity recived variable value chaged for iOS_double_variable");
        };

        // Variables

        var iosIntVariable = CleverTap.Define("iOS_int_variable", 10);
        Debug.Log("iOS_int_variable { Name: " + iosIntVariable.Name + ", Kind: " + iosIntVariable.Kind + ", Value: " + iosIntVariable.Value + ", DefaultValue: " + iosIntVariable.DefaultValue + "}");

        // Get Variable
        var iosIntVariableRef = CleverTap.GetVariable<int>("iOS_int_variable");
        Debug.Log("iosIntVariableRef { Name: " + iosIntVariableRef.Name + ", Kind: " + iosIntVariableRef.Kind + ", Value: " + iosIntVariableRef.Value + ", DefaultValue: " + iosIntVariableRef.DefaultValue + "}");

        // Fetch variables from server
        CleverTap.FetchVariables(VariableFetchedCallback);

        // Sync variable on server
        //CleverTap.SyncVariables();
    }

    /* --------------------------------------------------------------------------------
     *                          CLEVERTAP API USAGE ANDROID SPECIFIC
     * -------------------------------------------------------------------------------- */
    // Add code for android only features here
    void OnAndroidStart() {
        // set a scalar user profile property
        Dictionary<string, string> props = new Dictionary<string, string> { { "RegistrationSource", "Android" } };
        CleverTap.ProfilePush(props);

        // remove a user profile property
        CleverTap.ProfileRemoveValueForKey("foo");

        // set, add, remove user multi-value (array<string>) property
        List<string> stringList = new List<string> {
            "one",
            "two"
        };
        CleverTap.ProfileSetMultiValuesForKey("multiAndroid", stringList);

        List<string> stringList1 = new List<string> {
            "three",
            "four"
        };
        CleverTap.ProfileAddMultiValuesForKey("multiAndroid", stringList1);

        List<string> stringList2 = new List<string> { "two" };
        CleverTap.ProfileRemoveMultiValuesForKey("multiAndroid", stringList2);

        CleverTap.ProfileAddMultiValueForKey("multiAndroid", "five");
        CleverTap.ProfileRemoveMultiValueForKey("multiAndroid", "four");
        CleverTap.SetOptOut(false);
        CleverTap.EnableDeviceNetworkInfoReporting(true);

        // Variables

        var androidDoubleVariable = CleverTap.Define("android_double_variable", 2.5);
        androidDoubleVariable.OnValueChanged += () => {
            Debug.Log("unity recived variable value chaged for android_double_variable");
        };

        var androidIntVariable = CleverTap.Define("android_int_variable", 10);
        Debug.Log("android_int_variable { Name: " + androidIntVariable.Name + ", Kind: " + androidIntVariable.Kind + ", Value: " + androidIntVariable.Value + ", DefaultValue: " + androidIntVariable.DefaultValue + "}");

        // Get Variable
        var androidIntVariableRef = CleverTap.GetVariable<int>("android_int_variable");
        Debug.Log("androidIntVariableRef { Name: " + androidIntVariableRef.Name + ", Kind: " + androidIntVariableRef.Kind + ", Value: " + androidIntVariableRef.Value + ", DefaultValue: " + androidIntVariableRef.DefaultValue + "}");

        // Fetch variables from server
        CleverTap.FetchVariables(VariableFetchedCallback);

        // Sync variable on server
        //CleverTap.SyncVariables();
    }

    /* --------------------------------------------------------------------------------
     *                          CLEVERTAP API CALLBACKS
     * -------------------------------------------------------------------------------- */

    // handle deep link url
    void CleverTapDeepLinkCallback(string url) {
        Debug.Log("unity received deep link: " + (!String.IsNullOrEmpty(url) ? url : "NULL"));
    }

    // called when then the CleverTap user profile is initialized
    // returns {"CleverTapID":<CleverTap unique user id>}
    void CleverTapProfileInitializedCallback(string message) {
        Debug.Log("unity received profile initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("unity parsed profile initialized {0}", json));
        } catch {
            Debug.Log("unable to parse json");
        }
    }

    // called when the user profile is updated as a result of a server sync
    /**
        returns dict in the form:
        {
            "profile":{"<property1>":{"oldValue":<value>, "newValue":<value>}, ...},
            "events:{"<eventName>":
                        {"count":
                            {"oldValue":(int)<old count>, "newValue":<new count>},
                        "firstTime":
                            {"oldValue":(double)<old first time event occurred>, "newValue":<new first time event occurred>},
                        "lastTime":
                            {"oldValue":(double)<old last time event occurred>, "newValue":<new last time event occurred>},
                    }, ...
                }
        }
    */
    void CleverTapProfileUpdatesCallback(string message) {
        Debug.Log("unity received profile updates: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("unity parsed profile updates {0}", json));
        } catch {
            Debug.Log("unable to parse json");
        }
    }

    // returns the data associated with the push notification
    void CleverTapPushOpenedCallback(string message) {
        Debug.Log("unity received push opened: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("push notification data is {0}", json));
        } catch {
            Debug.Log("unable to parse json");
        }
    }

    // returns a unique CleverTap identifier suitable for use with install attribution providers.
    void CleverTapInitCleverTapIdCallback(string message) {
        Debug.Log("unity received clevertap id: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns the custom data associated with an in-app notification click
    void CleverTapInAppNotificationDismissedCallback(string message) {
        Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns the custom data associated with an in-app notification click
    void CleverTapInAppNotificationShowCallback(string message) {
        Debug.Log("unity received inapp notification onShow(): " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns the status of push permission response after it's granted/denied
    void CleverTapOnPushPermissionResponseCallback(string message) {
        Debug.Log("unity received push permission response: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns when an in-app notification is dismissed by a call to action with custom extras
    void CleverTapInAppNotificationButtonTapped(string message) {
        Debug.Log("unity received inapp notification button tapped: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    //returns callback for InitializeInbox
    void CleverTapInboxDidInitializeCallback() {
        //CleverTap.ShowAppInbox(new Dictionary<string, object>());
        Debug.Log("unity received inbox initialized");
    }

    void CleverTapInboxMessagesDidUpdateCallback() {
        Debug.Log("unity received inbox messages updated");
    }

    // returns on the click of app inbox message with a map of custom Key-Value pairs
    void CleverTapInboxCustomExtrasButtonSelect(string message) {
        Debug.Log("unity received inbox message button selected callback: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    //returns data associated with inbox message item click along with page index and button index
    void CleverTapInboxItemClicked(string message) {
        Debug.Log("unity received inbox message clicked callback: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns native display units data
    void CleverTapNativeDisplayUnitsUpdated(string message) {
        Debug.Log("unity received native display units updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are fetched
    void CleverTapProductConfigFetched(string message) {
        Debug.Log("unity received product config fetched: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are activated
    void CleverTapProductConfigActivated(string message) {
        Debug.Log("unity received product config activated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are initialized
    void CleverTapProductConfigInitialized(string message) {
        Debug.Log("unity received product config initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Feature Flags are updated 
    void CleverTapFeatureFlagsUpdated(string message) {
        Debug.Log("unity received feature flags updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when any variable changed
    void CleverTapVariablesChanged() {
        Debug.Log("Unity received variables changed");
    }

    void VariableFetchedCallback(bool isSuccess) {
        Debug.Log("unity received fetched variables is success: " + isSuccess);
    }

    void LaunchInbox() {
        CleverTap.ShowAppInbox(new Dictionary<string, object>());
    }
}