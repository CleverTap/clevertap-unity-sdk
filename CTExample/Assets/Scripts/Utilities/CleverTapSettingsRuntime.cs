using System.IO;
using UnityEngine;

namespace CTExample
{
    public class CleverTapSettingsRuntime
    {
        // Path must be the same as CleverTapSDK CleverTapSettings JSON path
        private static readonly string jsonPath = Path.Combine(Application.streamingAssetsPath, "CleverTapSettings.json");
        private static CleverTapSettingsRuntime _instance;
        private static bool loaded;

        public string CleverTapAccountId;
        public string CleverTapAccountToken;
        public string CleverTapAccountRegion;

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

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(CleverTapAccountId)
                   && !string.IsNullOrEmpty(CleverTapAccountToken);
        }

        private static CleverTapSettingsRuntime LoadSettings()
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                return JsonUtility.FromJson<CleverTapSettingsRuntime>(json);
            }
            Debug.LogError("CleverTapSettings JSON not found. \n" +
                "If you want to use CleverTapSettings runtime, check the CleverTapSettings \"Save to streaming assets\" option.");
            return null;
        }
    }
}