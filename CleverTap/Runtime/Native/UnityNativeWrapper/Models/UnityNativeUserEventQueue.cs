#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeUserEventQueue : UnityNativeBaseEventQueue {
        internal override Task<List<UnityNativeEvent>> FlushEvents() {
            throw new NotImplementedException();
        }
    }
}
#endif