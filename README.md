
<p align="center">
    <img src="https://github.com/CleverTap/clevertap-ios-sdk/blob/master/docs/images/clevertap-logo.png" width="50%"/>
</p>


# CleverTap Unity Plugin
[![codebeat badge](https://codebeat.co/badges/f66e4e0c-4989-4caa-b0ec-ee405c30cb4d)](https://codebeat.co/projects/github-com-clevertap-clevertap-unity-sdk-master)
<a href="https://github.com/CleverTap/clevertap-unity-sdk/releases">
    <img src="https://img.shields.io/github/release/CleverTap/clevertap-unity-sdk.svg" />
</a>

## 👋 Introduction

The CleverTap Unity Plugin for Mobile Customer Engagement and Analytics solutions.

For more information check out our [website](https://clevertap.com/ "CleverTap")  and  [documentation](https://developer.clevertap.com/docs/ "CleverTap Technical Documentation").

To get started, sign up [here](https://clevertap.com/live-product-demo/).

## 🛠 Installation and Setup #

You can install the CleverTap Unity SDK using the `.unitypackage` Unity package or as a local package through Unity Package Manager (UPM).

### Import the CleverTap Unity Package

1. Download the latest version of the CleverTap Unity package. Import the `.unitypackage` into your Unity Project. **Go to Assets** > **Import Package** > **Custom Package**. 
2. Add the **PlayServiceResolver** and the **ExternalDependencyManager** folders. These folders will install the **EDM4U** plugin, which automatically adds all the Android and iOS dependencies when building your project.
3. Ensure that the scripts inside the `Editor` folder are added (`AndroidPostImport`, `CleverTapPostBuildProcessor.` and the other scripts). The `AndroidPostImport` script sets up `clevertap-android-wrapper` library for Android. `CleverTapPostBuildProcessor` helps iOS setup.

### Import the CleverTap Unity Package as a Local Dependency

Clone the latest release version of CleverTap Unity SDK. The SDK can be imported as a local package through the Unity Package Manager.

### Set Up the Unity SDK

CleverTap API can be accessed anywhere in your project by simply calling the static `CleverTap` class. _No_ need to create `GameObject` or attach _any_ script. The SDK handles the following: 

- Instantiation of platform-specific binding (iOS, Android, Native)
- Creation of `GameObject`
- Script attachment.

You can view your `CleverTap Account ID` and `CleverTap Account Token` from the _CleverTap Dashboard -> Settings_.

```csharp
// Initialize CleverTap
CleverTap.LaunchWithCredentialsForRegion({YOUR_CLEVERTAP_ACCOUNT_ID}, {YOUR_CLEVERTAP_ACCOUNT_TOKEN}, {CLEVERTAP_ACCOUNT_REGION});
// Enable personalization
CleverTap.EnablePersonalization();
```

#### Callbacks

Add an event listener for a callback directly through the `CleverTap` static events.

```csharp
CleverTap.OnCleverTapDeepLinkCallback += YOUR_CALLBACK_METHOD;  
CleverTap.OnCleverTapProfileInitializedCallback += YOUR_CALLBACK_METHOD;  
CleverTap.OnCleverTapProfileUpdatesCallback += YOUR_CALLBACK_METHOD;
```

### iOS Instructions

iOS specific setup is described in the [iOS Instructions](/docs/Instructions-iOS.md)

### Android Instructions

Android specific setup is described in the [Android Instructions](/docs/Instructions-Android.md)


## 💻 Example Usage #

- [See the CleverTap Unity Usage Documentation](/docs/Usage.md)

## 🆕 Changelog #

Check out the CleverTap Unity plugin SDK [Change Log](/CHANGELOG.md) here.

## ⁉️ Questions? #

 If you have questions or concerns, you can reach out to the CleverTap support team from the CleverTap Dashboard. 
 
**TroubleShooting Guide:** Please refer [here](docs/Troubleshooting.md) if you are facing common integration issue.
