package com.clevertap.unity.example;

import com.clevertap.android.pushtemplates.PushTemplateNotificationHandler;
import com.clevertap.android.sdk.CleverTapAPI;
import com.clevertap.unity.CleverTapUnityApplication;

public class CTExampleApplication extends CleverTapUnityApplication {

    @Override
    public void onCreate() {
        CleverTapAPI.setNotificationHandler(new PushTemplateNotificationHandler());
        super.onCreate();
    }
}
