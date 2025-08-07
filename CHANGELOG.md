Change Log
==========

Version VERSION *(DD MONTH, YYYY)*
-------------------------------------------
- Allows syncing Custom In-app Templates in the Unity Editor.

Version 5.3.0 *(7 August, 2025)*
-------------------------------------------
- Updated to [CleverTap Android SDK v7.5.0](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev7.5.0)
- Updated to [CleverTap iOS SDK v7.3.1](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/7.3.1)
- Supports - Unity Native App Inbox
- Adds support for Unity 6

Version 5.2.0 *(31 March, 2025)*
-------------------------------------------
- Unity Native Variables - allows defining and syncing Variables in Unity Editor and other builds.
- Compatibility fixes Unity Native for Unity 2021
- Compatibility fixes WebGL for Unity 6000

Version 5.1.0 *(20 March, 2025)*
-------------------------------------------
- Updated to [CleverTap iOS SDK v7.1.1](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/7.1.1)
- Variables Callbacks improvements
- Allow setting custom instance of CleverTap to iOS CleverTapUnityManager
- Allow custom instance of CleverTapAPI to be set to Android CleverTapUnityPlugin
- Replace Handler with Timer in Android CleverTapUnityPlugin
- Add macro to disable iOS UnityAppController subclass

Version 5.0.1 *(11 February, 2025)*
-------------------------------------------
- Fixes Android Gradle project build when building bundle/apk
- Fixes In-app messages not displayed full screen on Android
- Fixes Android empty assets copying
- Fixes iOS empty assets copying

Version 5.0.0 *(31 January, 2025)*
-------------------------------------------
- Updated to [CleverTap Android SDK v7.1.2](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev7.1.2)
- Updated to [CleverTap iOS SDK v7.1.0](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/7.1.0)
- Supports triggering InApps based on first-time event filtering in multiple triggers (iOS and Android SDK updates)
- Supports [Custom Code Templates](docs/CustomCodeTemplates.md)
- Improvements to Android and iOS integration and SDK initialization
- Improvements to Callbacks
- Adds `CleverTapInboxMessage` model. Adds new methods that use the model `GetAllInboxMessagesParsed`, `GetUnreadInboxMessagesParsed`, `GetInboxMessageForIdParsed`
- Adds missing Android `GetUnreadInboxMessages` and `GetInboxMessageForId` bindings
- Adds iOS `OnCleverTapPushNotificationPermissionStatusCallback` callback
- Implements `CleverTapInAppNotificationShowCallback` on iOS (available from iOS SDK update)
- Fixes iOS Push Permission Response Received message
- Adds UserEventLog methods `GetUserEventLog`, `GetUserEventLogCount`, `GetUserAppLaunchCount`, `GetUserEventLogHistory`, `GetUserLastVisitTs`
- Deprecates `EventGetDetail`, `EventGetFirstTime`, `EventGetLastTime`, `EventGetOccurrences`, `UserGetEventHistory`, `UserGetPreviousVisitTime`, `UserGetTotalVisits`
- Deprecates Product Config and Feature Flags methods

Version 4.1.0 *(13 December, 2024)*
-------------------------------------------
- Updated to [CleverTap Android SDK v7.0.3](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev7.0.3)
- Updated to [CleverTap iOS SDK v7.0.3](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/7.0.3)
- Supports triggering InApps based on user attribute changes (iOS and Android SDK updates).
- Supports custom key-value pairs for Image Interstitial and Advanced Builder (iOS and Android SDK updates).
- Supports previewing in-apps created through the new dashboard advanced builder (iOS and Android SDK updates).
- Supports launch with proxy and spiky proxy domain
- Supports file type variables
- Unity native networking improvements
- Fixes missing GetAllInboxMessages() in Android plugin
- Fixes string variables on Android always having default values
- Fixes json messages send from Android Unity plugin

Version 4.0.0 *(07 August, 2024)*
-------------------------------------------
- Added native support for WebGL, Mac and Windows platform

Version 3.1.0 *(24 April, 2024)*
-------------------------------------------
- Updated to [CleverTap Android SDK v6.2.1](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev6.2.1)
- Updated to [CleverTap iOS SDK v6.2.1](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/6.2.1)
- Added privacy manifests support for iOS.
- Supports Android 14, made it compliant with Android 14 requirements. Details [here](https://developer.android.com/about/versions/14/summary)
- Upgrades AGP to 8.2.2 for building the SDK and adds related consumer proguard rules
- Deprecates Xiaomi public methods as we are sunsetting SDK. Details [here](https://dev.mi.com/distribute/doc/details?pId=1555).

Version 3.0.0 *(15 Jan, 2024)*
-------------------------------------------
- Updated to [CleverTap Android SDK v6.0.0](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev6.0.0)
- Updated to [CleverTap iOS SDK v6.0.0](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/6.0.0)
- Support `DateTime` objects for `ProfilePush`, `OnUserLogin`, `RecordEvent` and `RecordChargedEventWithDetailsAndItems`
- Support for [Product Experiences - Remote Config/Variables](docs/Variables.md)  
- Unity Package Manager support
- CleverTap structure and interface changes:
    - Use the static `CleverTap.cs` methods. 
    - `CleverTapBinding.cs` and `CleveTapUnity.cs` are now obsolete. They are still usable with minor changes but will be removed in the future.
    - Improved mechanism to handle callbacks - add an event listener for a callback directly through the `CleverTap` static events. _No need_ to set all callbacks in the `CleverTapUnity.cs` _anymore_.
    - iOS Settings are configured from Assets -> CleverTap Settings
- CleverTap SDK uses EDM4U for dependency management

Version 2.4.2 *(24 October, 2023)*
-------------------------------------------
- Profile APIs support for multiple data types
    - Adds `OnUserLogin(Dictionary<string, object> properties)` and `ProfilePush(Dictionary<string, object> properties)`

Version 2.4.1 *(18 Sept, 2023)*
-------------------------------------------
- Updated to [CleverTap Android SDK v5.1.0](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev5.1.0_ptv1.1.0)
- Fixes push notification clicked event handling for Android 12+

Version 2.4.0 *(15 May, 2023)*
-------------------------------------------
- Adds below new public APIs to support [CleverTap Android SDK v5.0.0](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev5.0.0_ptv1.0.9)
    - `CleverTapBinding.IsPushPermissionGranted()`, `CleverTapBinding.PromptPushPrimer(object)`, `CleverTapBinding.PromptForPushPermission(boolean)`
- Adds push permission callback method `CleverTapOnPushPermissionResponseCallback` which returns true/false after user allows/denies the notification permission.
- Adds `CleverTapInAppNotificationShowCallback` to handle InApp notification shown - Only for Android.
- Adds `DeleteInboxMessagesForIDs` for deleting multiple app inbox messages by passing a collection of messageIDs.
- Adds `DeleteInboxMessageForID` for deleting single app inbox messages by passing a messageID.
- Adds `MarkReadInboxMessagesForIDs` for marking multiple app inbox messages as read messages by passing a collection of messageIDs.
- Adds `MarkReadInboxMessageForID` for marking an app inbox messages as read message by passing a messageID.

Version 2.3.1 *(13 April, 2023)*
-------------------------------------------
- Updated to [CleverTap Android SDK v4.6.9](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev4.6.9)
- Updated to [CleverTap iOS SDK v4.2.2](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/4.2.2)
- Fixes `CreateNotificationChannelWithSound`, `CreateNotificationChannelWithGroup`, `CreateNotificationChannelWithGroupAndSound` - Only for Android
- Updates the callback `CleverTapInboxItemClicked` to receive inbox item click with item payload.The `ContentPageIndex` corresponds the index of the item clicked in the list whereas the `ButtonIndex` for the App Inbox button clicked (0, 1, or 2). A value of -1 indicates the App Inbox item is clicked.
- Adds `DismissAppInbox` to dismiss the App Inbox.

Version 2.3.0 *(16 March, 2023)*
-------------------------------------------
- Updated to [CleverTap Android SDK v4.6.7](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/corev4.6.7_ptv1.0.5.1)
- Updated to [CleverTap iOS SDK v4.1.6](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/4.1.6)
- Support for Exoplayer v2.17.1 - Only for Android.
- Adds new callback `CleverTapInboxItemClicked` to receive inbox item click.
- Adds a public method `deleteInboxMessagesForIds()` for deleting multiple App Inbox messages by passing a collection of messageIDs. Please note that this is only for iOS, and NO-OP for Android as of now.

Version 2.2.0 *(14 FEB, 2022)*
-------------------------------------------
- Add public APIs for Increment/Decrement ops and InApp Controls
- Deprecates `profileGetCleverTapID()`and `profileGetCleverTapAttributionIdentifier()`.
- Adds a new public method `getCleverTapID()` as an alternative to above-deprecated methods.
- Updated to [CleverTap Android SDK v4.4.0](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/core-v4.4.0)
- Updated to [CleverTap iOS SDK v4.0.0](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/4.0.0)

Version 2.1.2 *(19 May, 2021)*
-------------------------------------------
- Updated to [CleverTap iOS SDK v3.9.4](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/3.9.4)
- Updated to [CleverTap Android SDK v4.1.1](https://github.com/CleverTap/clevertap-android-sdk/releases/tag/core-v4.1.1)
- Removed Product Experiences (Dynamic Variables) related code
- Removed pushGooglePlusPerson and profileSetGraphUser API

Version 2.1.1 *(1 February, 2021)*
-------------------------------------------
- Update iOS CleverTapPostBuildProcessor.cs to fix Project Target issue on **UNITY_2019_3_OR_NEWER**
- Updated to CleverTap Android SDK v4.0.2

Version 2.1.0 *(16 October, 2020)*
-------------------------------------------
- Supports [CleverTap iOS SDK v3.9.1](https://github.com/CleverTap/clevertap-ios-sdk/releases/tag/3.9.1)
- Supports **Major release** of [CleverTap Android SDK v4.0.0](https://github.com/CleverTap/clevertap-android-sdk/blob/master/docs/CTCORECHANGELOG.md), which will break your existing integration. Please go through [Migration guide](https://github.com/CleverTap/clevertap-android-sdk/blob/master/docs/CTV4CHANGES.md) for smooth integration.

Version 1.3.0 *(19 May, 2020)*
-------------------------------------------
- Updated to CleverTap Android SDK v3.8.0
- Updated to CleverTap iOS SDK v3.8.0

Version 2.0.1 *(19 May, 2020)*
-------------------------------------------
- Updated to CleverTap Android SDK v3.8.0
- Updated to CleverTap iOS SDK v3.8.0
- Supports Android X

Version 1.2.9 *(27 February, 2020)*
-------------------------------------------
* Updated to CleverTap Android SDK v3.6.4

Version 1.2.7 *(24 February, 2020)*
-------------------------------------------
* Updated to CleverTap Android SDK v3.6.3

Version 1.2.6 *(20 February, 2020)*
-------------------------------------------
* Bug fixes

Version 1.2.5 *(15 January, 2020)*
-------------------------------------------
* Bug fixes and performance improvement

Version 1.2.4 *(31 October, 2019)*
-------------------------------------------
* Exposed method to pass custom install referrer parameters in Android

Version 2.0.0 *(30 October, 2019)*
-------------------------------------------
* Update Play Services Resolver to support Androidx dependencies

Version 1.2.3 *(11 October, 2019)*
-------------------------------------------
* Update to CleverTap Android SDK v3.6.0
* Update to CleverTap iOS SDK v3.7.0

Version 1.2.2 *(30 May, 2019)*
-------------------------------------------
* Update to CleverTap Android SDK v3.5.1
* Update to CleverTap iOS SDK v3.5.0

Version 1.2.1 *(08 March, 2019)*
-------------------------------------------
* Update Play Services Resolver to support FCM in Android

Version 1.2.0 *(15 February, 2019)*
-------------------------------------------
* Update to CleverTap Android SDK v3.4.2
* Update to CleverTap iOS SDK v3.4.1

Version 1.1.5 *(13 November, 2018)*
-------------------------------------------
* Update to CleverTap Android SDK v3.3.2

Version 1.1.4 *(01 November, 2018)*
-------------------------------------------
* Update to CleverTap Android SDK v3.3.1
* Update to CleverTap iOS SDK v3.3.0

Version 1.1.3 *(26 September, 2018)*
-------------------------------------------
* Update to CleverTap iOS SDK v 3.2.2

Version 1.1.2 *(12 September, 2018)*
-------------------------------------------
* Update to CleverTap Android SDK v 3.2.0
* Update to CleverTap iOS SDK v 3.2.0

Version 1.1.1 *(22 May, 2018)*
-------------------------------------------
* Update to CleverTap Android SDK v 3.1.10

Version 1.1.0 *(18 May, 2018)*
-------------------------------------------
* Update to CleverTap Android SDK v 3.1.9
* Update to CleverTap iOS SDK v 3.1.7

Version 1.0.9 *(15 January, 2018)*
-------------------------------------------
* Update to CleverTap Android SDK v 3.1.8


Version 1.0.8 *(13 October, 2017)*
-------------------------------------------
* Update to CleverTap iOS SDK v 3.1.6

Version 1.0.7 *(10 October, 2017)*
-------------------------------------------
* Update to CleverTapAndroidSDK v 3.1.7

Version 1.0.6 *(21 September, 2017)*
-------------------------------------------
*(Supports CleverTap 3.1.5/3.1.6)*

Version 1.0.5 *(17 August, 2017)*
-------------------------------------------
*expose iOS pushInstallReferrerSource api to unity*

Version 1.0.4 *(18 July, 2017)*
-------------------------------------------
*add unitypackage*
*remove library aars in favor of PlayServicesResolver*

Version 1.0.3 *(30 June, 2017)*
-------------------------------------------
*(Supports CleverTap 3.1.4)*

Version 1.0.2 *(22 February, 2017)*
-------------------------------------------
*(Supports CleverTap 3.1.2)*

adds setApplicationIconBadgeNumber api

Version 1.0.1 *(30 January, 2017)*
-------------------------------------------
*(Supports CleverTap 3.1.2)*

Version 1.0.0 *(23 October, 2016)*
-------------------------------------------
*(Supports CleverTap 3.1.0)*


