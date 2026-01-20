package com.clevertap.unity;

import android.app.Application;

import com.clevertap.android.sdk.ActivityLifecycleCallback;

public class CleverTapUnityApplication extends Application {

    @Override
    public void onCreate() {
        // Initialize CleverTap with custom environment-aware config BEFORE calling super.onCreate()
        // and BEFORE registering ActivityLifecycleCallback to prevent auto-initialization
        CleverTapUnityAPI.initialize(this);
        
        // Now register the lifecycle callback with the already-configured instance
        ActivityLifecycleCallback.register(this);
        super.onCreate();
    }
}
