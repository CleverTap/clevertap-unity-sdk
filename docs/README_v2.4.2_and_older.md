
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

1. Import the CleverTapUnityPlugin.unitypackage into your Unity Project (`Assets` > `Import Package` > `Custom Package`) or manually Copy `Plugin/CleverTapUnity` and `Plugin/Plugins/Android` (or copy the files in `Plugin/Plugins/Android` to your existing `Assets/Plugins/Android` directory) into the Assets directory of your Unity Project.

2. Create an empty game object (GameObject -> Create Empty) and rename it `CleverTapUnity`. Add `Assets/CleverTapUnity/CleverTapUnity-Scripts/CleverTapUnity.cs` as a component of the `CleverTapUnity GameObject`. 

| NOTE: To receive the SDK callbacks the name of the game object must be `CleverTapUnity` |
| --- |

   ![alt text](/docs/images/unity_gameobj.jpg  "unity game object")

3. Select the `CleverTapUnity GameObject` you created in the Hierarchy pane and add your CleverTap settings inside the Inspector window. You must include your `CleverTap Account ID` and `CleverTap Account Token` from your [CleverTap Dashboard -> Settings](https://dashboard.clevertap.com/x/settings.html).

4. Edit `Assets/CleverTapUnity/CleverTapUnity-Scripts/CleverTapUnity.cs` to add your calls to CleverTap SDK.  See usage examples in [example/CleverTapUnity.cs](/example/CleverTapUnity.cs).  For more information check out our [documentation](https://developer.clevertap.com/docs "CleverTap Technical Documentation").

5. For more details around setup check out Android Specific Instructions [here](/docs/Instructions-Android_v2.4.2_and_older.md) and iOS Specific Instructions [here](/docs/Instructions-iOS_v2.4.2_and_older.md).

## 💻 Example Usage #

- [See the CleverTap Unity Usage Documentation](/docs/Usage_v2.4.2_and_older.md)
- [See the included Example Project](/example) showing the integration of our Plugin

## 🆕 Changelog #

Check out the CleverTap Unity plugin SDK [Change Log](/CHANGELOG.md) here.

## ⁉️ Questions? #

 If you have questions or concerns, you can reach out to the CleverTap support team from the CleverTap Dashboard. 
 
**TroubleShooting Guide:** Please refer [here](/docs/Troubleshooting.md) if you are facing common integration issue.
