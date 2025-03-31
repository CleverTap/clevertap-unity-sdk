using System.IO;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Threading.Tasks;
using UnityEngine.Networking;
#endif

namespace CTExample
{
    public class CleverTapSettingsRuntime
    {
        // Path must be the same as CleverTapSDK CleverTapSettings JSON path
        private static readonly string jsonPath = Path.Combine(Application.streamingAssetsPath, "CleverTapSettings.json");

#if !(UNITY_WEBGL && !UNITY_EDITOR)
        private static CleverTapSettingsRuntime _instance;
        private static bool loaded;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        private static Task<CleverTapSettingsRuntime> _instanceTask;
        private static readonly object _lock = new object();
#endif

        public string CleverTapAccountId;
        public string CleverTapAccountToken;
        public string CleverTapAccountRegion;

#if !(UNITY_WEBGL && !UNITY_EDITOR)
        public static CleverTapSettingsRuntime Instance
        {
            get
            {
                if (!loaded)
                {
                    _instance = LoadSettings();
                    loaded = true;
                }
                return _instance;
            }
        }
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        public static Task<CleverTapSettingsRuntime> Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instanceTask == null || _instanceTask.IsFaulted)
                    {
                        _instanceTask = LoadSettingsWebGL();
                    }
                }
                return _instanceTask;
            }
        }
#endif

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(CleverTapAccountId)
                   && !string.IsNullOrEmpty(CleverTapAccountToken);
        }

#if !(UNITY_WEBGL && !UNITY_EDITOR)
        private static CleverTapSettingsRuntime LoadSettings()
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                return JsonUtility.FromJson<CleverTapSettingsRuntime>(json);
            }
            Logger.LogWarning("CleverTapSettings JSON not found. \n" +
                "If you want to use CleverTapSettings runtime, check the CleverTapSettings \"Save to streaming assets\" option.");
            return null;
        }
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        private static async Task<CleverTapSettingsRuntime> LoadSettingsWebGL()
        {
            UnityWebRequest request = UnityWebRequest.Get(jsonPath);
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = request.downloadHandler.text;
                return JsonUtility.FromJson<CleverTapSettingsRuntime>(json);
            }
            else
            {
                Logger.LogWarning("Cannot load CleverTapSettings JSON at " + jsonPath);
                return null;
            }
        }
#endif
    }
}