## Troubleshooting Guide for common integration issues in Unity iOS/Android plugin

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
