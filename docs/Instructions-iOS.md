## 👩‍💻 iOS Specific Instructions:

iOS dependencies are added using CocoaPods Podfile. After exporting the project to XCode, you need to run `pod install`. This can be _automated_ by [EDM4U](https://github.com/googlesamples/unity-jar-resolver) plugin to be done on each export. You can always manually install pods by running `pod install` in the exported XCode project. Ensure to start `.xcworkspace` to build the game with dependencies.

1. Set your account Id, token, and region, as well as personalization and IDFV settings in the CleverTap Settings **Assets** > **CleverTap Settings**

![CleverTap iOS Settings](docs/images/clevertap_ios_settings.png  "CleverTap iOS Settings")

This is needed so the CleverTap Post Processor script can add those settings automatically in the `Info.plist` file.

> 📘 IDFV Usage for CleverTap ID
> 
> Starting from CleverTap Unity SDK 2.1.2, you can optionally disable the usage of IDFV for CleverTap ID. We recommend this for apps that send data from different iOS apps to a common CleverTap account. By default, the CleverTapDisableIDFV checkbox is cleared.

2. Set up [EDM4U](<>) plugin to install CocoaPods automatically. Go to **Assets** > **External Dependency Manager** > **iOS Resolver** > **Settings**. You can also install manually using `pod install`.

![iOS Resolver Settings](docs/images/ios_resolver_settings.jpg  "iOS Resolver Settings")

2. Open the `.xcworkspace` project.
3. To enable Push Notifications, ensure to add the Push Notifications capability to your Xcode project.  
4. Build and run your iOS project.
