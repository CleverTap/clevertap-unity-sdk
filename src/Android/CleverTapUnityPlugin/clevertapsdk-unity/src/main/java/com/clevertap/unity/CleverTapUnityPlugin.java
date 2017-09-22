package com.clevertap.unity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.util.Log;

import com.clevertap.android.sdk.ActivityLifecycleCallback;
import com.clevertap.android.sdk.CleverTapAPI;
import com.clevertap.android.sdk.EventDetail;
import com.clevertap.android.sdk.InAppNotificationListener;
import com.clevertap.android.sdk.SyncListener;
import com.clevertap.android.sdk.UTMDetail;

import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONObject;
import org.json.JSONException;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

public class CleverTapUnityPlugin implements SyncListener, InAppNotificationListener {

    private static final String LOG_TAG = "CleverTapUnityPlugin";

    private static final String CLEVERTAP_GAME_OBJECT_NAME = "CleverTapUnity";
    private static final String CLEVERTAP_PROFILE_INITIALIZED_CALLBACK = "CleverTapProfileInitializedCallback";
    private static final String CLEVERTAP_PROFILE_UPDATES_CALLBACK = "CleverTapProfileUpdatesCallback";
    private static final String CLEVERTAP_DEEP_LINK_CALLBACK = "CleverTapDeepLinkCallback";
    private static final String CLEVERTAP_PUSH_OPENED_CALLBACK = "CleverTapPushOpenedCallback";
    private static final String CLEVERTAP_INAPP_NOTIFICATION_DISMISSED_CALLBACK = "CleverTapInAppNotificationDismissedCallback";

    private static CleverTapUnityPlugin instance = null;

    private static CleverTapAPI clevertap = null;

    private static void changeCredentials(final String accountID, final String accountToken, final String region) {
        CleverTapAPI.changeCredentials(accountID, accountToken, region);
    }

    static void handleIntent(Intent intent) {
        if (intent == null) return;

        if (intent.getAction().equals(Intent.ACTION_VIEW)) {
            Uri data = intent.getData();
            if (data != null) {
                handleDeepLink(data);
            }
        }
        else {
            Bundle extras = intent.getExtras();
            Boolean isPushNotification = (extras != null && extras.get("wzrk_pn") != null);
            if (isPushNotification) {
                JSONObject data = new JSONObject();

                for (String key : extras.keySet()) {
                    try {
                        Object value = extras.get(key);
                        if (value instanceof Map) {
                            JSONObject jsonObject = new JSONObject((Map) value);
                            data.put(key, jsonObject);
                        } else if (value instanceof List) {
                            JSONArray jsonArray = new JSONArray((List) value);
                            data.put(key, jsonArray);
                        } else {
                            data.put(key, extras.get(key));
                        }

                    } catch(JSONException e) {
                        // no-op
                    }
                }
                handlePushNotification(data);
            }
        }
    }

    static private void handlePushNotification(final JSONObject data) {
        final String json = data.toString();
        messageUnity(CLEVERTAP_GAME_OBJECT_NAME, CLEVERTAP_PUSH_OPENED_CALLBACK, json);
    }

    static private void handleDeepLink(final Uri data) {
        final String json = data.toString();
        messageUnity(CLEVERTAP_GAME_OBJECT_NAME, CLEVERTAP_DEEP_LINK_CALLBACK, json);
    }

    public static void initialize(final String accountID, final String accountToken, final Activity activity) {
        initialize(accountID, accountToken, null, activity);
    }

    public static void initialize(final String accountID, final String accountToken, final String region, final Activity activity) {
        try {
            changeCredentials(accountID, accountToken, region);
            ActivityLifecycleCallback.register(activity.getApplication());
            CleverTapAPI.setAppForeground(true);
            getInstance(activity.getApplicationContext());
            clevertap.activityResumed(activity);
        } catch (Throwable t) {
            Log.e(LOG_TAG, "initialize error", t);
        }
    }

    public static void setDebugLevel(int level) {
        CleverTapAPI.setDebugLevel(level);
    }

    public static synchronized CleverTapUnityPlugin getInstance(final Context context) {

        if (instance == null && context != null) {
            instance = new CleverTapUnityPlugin(context.getApplicationContext());
        }
        return instance;
    }

    private CleverTapUnityPlugin(final Context context) {
        try {
            clevertap = CleverTapAPI.getInstance(context);
            clevertap.setInAppNotificationListener(this);
            clevertap.setSyncListener(this);
        } catch (Throwable t) {
           Log.e(LOG_TAG, "initialization error", t);
        }
    }

    public void enablePersonalization() {
        try {
            clevertap.enablePersonalization();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "enablePersonalization error", t);
        }
    }

    public void disablePersonalization() {
        try {
            clevertap.disablePersonalization();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "disablePersonalization error", t);
        }
    }

    public void setLocation(final double lat, final double lon) {
        try {
            final Location location = new Location("CleverTapUnityPlugin");
            location.setLatitude(lat);
            location.setLongitude(lon);
            clevertap.setLocation(location);
        } catch (Throwable t) {
            Log.e(LOG_TAG, "setLocation error", t);
        }
    }

    public void onUserLogin(final String jsonString) {
        try {
            Map<String, Object> profile = toMap(new JSONObject(jsonString));
            clevertap.onUserLogin(profile);
        } catch (Throwable t) {
            Log.e(LOG_TAG, "onUserLogin error", t);
        }
    }

    public void profilePush(final String jsonString) {
        try {
            Map<String, Object> profile = toMap(new JSONObject(jsonString));
            clevertap.profile.push(profile);
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profilePush error", t);
        }
    }

    public void profilePushFacebookUser(final String jsonString) {
        try {
            clevertap.profile.pushFacebookUser(new JSONObject(jsonString));
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profilePushFacebookUser error", t);
        }
    }

    public String profileGet(final String key) {
        try {
            String val = null;
            Object _val = clevertap.profile.getProperty(key);
            if (_val != null) {
                val = _val.toString();
            }
            return val;

        } catch (Throwable t) {
            Log.e(LOG_TAG, "profileGet error", t);
            return null;
        }
    }

    public String profileGetCleverTapAttributionIdentifier() {
        try {
            return clevertap.getCleverTapAttributionIdentifier();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profileGetCleverTapAttributionIdentifier error", t);
            return null;
        }
    }

    public String profileGetCleverTapID() {
        try {
            return clevertap.getCleverTapID();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profileGetCleverTapID error", t);
            return null;
        }
    }

    public void profileRemoveValueForKey(final String key) {
        clevertap.profile.removeValueForKey(key);
    }

    public void profileAddMultiValueForKey(final String key, final String val) {
        clevertap.profile.addMultiValueForKey(key, val);
    }

    public void profileRemoveMultiValueForKey(final String key, final String val) {
        clevertap.profile.removeMultiValueForKey(key, val);
    }

    public void profileSetMultiValuesForKey(final String key, final String[] values) {
        try {
            clevertap.profile.setMultiValuesForKey(key, new ArrayList<String>(Arrays.asList(values)));
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profileSetMultiValuesForKey error", t);
        }
    }

    public void profileAddMultiValuesForKey(final String key, final String[] values) {
        try {
            clevertap.profile.addMultiValuesForKey(key, new ArrayList<String>(Arrays.asList(values)));
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profileAddMultiValuesForKey error", t);
        }
    }

    public void profileRemoveMultiValuesForKey(final String key, final String[] values) {
        try {
            clevertap.profile.removeMultiValuesForKey(key, new ArrayList<String>(Arrays.asList(values)));
        } catch (Throwable t) {
            Log.e(LOG_TAG, "profileRemoveMultiValuesForKey error", t);
        }
    }

    public void recordScreenView(final String screenName) {
        Log.i(LOG_TAG, "recordScreenView is not supported for Android");
    }

    public void recordEvent(final String eventName, final String propertiesJsonString) {

        if (eventName == null) return;

        if (propertiesJsonString == null) {
            clevertap.event.push(eventName);
        } else {
            try {
                JSONObject _props = new JSONObject(propertiesJsonString);
                Map<String, Object> props = toMap(_props);
                clevertap.event.push(eventName, props);
            } catch (Throwable t) {
                Log.e(LOG_TAG, "recordEvent error", t);
            }
        }
    }

    public void recordChargedEventWithDetailsAndItems(final String detailsJSON, final String itemsJSON) {

        try {
            JSONObject details = new JSONObject(detailsJSON);
            JSONArray items = new JSONArray(itemsJSON);
            clevertap.event.push("Charged", toMap(details), toArrayListOfStringObjectMaps(items));
        } catch (Throwable t) {
            Log.e(LOG_TAG, "recordChargedEventWithDetailsAndItems error", t);
        }
    }

    public int eventGetFirstTime(final String event) {
        return clevertap.event.getFirstTime(event);
    }

    public int eventGetLastTime(final String event) {
        return clevertap.event.getLastTime(event);
    }

    public int eventGetOccurrences(final String event) {
        return clevertap.event.getCount(event);
    }

    public String eventGetDetail(final String event) {
        try {
            EventDetail details = clevertap.event.getDetails(event);
            return eventDetailsToJSON(details).toString();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "eventGetDetail error", t);
            return null;
        }
    }

    public String userGetEventHistory() {
        try {
            Map<String, EventDetail> history = clevertap.event.getHistory();
            return eventHistoryToJSON(history).toString();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "userGetEventHistory error", t);
            return null;
        }
    }

    public String sessionGetUTMDetails() {
        try {
            UTMDetail details = clevertap.session.getUTMDetails();
            return utmDetailsToJSON(details).toString();
        } catch (Throwable t) {
            Log.e(LOG_TAG, "sessionGetUTMDetails error", t);
            return null;
        }
    }

    public int sessionGetTimeElapsed() {
        return clevertap.session.getTimeElapsed();
    }

    public int userGetTotalVisits() {
        return clevertap.session.getTotalVisits();
    }

    public int userGetScreenCount() {
        return clevertap.session.getScreenCount();
    }

    public int userGetPreviousVisitTime() {
        return clevertap.session.getPreviousVisitTime();
    }

    // InAppNotificationListener
    public boolean beforeShow(Map<String, Object> var1) {
        return true;
    }

    public void onDismissed(Map<String, Object> var1, @Nullable Map<String, Object> var2) {
        if(var1 == null && var2 == null) {
            return ;
        }

        JSONObject extras = var1 != null ? new JSONObject(var1) : new JSONObject();
        String _json = "{extras:"+extras.toString()+",";

        JSONObject actionExtras = var2 != null ? new JSONObject(var2) : new JSONObject();
        _json += "actionExtras:"+actionExtras.toString()+"}";

        final String json = _json;
        messageUnity(CLEVERTAP_GAME_OBJECT_NAME, CLEVERTAP_INAPP_NOTIFICATION_DISMISSED_CALLBACK, json);
    }

    // SyncListener
    public void profileDataUpdated(JSONObject updates) {

        if(updates == null) {
            return ;
        }

        final String json = "{updates:"+updates.toString()+"}";
        messageUnity(CLEVERTAP_GAME_OBJECT_NAME, CLEVERTAP_PROFILE_UPDATES_CALLBACK, json);
    }

    public void profileDidInitialize (String CleverTapID) {

        if (CleverTapID == null) {
            return;
        }

        final String json = "{CleverTapID:"+ CleverTapID +"}";
        messageUnity(CLEVERTAP_GAME_OBJECT_NAME, CLEVERTAP_PROFILE_INITIALIZED_CALLBACK, json);
    }

    /*******************
     * Helpers
     ******************/

    private static void messageUnity(final String gameObject, final String method, final String data) {
        UnityPlayer.UnitySendMessage(gameObject, method, data);
    }

    private static Object fromJson(Object json) throws JSONException {
        if (json == JSONObject.NULL) {
            return null;
        } else if (json instanceof JSONObject) {
            return toMap((JSONObject) json);
        } else {
            return json;
        }
    }

    private static HashMap<String, Object> toMap(JSONObject object) throws JSONException {
        HashMap<String, Object> map = new HashMap<String, Object>();
        Iterator keys = object.keys();
        while (keys.hasNext()) {
            String key = (String) keys.next();
            map.put(key, fromJson(object.get(key)));
        }
        return map;
    }

    private static ArrayList<HashMap<String, Object>> toArrayListOfStringObjectMaps(JSONArray array) throws JSONException {
        ArrayList<HashMap<String, Object>> aList = new ArrayList<HashMap<String, Object>>();

        for (int i = 0; i < array.length(); i++) {
            aList.add(toMap((JSONObject) array.get(i)));
        }

        return aList;
    }

    private static JSONObject eventDetailsToJSON(EventDetail details) throws JSONException {

        JSONObject json = new JSONObject();

        if(details != null) {
            json.put("name", details.getName());
            json.put("firstTime", details.getFirstTime());
            json.put("lastTime", details.getLastTime());
            json.put("count", details.getCount());
        }

        return json;
    }

    private static JSONObject utmDetailsToJSON(UTMDetail details) throws JSONException {

        JSONObject json = new JSONObject();

        if(details != null) {
            json.put("campaign", details.getCampaign());
            json.put("source", details.getSource());
            json.put("medium", details.getMedium());
        }

        return json;
    }

    private static JSONObject eventHistoryToJSON( Map<String, EventDetail> history) throws JSONException {

        JSONObject json = new JSONObject();

        if(history != null) {
            for (Object key : history.keySet()) {
                json.put(key.toString(), eventDetailsToJSON(history.get((String)key)));
            }
        }

        return json;
    }
}