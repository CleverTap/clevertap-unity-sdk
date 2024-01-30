#if !UNITY_IOS && !UNITY_ANDROID
namespace CleverTapSDK.Native {
    internal enum UnityNativeEventType {
        PageEvent = 1,
        PingEvent = 2, 
        ProfileEvent = 3,
        RaisedEvent = 4,
        DataEvent = 5,
        NVEvent = 6,
        FetchEvent = 7
    }
}
#endif