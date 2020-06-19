## Troubleshooting Guide for common integration issues in Unity plugin

### Android 

**1. Not getting any callbacks for deeplink/inbox etc. in the game object?**
    
    Make sure that the name of the game object where you want the callback is "CleverTapUnity".
    Otherwise, you won't receive any callback.

**2. Getting build errors after adding FCM or any other SDK's?**

    Please check once the version of the PlayServiceResolver which you are using in your project.
    There might be the case that the PlayServiceResolver has been overridden by adding the SDK, which is causing the problem.
    If any such build error occurs due to some issue with the PlayServicesResolver version we from SDK end can’t do anything
    about it. Please use the suitable version of PlayServicesResolver to fix this.

### iOS 

**1. Submit to App Store issue: Unsupported Architectures. The executable contains unsupported architecture '[x86_64, i386]**

After adding the [run script](https://github.com/CleverTap/clevertap-unity-sdk/blob/master/Plugin/CleverTapUnity/iOS/strip.sh) to your build phases, if App Store still reports **Unsupported Architecture. Your executable contains unsupported architecture '[x86_64, i386]** while submitting the app to the App Store.

Follow the below steps to validate the archive: 

1. Open Terminal
2. Navigate to **CleverTapSDK** and **SDWebImage** framework inside your Application
3. For CleverTapSDK: Run the following command:-
```shell
 cd Your-Application-Path/Frameworks/CleverTapUnity/ios/CleverTapSDK.framework.
 lipo -remove x86_64 CleverTapSDK -o CleverTapSDK
 lipo -remove i386 CleverTapSDK -o CleverTapSDK
 ```

4. For SDWebImage: Run the following command:-
```shell
 cd  Your-Application-Path/Frameworks/CleverTapUnity/ios/SDWebImage.framework
 lipo -remove i386 SDWebImage -o SDWebImage
 lipo -remove x86_64 SDWebImage -o SDWebImage
  ```
