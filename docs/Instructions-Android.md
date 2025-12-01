## 👨‍💻 Android Specific Instructions:

1. Go to **File** > **Build Settings** > **Android** > **Player Settings** > **Publishing Settings** > **Build**. Enable _.gradle templates_ and _custom AndroidManifest_. EDM4U populates the _Custom Main Gradle Template_ and _Gradle Properties Template_ with the required Android dependencies.

![Android Build Settings](/docs/images/android_settings.png  "Android Build Settings")

2. Once you enable the custom Android Manifest, ensure to add additional configurations to enable Push Notifications and override the default `UnityActivity`. Add your Bundle Identifier, Deep Link URL scheme (if applicable). You can also add your account configuration manually if you did not user the CleverTap Setting from the Unity Editor.

    1. In case you have implemented your own `android.app.Application` class, add the following code in it:
        ```java
        @Override
        public void onCreate() {
            ActivityLifecycleCallback.register(this);
            super.onCreate();
            CleverTapUnityAPI.initialize(this);
        }
        ```
        Otherwise use `com.clevertap.unity.CleverTapUnityApplication` as shown in the Manifest below.

    2. In case you have implemented your own Launcher Activity, add the following code in it:
        ```java
        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            CleverTapUnityAPI.onLauncherActivityCreate(this);
        }

        @Override
        public void onNewIntent(Intent intent) {
            super.onNewIntent(intent);
            CleverTapUnityAPI.onLauncherActivityNewIntent(this, intent);
        }
        ```
        Otherwise use `com.clevertap.unity.CleverTapOverrideActivity` as shown in the Manifest below.
        
### Unity 6 – Application Entry Point Consideration

In **Unity 6**, the Android Player settings provide two application entry point options:

- **Activity**
- **GameActivity**

For CleverTap integration, ensure that the **Activity** option is selected in:

```text
Project Settings → Player → Android → Other Settings → Application Entry Point
```

```xml

<?xml version="1.0" encoding="utf-8"?>
<manifest package="YOUR_BUNDLE_IDENTIFIER"
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:installLocation="preferExternal"
    android:versionCode="1"
    android:versionName="1.0">

    <supports-screens
        android:anyDensity="true"
        android:largeScreens="true"
        android:normalScreens="true"
        android:smallScreens="true"
        android:xlargeScreens="true" />

    <uses-permission android:name="android.permission.INTERNET" />
    <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
    <uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
    <uses-permission android:name="android.permission.WAKE_LOCK" />

    <uses-feature android:glEsVersion="0x00020000" />

    <uses-feature
        android:name="android.hardware.touchscreen"
        android:required="false" />
    <uses-feature
        android:name="android.hardware.touchscreen.multitouch"
        android:required="false" />
    <uses-feature
        android:name="android.hardware.touchscreen.multitouch.distinct"
        android:required="false" />

    <application
        android:debuggable="true"
        android:icon="@drawable/app_icon"
        android:isGame="true"
        android:name="com.clevertap.unity.CleverTapUnityApplication"
        android:label="@string/app_name"
        android:theme="@style/UnityThemeSelector">

        <activity
            android:name="com.clevertap.unity.CleverTapOverrideActivity"
            android:configChanges="mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale"
            android:label="@string/app_name"
            android:launchMode="singleTask"
            android:screenOrientation="fullSensor">

            <intent-filter>
                <action android:name="android.intent.action.MAIN" />

                <category android:name="android.intent.category.LAUNCHER" />
                <category android:name="android.intent.category.LEANBACK_LAUNCHER" />
            </intent-filter>

            <!-- Deep Links uncomment and replace YOUR_URL_SCHEME, if applicable, or remove if not supporting deep links-->
            <!--
            <intent-filter android:label="@string/app_name">
            <action android:name="android.intent.action.VIEW" />
            <category android:name="android.intent.category.DEFAULT" />
            <category android:name="android.intent.category.BROWSABLE" />
            <data android:scheme="YOUR_URL_SCHEME" />
            </intent-filter>
            -->

            <meta-data
                android:name="unityplayer.UnityActivity"
                android:value="true" />
        </activity>

        <service
            android:name="com.clevertap.android.sdk.pushnotification.fcm.FcmMessageListenerService"
            android:exported="true">
            <intent-filter>
                <action android:name="com.google.firebase.MESSAGING_EVENT" />
            </intent-filter>
        </service>

        <service
            android:name="com.clevertap.android.sdk.pushnotification.CTNotificationIntentService"
            android:exported="false">
            <intent-filter>
                <action android:name="com.clevertap.PUSH_EVENT" />
            </intent-filter>
        </service>

        <!-- Uncomment these lines to manually add your account configuration.
        <meta-data
            android:name="CLEVERTAP_ACCOUNT_ID"
            android:value="Your CleverTap Account ID" />

        <meta-data
            android:name="CLEVERTAP_TOKEN"
            android:value="Your CleverTap Account Token" />

        <meta-data
            android:name="CLEVERTAP_REGION"
            android:value="Your CleverTap Account Region" />

        <meta-data
            android:name="CLEVERTAP_PROXY_DOMAIN"
            android:value="Your CleverTap Account Proxy Domain" />

        <meta-data
            android:name="CLEVERTAP_SPIKY_PROXY_DOMAIN"
            android:value="Your CleverTap Account Spiky Proxy Domain" />
        -->
    </application>

</manifest>
```

3. Add your `google-services.json` file to the project's **Assets** folder.
4. Build your app or Android project.
