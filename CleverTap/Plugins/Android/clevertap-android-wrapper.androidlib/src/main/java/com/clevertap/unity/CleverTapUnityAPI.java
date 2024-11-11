package com.clevertap.unity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;

import com.clevertap.android.sdk.CleverTapAPI;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;
import java.util.Map;

public class CleverTapUnityAPI {

    public static void initialize(Context context) {
        CleverTapCustomTemplates.registerCustomTemplates(context);
        CleverTapAPI clevertap = CleverTapAPI.getDefaultInstance(context);
        if (clevertap != null) {
            clevertap.setLibrary("Unity");
            CleverTapUnityCallbackHandler.getInstance().attachToApiInstance(clevertap);
        }
    }

    static void handleIntent(Intent intent, Activity activity) {
        if (intent == null) {
            return;
        }
        if (intent.getAction() == null) {
            return;
        }

        if (intent.getAction().equals(Intent.ACTION_VIEW)) {
            Uri data = intent.getData();
            if (data != null) {
                CleverTapUnityCallbackHandler.handleDeepLink(data);
            }
        } else {
            Bundle extras = intent.getExtras();
            boolean isPushNotification = (extras != null && extras.get("wzrk_pn") != null);
            if (isPushNotification) {

                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
                    CleverTapAPI.getDefaultInstance(activity).pushNotificationClickedEvent(extras);
                }

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

                    } catch (JSONException e) {
                        // no-op
                    }
                }
                CleverTapUnityCallbackHandler.handlePushNotification(data);
            }
        }
    }
}
