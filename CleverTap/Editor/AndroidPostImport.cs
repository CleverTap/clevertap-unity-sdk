<<<<<<< HEAD
using System;
using System.Linq;
using UnityEditor;

namespace CleverTapSDK.Private
{
    class AndroidPostImport : AssetPostprocessor
    {
        private static readonly string ctExportCommandLineArg = "-ct-export";

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            string[] cmdArgs = Environment.GetCommandLineArgs();
            if (cmdArgs.Contains(ctExportCommandLineArg))
                return;

            if (AssetDatabase.IsValidFolder("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper") && importedAssets?.Length > 0 && importedAssets.Contains("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper"))
            {
                AssetDatabase.DeleteAsset("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper.androidlib");
                AssetDatabase.MoveAsset("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper", "Assets/CleverTap/Plugins/Android/clevertap-android-wrapper.androidlib");
            }
        }
    }
=======
using System.Linq;
using UnityEditor;

class AndroidPostImport : AssetPostprocessor {
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload) {
        if (AssetDatabase.IsValidFolder("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper") && importedAssets?.Length > 0 && importedAssets.Contains("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper")) {
            AssetDatabase.MoveAsset("Assets/CleverTap/Plugins/Android/clevertap-android-wrapper", "Assets/CleverTap/Plugins/Android/clevertap-android-wrapper.androidlib");
        }
    }
>>>>>>> 58f5a2b (Feature/sdk 3323/variables bindings (#54))
}