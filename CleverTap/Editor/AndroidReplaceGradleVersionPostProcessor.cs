#if UNITY_ANDROID && UNITY_EDITOR
using System.IO;
using UnityEditor.Android;
using UnityEngine;

namespace CleverTapSDK.Private
{
    public class AndroidReplaceGradleVersionPostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 1;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            ReplaceGradleWrapperVersion(path);
        }

        private static void ReplaceGradleWrapperVersion(string unityLibraryPath)
        {
            var gradleWrapperPath = Path.Combine(unityLibraryPath, "../gradle/wrapper/gradle-wrapper.properties");
            if (!File.Exists(gradleWrapperPath))
            {
                Debug.LogError($"Gradle wrapper file not found at {gradleWrapperPath}");
                return;
            }

            var lines = File.ReadAllLines(gradleWrapperPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("distributionUrl"))
                {
                    lines[i] = "distributionUrl=https\\://services.gradle.org/distributions/gradle-8.7-bin.zip";
                    Debug.Log($"Updated Gradle wrapper version to 8.7 in {gradleWrapperPath}");
                    break;
                }
            }
            File.WriteAllLines(gradleWrapperPath, lines);
        }
    }
}
#endif
