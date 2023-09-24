using CleverTapUnitySDK.Common;
using UnityEngine;

namespace CleverTapUnitySDK.Android
{
    public class AndoridCallbackHandler : CleverTapCallbackHandler
    {
        public AndoridCallbackHandler()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<AndoridCallbackHandler>();
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
