package com.clevertap.unity;

import android.app.Activity;
import android.app.Application;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.view.Window;
import android.view.WindowManager;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import androidx.annotation.NonNull;
import android.util.Log;

import com.clevertap.android.sdk.CleverTapAPI;
import com.clevertap.android.sdk.CleverTapInstanceConfig;

public class CleverTapUnityAPI {

    /**
     * Initializes the CleverTap SDK for Unity. It is recommended to call this method
     * in {@link android.app.Application#onCreate()} or extend
     * {@link CleverTapUnityApplication} to ensure proper initialization.
     */
    public static void initialize(Context context) {
        CleverTapCustomTemplates.registerCustomTemplates(context);
        initializeCleverTapWithEnvironment(context);
    }

    /**
     * Set a custom instance of {@link CleverTapAPI} that CleverTapUnity will use.
     */
    public static synchronized void setCleverTapApiInstance(CleverTapAPI cleverTapApi) {
        if (cleverTapApi != null) {
            cleverTapApi.setLibrary("Unity");
            CleverTapUnityPlugin.setCleverTapApiInstance(cleverTapApi);
            CleverTapUnityCallbackHandler.getInstance().attachToApiInstance(cleverTapApi);
        }
    }

    /**
     * Handle new intents of a launcher Activity. Call this method in
     * {@link Activity#onNewIntent(Intent)} or extend {@link CleverTapOverrideActivity}.
     *
     * @param activity The launcher Activity
     * @param intent   The intent received in {@link Activity#onNewIntent(Intent)}
     */
    public static void onLauncherActivityNewIntent(@NonNull Activity activity, Intent intent) {
        handleIntent(intent, true);
    }

    /**
     * Handle the starting intent of a launcher Activity. Call this method in
     * {@link Activity#onCreate(Bundle)} or extend {@link CleverTapOverrideActivity}.
     *
     * @param activity The launcher Activity
     */
    public static void onLauncherActivityCreate(@NonNull Activity activity) {
        handleIntent(activity.getIntent(), false);
        setInAppActivityFullScreenFromOtherActivity(activity);
    }

    /**
     * Show CleverTap in-app notifications in full screen.
     *
     * @param application  The application instance.
     * @param isFullScreen Whether to show in-app notifications in full screen or not.
     */
    public static void setInAppsFullScreen(@NonNull Application application, boolean isFullScreen) {
        CleverTapLifecycleCallbacks.register(application, isFullScreen);
    }

    private static void setInAppActivityFullScreenFromOtherActivity(@NonNull Activity activity) {
        Window window = activity.getWindow();
        if (window == null) {
            return;
        }

        int flags = window.getAttributes().flags;
        boolean isFullScreen = (flags & WindowManager.LayoutParams.FLAG_FULLSCREEN) != 0;
        setInAppsFullScreen(activity.getApplication(), isFullScreen);
    }

    private static void handleIntent(Intent intent, boolean isOnNewIntent) {
        if (intent == null || intent.getAction() == null) {
            return;
        }

        if (intent.getAction().equals(Intent.ACTION_VIEW)) {
            Uri data = intent.getData();
            if (data != null) {
                CleverTapUnityCallbackHandler.handleDeepLink(data);
            }
        } else if (isOnNewIntent) {
            // notify the CT SDK if a push notification payload is found since Activity#onNewIntent
            // is not part of the ActivityLifecycleCallback and the Unity activity has singleTask
            // launch mode
            Bundle extras = intent.getExtras();
            boolean isPushNotification = (extras != null && extras.get("wzrk_pn") != null);
            if (isPushNotification) {

                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
                    CleverTapAPI clevertap = CleverTapUnityPlugin.getCleverTapApiInstance();
                    if (clevertap != null) {
                        clevertap.pushNotificationClickedEvent(extras);
                    }
                }
            }
        }
    }

    /**
     * Initializes CleverTap API with environment-specific configuration
     * For debug builds: Reads environment from PlayerPrefs or manifest metadata
     * For release builds: Always uses primary environment (no suffix)
     */
    private static void initializeCleverTapWithEnvironment(Context context) {
        String envSuffix = "";
        boolean isDebugBuild = false;
        Log.d("CleverTap", "Initialize CleverTap With Environment.");


        // Check if this is a debug/development build
        try {
            ApplicationInfo appInfo = context.getApplicationInfo();
            isDebugBuild = (appInfo.flags & ApplicationInfo.FLAG_DEBUGGABLE) != 0;
            Log.d("CleverTap", "Initialize CleverTap With Environment. isDebugBuild: " + isDebugBuild);
        } catch (Exception e) {
            Log.e("CleverTap", "Error checking debug build status: " + e.getMessage());
        }

        // Development builds: Read environment from PlayerPrefs
        if (isDebugBuild) {
            String environment = CleverTapUnityPlugin.getUnityPlayerPrefsString(context, "CleverTapEnvironment");
            Log.d("CleverTap", " DEBUG build - Environment from PlayerPrefs: " + environment);

            if (environment != null && !environment.isEmpty()) {
                envSuffix = "_" + environment;
                Log.d("CleverTap", "DEBUG build - Using environment from PlayerPrefs: " + environment);
            } else {
                Log.d("CleverTap", " else DEBUG build - Using environment from PlayerPrefs: " + environment);
                // No PlayerPrefs set, check for default environment from manifest
                try {
                    ApplicationInfo ai = context.getPackageManager().getApplicationInfo(
                        context.getPackageName(), PackageManager.GET_META_DATA);
                    Bundle metaData = ai.metaData;
                    if (metaData != null) {
                        String defaultEnv = metaData.getString("CLEVERTAP_DEFAULT_ENV");
                        if (defaultEnv != null && !defaultEnv.isEmpty()) {
                            envSuffix = "_" + defaultEnv;
                            Log.d("CleverTap", "DEBUG build - Using default environment from manifest: " + defaultEnv);
                        } else {
                            Log.d("CleverTap", "DEBUG build - No environment set, using primary");
                        }
                    } else {
                        Log.d("CleverTap", "DEBUG build - No environment set, using primary");
                    }
                } catch (Exception e) {
                    Log.d("CleverTap", "DEBUG build - No environment set, using primary");
                }
            }
        } else {
            // Release builds: Always use primary environment (base keys)
            Log.d("CleverTap", "RELEASE build - Using primary environment");
        }

        // Read credentials from manifest with environment suffix
        try {
            ApplicationInfo ai = context.getPackageManager().getApplicationInfo(
                context.getPackageName(), PackageManager.GET_META_DATA);
            Bundle metaData = ai.metaData;

            if (metaData != null) {
                String accountId = metaData.getString("CLEVERTAP_ACCOUNT_ID" + envSuffix);
                String token = metaData.getString("CLEVERTAP_TOKEN" + envSuffix);
                String region = metaData.getString("CLEVERTAP_REGION" + envSuffix);
                String proxyDomain = metaData.getString("CLEVERTAP_PROXY_DOMAIN" + envSuffix);
                String spikyProxyDomain = metaData.getString("CLEVERTAP_SPIKY_PROXY_DOMAIN" + envSuffix);

                if (accountId == null || accountId.isEmpty() || token == null || token.isEmpty()) {
                    Log.w("CleverTap", "Missing accountId/token in manifest meta-data, using default instance");
                    setCleverTapApiInstance(CleverTapAPI.getDefaultInstance(context));
                    return;
                }

                CleverTapInstanceConfig config;
                if (region != null && !region.isEmpty()) {
                    config = CleverTapInstanceConfig.createInstance(context, accountId, token, region);
                } else {
                    config = CleverTapInstanceConfig.createInstance(context, accountId, token);
                }

                if (proxyDomain != null && !proxyDomain.isEmpty()) {
                    config.setProxyDomain(proxyDomain);
                }
                if (spikyProxyDomain != null && !spikyProxyDomain.isEmpty()) {
                    config.setSpikyProxyDomain(spikyProxyDomain);
                }

                CleverTapAPI instance = CleverTapAPI.instanceWithConfig(context, config);
                setCleverTapApiInstance(instance);
                Log.d("CleverTap", "Initialized with environment suffix: " + envSuffix);
            } else {
                Log.w("CleverTap", "No meta-data found in AndroidManifest.xml, using default instance");
                setCleverTapApiInstance(CleverTapAPI.getDefaultInstance(context));
            }
        } catch (Exception e) {
            Log.e("CleverTap", "Error reading manifest meta-data: " + e.getMessage());
            setCleverTapApiInstance(CleverTapAPI.getDefaultInstance(context));
        }
    }
}
