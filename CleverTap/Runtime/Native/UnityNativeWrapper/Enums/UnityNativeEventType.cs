#if !UNITY_IOS && !UNITY_ANDROID
namespace CleverTapSDK.Native {
    internal enum UnityNativeEventType {
        PageEvent = 1,
        PingEvent = 2, 
        ProfileEvent = 3,
        RecordEvent = 4,
        DataEvent = 5,
        NotificiationViewEvent = 6,
        FetchEvent = 7
    }
}
#endif