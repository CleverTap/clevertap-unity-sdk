package com.clevertap.unity;

import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_DEEP_LINK_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_DISPLAY_UNITS_UPDATED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_FEATURE_FLAG_UPDATED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_FILE_VARIABLE_READY;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_INAPPS_FETCHED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_INAPP_NOTIFICATION_DISMISSED_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_INAPP_NOTIFICATION_SHOW_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_INBOX_DID_INITIALIZE;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_INBOX_MESSAGES_DID_UPDATE;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_INIT_CLEVERTAP_ID_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_ON_INAPP_BUTTON_CLICKED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_ON_INBOX_BUTTON_CLICKED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_ON_INBOX_ITEM_CLICKED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_ON_PUSH_PERMISSION_RESPONSE_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_PRODUCT_CONFIG_ACTIVATED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_PRODUCT_CONFIG_FETCHED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_PRODUCT_CONFIG_INITIALIZED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_PROFILE_INITIALIZED_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_PROFILE_UPDATES_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_PUSH_OPENED_CALLBACK;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_VARIABLES_CHANGED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_VARIABLES_CHANGED_AND_NO_DOWNLOADS_PENDING;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_VARIABLES_FETCHED;
import static com.clevertap.unity.CleverTapUnityCallback.CLEVERTAP_VARIABLE_VALUE_CHANGED;
import static com.clevertap.unity.CleverTapUnityPlugin.LOG_TAG;

import android.annotation.SuppressLint;
import android.net.Uri;
import android.util.Log;

import androidx.annotation.Nullable;

import com.clevertap.android.sdk.CTFeatureFlagsListener;
import com.clevertap.android.sdk.CTInboxListener;
import com.clevertap.android.sdk.CleverTapAPI;
import com.clevertap.android.sdk.InAppNotificationButtonListener;
import com.clevertap.android.sdk.InAppNotificationListener;
import com.clevertap.android.sdk.InboxMessageButtonListener;
import com.clevertap.android.sdk.InboxMessageListener;
import com.clevertap.android.sdk.PushPermissionResponseListener;
import com.clevertap.android.sdk.SyncListener;
import com.clevertap.android.sdk.displayunits.DisplayUnitListener;
import com.clevertap.android.sdk.displayunits.model.CleverTapDisplayUnit;
import com.clevertap.android.sdk.inapp.CTInAppNotification;
import com.clevertap.android.sdk.inapp.callbacks.FetchInAppsCallback;
import com.clevertap.android.sdk.inbox.CTInboxMessage;
import com.clevertap.android.sdk.interfaces.OnInitCleverTapIDListener;
import com.clevertap.android.sdk.product_config.CTProductConfigListener;
import com.clevertap.android.sdk.variables.Var;
import com.clevertap.android.sdk.variables.callbacks.FetchVariablesCallback;
import com.clevertap.android.sdk.variables.callbacks.VariableCallback;
import com.clevertap.android.sdk.variables.callbacks.VariablesChangedCallback;
import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class CleverTapUnityCallbackHandler implements SyncListener, InAppNotificationListener,
        CTInboxListener, InAppNotificationButtonListener, InboxMessageButtonListener,
        DisplayUnitListener, CTFeatureFlagsListener, CTProductConfigListener,
        OnInitCleverTapIDListener, InboxMessageListener, PushPermissionResponseListener {

    private static final String CLEVERTAP_GAME_OBJECT_NAME = "AndroidCallbackHandler";

    private static CleverTapUnityCallbackHandler instance = null;

    public synchronized static CleverTapUnityCallbackHandler getInstance() {
        if (instance == null) {
            instance = new CleverTapUnityCallbackHandler();
        }
        return instance;
    }

    public static void handleDeepLink(Uri data) {
        final String json = data.toString();
        sendToUnity(CLEVERTAP_DEEP_LINK_CALLBACK, json);
    }

    public static void handlePushNotification(JSONObject data) {
        final String json = data.toString();
        sendToUnity(CLEVERTAP_PUSH_OPENED_CALLBACK, json);
    }

    private static void sendToUnity(final CleverTapUnityCallback callback, final String data) {
        UnityPlayer.UnitySendMessage(CLEVERTAP_GAME_OBJECT_NAME, callback.callbackName, data);
    }

    private final VariablesChangedCallback variablesChangedCallback;
    private final VariablesChangedCallback variablesChangedAndNoDownloadsPendingCallback;

    private CleverTapUnityCallbackHandler() {
        variablesChangedCallback = getVariablesChangedCallback();
        variablesChangedAndNoDownloadsPendingCallback = getVariablesChangedAndNoDownloadsPending();
    }

    public void attachToApiInstance(CleverTapAPI clevertap) {
        clevertap.unregisterPushPermissionNotificationResponseListener(this);
        clevertap.registerPushPermissionNotificationResponseListener(this);
        clevertap.setInAppNotificationListener(this);
        clevertap.setSyncListener(this);
        clevertap.setCTNotificationInboxListener(this);
        clevertap.setInboxMessageButtonListener(this);
        clevertap.setCTInboxMessageListener(this);
        clevertap.setInAppNotificationButtonListener(this);
        clevertap.setDisplayUnitListener(this);
        clevertap.setCTFeatureFlagsListener(this);
        clevertap.setCTProductConfigListener(this);
        clevertap.removeVariablesChangedCallback(variablesChangedCallback);
        clevertap.addVariablesChangedCallback(variablesChangedCallback);
        clevertap.onVariablesChangedAndNoDownloadsPending(variablesChangedAndNoDownloadsPendingCallback);
    }

    //OnInitCleverTapIDListener
    @Override
    public void onInitCleverTapID(String cleverTapID) {
        final String json = "{cleverTapID:" + cleverTapID + "}";
        try {
            sendToUnity(CLEVERTAP_INIT_CLEVERTAP_ID_CALLBACK, json);
        } catch (Throwable t) {
            Log.e(LOG_TAG, "onInitCleverTapID error", t);
        }
    }

    //Push primer listener
    @Override
    public void onPushPermissionResponse(boolean accepted) {
        sendToUnity(CLEVERTAP_ON_PUSH_PERMISSION_RESPONSE_CALLBACK, String.valueOf(accepted));
    }

    // InAppNotificationListener
    public boolean beforeShow(Map<String, Object> var1) {
        return true;
    }

    @SuppressLint("RestrictedApi")
    @Override
    public void onShow(CTInAppNotification ctInAppNotification) {
        if (ctInAppNotification != null && ctInAppNotification.getJsonDescription() != null) {
            final String json = "{inApp onShow() json payload:" + ctInAppNotification.getJsonDescription().toString() + "}";
            sendToUnity(CLEVERTAP_INAPP_NOTIFICATION_SHOW_CALLBACK, json);
        }
    }

    @Override
    public void onDismissed(Map<String, Object> var1, @Nullable Map<String, Object> var2) {
        if (var1 == null && var2 == null) {
            return;
        }

        JSONObject extras = var1 != null ? new JSONObject(var1) : new JSONObject();
        String _json = "{extras:" + extras + ",";

        JSONObject actionExtras = var2 != null ? new JSONObject(var2) : new JSONObject();
        _json += "actionExtras:" + actionExtras + "}";

        final String json = _json;
        sendToUnity(CLEVERTAP_INAPP_NOTIFICATION_DISMISSED_CALLBACK, json);
    }

    // SyncListener
    @Override
    public void profileDataUpdated(JSONObject updates) {

        if (updates == null) {
            return;
        }

        final String json = "{updates:" + updates + "}";
        sendToUnity(CLEVERTAP_PROFILE_UPDATES_CALLBACK, json);
    }

    @Override
    public void profileDidInitialize(String CleverTapID) {

        if (CleverTapID == null) {
            return;
        }

        final String json = "{CleverTapID:" + CleverTapID + "}";
        sendToUnity(CLEVERTAP_PROFILE_INITIALIZED_CALLBACK, json);
    }

    //Inbox Listeners
    @Override
    public void inboxDidInitialize() {
        final String json = "{CleverTap App Inbox Initialized}";
        sendToUnity(CLEVERTAP_INBOX_DID_INITIALIZE, json);
    }

    @Override
    public void inboxMessagesDidUpdate() {
        final String json = "{CleverTap App Inbox Messages Updated}";
        sendToUnity(CLEVERTAP_INBOX_MESSAGES_DID_UPDATE, json);
    }

    //Inbox Button Click Listener
    @Override
    public void onInboxButtonClick(HashMap<String, String> payload) {
        JSONObject jsonObject = new JSONObject(payload);
        final String json = "{inbox button payload:" + jsonObject + "}";
        sendToUnity(CLEVERTAP_ON_INBOX_BUTTON_CLICKED, json);
    }

    @Override
    public void onInboxItemClicked(CTInboxMessage message, int contentPageIndex, int buttonIndex) {
        if (message != null && message.getData() != null) {
            JSONObject jsonObject = new JSONObject();
            try {
                jsonObject.put("ContentPageIndex", contentPageIndex);
                jsonObject.put("ButtonIndex", buttonIndex);
                jsonObject.put("CTInboxMessagePayload", message.getData());
                sendToUnity(CLEVERTAP_ON_INBOX_ITEM_CLICKED, jsonObject.toString());
            } catch (JSONException e) {
                throw new RuntimeException(e);
            }
        }
    }

    // Variables callbacks
    public FetchVariablesCallback getFetchVariablesCallback(int callbackId) {
        return isSuccess -> {
            JSONObject json = new JSONObject();
            try {
                json.put("callbackId", callbackId);
                json.put("isSuccess", isSuccess);
            } catch (JSONException e) {
                throw new RuntimeException(e);
            }

            sendToUnity(CLEVERTAP_VARIABLES_FETCHED, json.toString());
        };
    }

    public <T> VariableCallback<T> getVariableCallback() {
        return new VariableCallback<T>() {
            @Override
            public void onValueChanged(Var variable) {
                sendToUnity(CLEVERTAP_VARIABLE_VALUE_CHANGED, variable.name());
            }
        };
    }

    public VariableCallback<String> getFileVariableCallback() {
        return new VariableCallback<String>() {
            @Override
            public void onValueChanged(Var<String> variable) {
                sendToUnity(CLEVERTAP_FILE_VARIABLE_READY, variable.name());
            }
        };
    }

    public VariablesChangedCallback getVariablesChangedCallback() {
        return new VariablesChangedCallback() {
            @Override
            public void variablesChanged() {
                sendToUnity(CLEVERTAP_VARIABLES_CHANGED, "{ Variables Changed Callback }");
            }
        };
    }

    public VariablesChangedCallback getVariablesChangedAndNoDownloadsPending() {
        return new VariablesChangedCallback() {
            @Override
            public void variablesChanged() {
                sendToUnity(CLEVERTAP_VARIABLES_CHANGED_AND_NO_DOWNLOADS_PENDING, "{ Variables Changed No Downloads Pending Callback }");
            }
        };
    }

    //FetchInAppsCallback
    public FetchInAppsCallback getFetchInAppsCallback(int callbackId) {
        return isSuccess -> {
            JSONObject json = new JSONObject();
            try {
                json.put("callbackId", callbackId);
                json.put("isSuccess", isSuccess);
            } catch (JSONException e) {
                throw new RuntimeException(e);
            }

            sendToUnity(CLEVERTAP_INAPPS_FETCHED, json.toString());
        };
    }

    @Override
    public void onInAppButtonClick(HashMap<String, String> payload) {
        JSONObject jsonObject = new JSONObject(payload);
        final String json = "{inapp button payload:" + jsonObject + "}";
        sendToUnity(CLEVERTAP_ON_INAPP_BUTTON_CLICKED, json);
    }

    //Native Display Listener
    @Override
    public void onDisplayUnitsLoaded(ArrayList<CleverTapDisplayUnit> units) {
        try {
            JSONArray jsonArray = JsonConverter.displayUnitListToJSONArray(units);
            final String json = "{display units:" + jsonArray + "}";
            sendToUnity(CLEVERTAP_DISPLAY_UNITS_UPDATED, json);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    //Feature Flag Listener
    @Override
    public void featureFlagsUpdated() {
        final String json = "{CleverTap App Feature Flags Updated}";
        sendToUnity(CLEVERTAP_FEATURE_FLAG_UPDATED, json);
    }

    //Product Config Listener
    @Override
    public void onInit() {
        final String json = "{CleverTap App Product Config Initialized}";
        sendToUnity(CLEVERTAP_PRODUCT_CONFIG_INITIALIZED, json);
    }

    @Override
    public void onFetched() {
        final String json = "{CleverTap App Product Config Fetched}";
        sendToUnity(CLEVERTAP_PRODUCT_CONFIG_FETCHED, json);
    }

    @Override
    public void onActivated() {
        final String json = "{CleverTap App Product Config Activated}";
        sendToUnity(CLEVERTAP_PRODUCT_CONFIG_ACTIVATED, json);
    }
}
