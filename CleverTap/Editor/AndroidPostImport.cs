using System.Linq;
using UnityEditor;

class AndroidPostImport : AssetPostprocessor {
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload) {
        if (AssetDatabase.IsValidFolder("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper") && importedAssets?.Length > 0 && importedAssets.Contains("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper")) {
            AssetDatabase.MoveAsset("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper", "Assets/CleverTap/Plugins/Android/clevertap-android-wrapper.androidlib");
        }
    }
}