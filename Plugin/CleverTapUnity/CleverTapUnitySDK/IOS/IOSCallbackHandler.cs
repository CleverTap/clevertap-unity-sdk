using CleverTapUnitySDK.Common;
using UnityEngine;

namespace CleverTapUnitySDK.IOS
{
    public class IOSCallbackHandler : CleverTapCallbackHandler
    {
        public IOSCallbackHandler()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<IOSCallbackHandler>();
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
