using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using CleverTap;
using CleverTap.Utilities;

public class CleverTapUnity: MonoBehaviour {

    public String CLEVERTAP_ACCOUNT_ID = "Your CleverTap Account ID";
    public String CLEVERTAP_ACCOUNT_TOKEN = "Your CleverTap Account Token";
    public int CLEVERTAP_DEBUG_LEVEL = 0;
    public bool CLEVERTAP_ENABLE_PERSONALIZATION = true;

    void Awake(){
        #if (UNITY_IPHONE && !UNITY_EDITOR)
        DontDestroyOnLoad(gameObject);
        #endif

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        DontDestroyOnLoad(gameObject);
        CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
        CleverTapBinding.Initialize(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN);
        if (CLEVERTAP_ENABLE_PERSONALIZATION) {
            CleverTapBinding.EnablePersonalization();
        }
        #endif
    }
    
    // CleverTap API usage examples
    // Just for illustration here in Start 
    void Start() {
        #if (UNITY_IPHONE && !UNITY_EDITOR)
        // register for push notifications
        CleverTap.CleverTapBinding.RegisterPush();
        // set to 0 to remove icon badge
        CleverTap.CleverTapBinding.SetApplicationIconBadgeNumber(0);
        #endif

        // record special Charged event
        Dictionary<string, object> chargeDetails = new Dictionary<string, object> ();
        chargeDetails.Add("Amount", 500);
        chargeDetails.Add("Currency", "USD");
        chargeDetails.Add("Payment Mode", "Credit card");

        Dictionary<string, object> item = new Dictionary<string, object> ();
        item.Add("price", 50);
        item.Add("Product category", "books");
        item.Add("Quantity", 1);

        Dictionary<string, object> item2 = new Dictionary<string, object> ();
        item2.Add("price", 100);
        item2.Add("Product category", "plants");
        item2.Add("Quantity", 10);

        List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
        items.Add(item);
        items.Add(item2);

        CleverTapBinding.RecordChargedEventWithDetailsAndItems(chargeDetails, items);

        // record basic event with no properties
        CleverTapBinding.RecordEvent("testEvent");

        // record event with properties
        Dictionary<string, object> Props = new Dictionary<string, object> ();
        Props.Add("testKey", "testValue");
        CleverTapBinding.RecordEvent("testEventWithProps", Props);

        // set user location
        CleverTapBinding.SetLocation(34.147785, -118.144516);

        // reset the user profile after a login with a new Identity
        Dictionary<string, string> newProps = new Dictionary<string, string> ();
        newProps.Add("Identity", "123456");
        CleverTapBinding.OnUserLogin(newProps);
        
        // set a scalar user profile property
        Dictionary<string, string> props = new Dictionary<string, string> ();

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        props.Add("RegistrationSource", "Android");
        #endif

        #if (UNITY_IPHONE && !UNITY_EDITOR)
        props.Add("RegistrationSource", "iOS");
        #endif

        CleverTapBinding.ProfilePush(props);

        // remove a user profile property
        CleverTapBinding.ProfileRemoveValueForKey("foo");

        // set, add, remove user multi-value (array<string>) property
        List<string> stringList = new List<string>();
        stringList.Add("one");
        stringList.Add("two");

        #if (UNITY_IPHONE && !UNITY_EDITOR)
        CleverTapBinding.ProfileSetMultiValuesForKey("multiIOS", stringList);
        #endif

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        CleverTapBinding.ProfileSetMultiValuesForKey("multiAndroid", stringList);
        #endif

        List<string> stringList1 = new List<string>();
        stringList1.Add("three");
        stringList1.Add("four");

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        CleverTapBinding.ProfileAddMultiValuesForKey("multiAndroid", stringList1);
        #endif

        #if (UNITY_IPHONE && !UNITY_EDITOR)
        CleverTapBinding.ProfileAddMultiValuesForKey("multiIOS", stringList1);
        #endif

        List<string> stringList2 = new List<string>();
        stringList2.Add("two");

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        CleverTapBinding.ProfileRemoveMultiValuesForKey("multiAndroid", stringList2);
        CleverTapBinding.ProfileAddMultiValueForKey("multiAndroid", "five");
        CleverTapBinding.ProfileRemoveMultiValueForKey("multiAndroid", "four");
        #endif

        #if (UNITY_IPHONE && !UNITY_EDITOR)
        CleverTapBinding.ProfileRemoveMultiValuesForKey("multiIOS", stringList2);
        CleverTapBinding.ProfileAddMultiValueForKey("multiIOS", "five");
        CleverTapBinding.ProfileRemoveMultiValueForKey("multiIOS", "four");
        #endif

        // get the CleverTap unique install attributionidentifier
        string CleverTapAttributionIdentifier = CleverTapBinding.ProfileGetCleverTapAttributionIdentifier();
        Debug.Log("CleverTapAttributionIdentifier is: " + (!String.IsNullOrEmpty(CleverTapAttributionIdentifier) ? CleverTapAttributionIdentifier : "NULL") );

        // get the CleverTap unique profile identifier
        string CleverTapID = CleverTapBinding.ProfileGetCleverTapID();
        Debug.Log("CleverTapID is: " + (!String.IsNullOrEmpty(CleverTapID) ? CleverTapID : "NULL") );

        // get event and session data
        int firstTime = CleverTapBinding.EventGetFirstTime("App Launched");
        int lastTime = CleverTapBinding.EventGetLastTime("App Launched");
        int occurrences = CleverTapBinding.EventGetOccurrences("App Launched");
        Debug.Log(String.Format("App Launched first time is {0} last time is {1} occurrences is {2}", 
            firstTime, lastTime, occurrences));

        JSONClass history = CleverTapBinding.UserGetEventHistory();
        Debug.Log(String.Format("User event history is {0}", history));

        int elapsedTime = CleverTapBinding.SessionGetTimeElapsed();
        int totalVisits = CleverTapBinding.UserGetTotalVisits();
        int screenCount = CleverTapBinding.UserGetScreenCount();
        int previousVisitTime = CleverTapBinding.UserGetPreviousVisitTime();
        Debug.Log(String.Format("session stats: elapsed time: {0}, total visits: {1}, screen count: {2}, previous visit time: {3}", 
            elapsedTime, totalVisits, screenCount, previousVisitTime));

        // get session referrer utm values
        JSONClass utmDetail = CleverTapBinding.SessionGetUTMDetails();
        Debug.Log(String.Format("session utm details is {0}", utmDetail));

        // get event data for a specific event
        JSONClass eventDetail = CleverTapBinding.EventGetDetail("App Launchedd");
        Debug.Log(String.Format("event detail is {0}", eventDetail));

        // get user profile attributes
        // scalar value
        string profileName = CleverTapBinding.ProfileGet("Name");
        Debug.Log("profileName is: " + (!String.IsNullOrEmpty(profileName) ? profileName : "NULL") );

        // multi-value (array)
        string multiValueProperty = CleverTapBinding.ProfileGet("multiAndroid");
        Debug.Log("multiValueProperty is: " + (!String.IsNullOrEmpty(multiValueProperty) ? multiValueProperty : "NULL") );
        if (!String.IsNullOrEmpty(multiValueProperty)) {
            try {
                JSONArray values = (JSONArray)JSON.Parse(multiValueProperty);
            } catch {
                Debug.Log("unable to parse json");
            }
        }
	}

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

    // returns the custom data associated with an in-app notification click
    void CleverTapInAppNotificationDismissedCallback(string message) {
        Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }
}
