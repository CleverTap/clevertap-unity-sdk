## Unity iOS/Android plugin for the CleverTap SDK

1. Copy `Plugin/CleverTapUnity`, `Plugin/Editor` and `Plugin/Plugins/Android` (or copy the files in `Plugin/Plugins/Android` to your existing `Assets/Plugins/Android` directory) into the Assets directory of your Unity Project.

2. Create an empty game object (GameObject -> Create Empty) and rename it `CleverTapUnity`. Add `Assets/CleverTapUnity/CleverTapUnity-Scripts/CleverTapUnity.cs` as a component of the `CleverTapUnity GameObject`.
    
    ![alt text](example/images/unity_gameobj.jpg  "unity game object")

3. Select the `CleverTapUnity GameObject` you created in the Hierarchy pane and add your CleverTap settings inside the Inspector window. You must include your `CleverTap Account ID` and `CleverTap Account Token` from your [CleverTap Dashboard -> Settings](https://dashboard.clevertap.com/x/settings.html).

4. Edit `Assets/CleverTapUnity/CleverTapUnity-Scripts/CleverTapUnity.cs` to add your calls to CleverTap SDK.  See usage examples in [example/CleverTapUnity.cs](example/CleverTapUnity.cs).  For more information check out our [documentation](https://support.clevertap.com/docs.html "CleverTap Technical Documentation").

### iOS Specific:
- Edit `Assets/Editor/CleverTapPostBuildProcessor.cs` in your Unity project to include your `CleverTap Account ID` and `CleverTap Account Token` from your [CleverTap Dashboard -> Settings](https://dashboard.clevertap.com/x/settings.html), and add your deep link url scheme, if applicable.

    ![alt text](example/images/ct_settings_ios.jpg  "example CleverTap settings")

- If you want to enable Push Notifications, be sure to add the Push Notifications capability to your Xcode project.  

    ![alt text](example/images/push_entitle.jpg  "push notifications capability")

- Build and run your iOS project.

### Android Specific:
- Edit the `AndroidManifest.xml` file in `Assets/Plugins/Android` to add your Bundle Identifier, GCM Sender ID and Deep Link url scheme (if applicable): 

    ```
    <manifest xmlns:android="http://schemas.android.com/apk/res/android" package="YOUR_BUNDLE_IDENTIFIER" android:versionName="1.0" android:versionCode="1" android:installLocation="preferExternal"> <supports-screens android:smallScreens="true" android:normalScreens="true" android:largeScreens="true" android:xlargeScreens="true" android:anyDensity="true" />
    ```  

    ```
    <permission android:protectionLevel="signature" android:name="YOUR_BUNDLE_IDENTIFIER.permission.C2D_MESSAGE" />
    <uses-permission android:name="YOUR_BUNDLE_IDENTIFIER.permission.C2D_MESSAGE" />
    ```  

    ```
    <receiver
        android:name="com.google.android.gms.gcm.GcmReceiver"
        android:exported="true"
        android:permission="com.google.android.c2dm.permission.SEND" >
        <intent-filter>
            <action android:name="com.google.android.c2dm.intent.RECEIVE" />
            <action android:name="com.google.android.c2dm.intent.REGISTRATION" />
            <category android:name="YOUR_BUNDLE_IDENTIFIER" />
        </intent-filter>
    </receiver>

    ```

    ```
    <meta-data
        android:name="GCM_SENDER_ID"
        android:value="id:YOUR_GCM_SENDER_ID"/>
    ```

    ```
    <!-- Deep Links uncomment and replace YOUR_URL_SCHEME, if applicable, or remove if not supporting deep links-->
    <!--
        <intent-filter android:label="@string/app_name">
        <action android:name="android.intent.action.VIEW" />
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        <data android:scheme="YOUR_URL_SCHEME" />
        </intent-filter>
    -->  
    ```

- Build your app or Android project as usual.
