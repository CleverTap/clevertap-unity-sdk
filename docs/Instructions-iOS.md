## 👩‍💻 iOS Specific Instructions:

iOS dependencies are added using CocoaPods Podfile. After exporting the project to XCode, you need to run `pod install`. This can be _automated_ by [EDM4U](https://github.com/googlesamples/unity-jar-resolver) plugin to be done on each export. You can always manually install pods by running `pod install` in the exported XCode project. Ensure to start `.xcworkspace` to build the game with dependencies.

1. Set your Account Id (Project ID), Account token (Project Token), and other settings in the CleverTap Settings **Assets** > **CleverTap Settings**

![CleverTap Settings](/docs/images/clevertap_settings.png  "CleverTap Settings")

This is needed so the CleverTap Post Processor script can add those settings automatically in the `Info.plist` file.

> 📘 IDFV Usage for CleverTap ID
> 
> Starting from CleverTap Unity SDK 2.1.2, you can optionally disable the usage of IDFV for CleverTap ID. We recommend this for apps that send data from different iOS apps to a common CleverTap account. By default, the `CleverTapDisableIDFV` checkbox is false.

**Initialization**

On iOS, the SDK initialization is done using `[CleverTap autoIntegrate]` in the `application:didFinishLaunchingWithOptions:` method using `CleverTapUnityAppController` overriding the default `UnityAppController`.

If you use your own `UnityAppController` override, inherit from the `CleverTapUnityAppController` or call the CleverTap methods.

**Settings**
_Disable IDFV_
Set the `CleverTapDisableIDFV` setting to `true`, to disable the usage of IDFV for CleverTap ID.

_Use autoIntegrate_

Initializes the iOS SDK using `[CleverTap autoIntegrate]`. This _swizzles_ the application and push notification methods automatically.
If you need to disable swizzling, set the `CleverTapIOSUseAutoIntegrate` setting to false.

_Use UNUserNotificationCenter_

The `UNUserNotificationCenterDelegate` is required so notifications can be handled correctly. It is needed regardless of using autoIntegrate or not.
If you implement the delegate in your own class, you can disable the `UNUserNotificationCenterDelegate` to be set to `CleverTapUnityAppController` by unchecking the `CleverTapIOSUseUNUserNotificationCenter` setting. 
In this case, you _must_ implement the `UNUserNotificationCenterDelegate` methods yourself and call the CleverTap methods.

_Present notification on foreground_

Whether to present remote notifications while app is on foreground. Default value is `false`. The `UNNotificationPresentationOptionNone` is used. The notification will be handled without presenting it, if it has a deeplink, it will be opened.
If changed to `true`, notification will be presented as banner/alert using `UNNotificationPresentationOptionBanner/Alert | UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound` options.

2. Set up [EDM4U](<>) plugin to install CocoaPods automatically. Go to **Assets** > **External Dependency Manager** > **iOS Resolver** > **Settings**. You can also install manually using `pod install`.

![iOS Resolver Settings](/docs/images/ios_resolver_settings.png  "iOS Resolver Settings")

2. Open the `.xcworkspace` project.
3. To enable Push Notifications, ensure to add the Push Notifications capability to your Xcode project.  
4. Build and run your iOS project.
