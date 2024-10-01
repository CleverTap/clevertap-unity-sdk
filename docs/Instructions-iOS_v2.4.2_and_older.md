## 👩‍💻 iOS Specific Instructions:
- If you want to enable Push Notifications, be sure to add the Push Notifications capability to your Xcode project.  

    ![alt text](/docs/images/push_entitle.jpg  "push notifications capability")
    
- Configure the Framework
   - In Xcode, go to your Targets, under your app’s name
   - Under General, Navigate to Frameworks, Libraries and Embedded Content, please ensure that **CleverTapSDK** and **SDWebImage** frameworks are marked as **Embed & Sign** and **Embedded** to your project.
   
 ![alt text](/docs/images/ct_xcode12.x_framework.png  "frameworks")

- Required for CleverTap Unity v2.1.0 or below: Add a run script to your build phases, In Xcode, go to your Targets, under your app’s name, select Build Phases after   embed frameworks, add a run script phase and set it to use `/bin/sh` and the [script found here](https://github.com/CleverTap/clevertap-unity-sdk/blob/master/Plugin/CleverTapUnity/iOS/strip.sh).

The script will look through your built application’s `Frameworks` folder and strip out the unnecessary simulator architectures from the CleverTakSDK.framework prior to archiving/submitting the app store.

  ![alt text](/docs/images/ct_script_ios.jpg  "run script")
  

- Build and run your iOS project.


### CleverTap Disable IDFV Usage

From CleverTap Unity SDK 2.1.2 onwards, you can disable the usage of IDFV by checking `CLEVERTAP_DISABLE_IDFV` as shown below 

 ![alt text](/docs/images/ct-ios-idfv.png  "CleverTapDisableIDFV")

By default, the value of `CLEVERTAP_DISABLE_IDFV` is not checked which means CleverTap will continue to use the vendor identifier for the purpose of profile creation.
