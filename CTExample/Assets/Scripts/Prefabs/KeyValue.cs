using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace CTExample
{
    public class KeyValue : MonoBehaviour
    {
        [SerializeField] private TMP_Text KeyText;
        [SerializeField] private TMP_Text ValueText;

        void Start()
        {
            KeyText.GetComponent<ClickableText>().OnTextClickedEvent += KeyValue_OnTextClickedEvent;
            ValueText.GetComponent<ClickableText>().OnTextClickedEvent += KeyValue_OnTextClickedEvent;

            RefreshContentHelper.RefreshContentFitters((RectTransform)transform);
        }

        public string GetKey()
        {
            return KeyText.text;
        }

        public void SetKey(string text)
        {
            if (text != KeyText.text)
            {
                KeyText.SetText(text);
            }
        }

        public string GetValue()
        {
            return ValueText.text;
        }

        public void SetValue(string text)
        {
            if (text != ValueText.text)
            {
                ValueText.SetText(text);
            }
        }

        private void KeyValue_OnTextClickedEvent(string text)
        {
            if (File.Exists(text))
            {
                OpenFile(text);
                return;
            }

            TextEditor te = new TextEditor();
            te.text = text;
            te.SelectAll();
            te.Copy();

            Logger.Log($"Copied: {text}");
            Toast.Show($"Copied: {text}");
        }

        private void OpenFile(string filePath)
        {
            Logger.Log($"Opening file: {filePath}");
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var javaFile = new AndroidJavaObject("java.io.File", filePath);
                var authority = Application.identifier + ".fileprovider";
                var contentUri = new AndroidJavaClass("androidx.core.content.FileProvider")
                    .CallStatic<AndroidJavaObject>("getUriForFile", activity, authority, javaFile);
                var intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW");
                intent.Call<AndroidJavaObject>("setDataAndType", contentUri, GetMimeType(filePath));
                intent.Call<AndroidJavaObject>("addFlags", 0x10000001); // FLAG_ACTIVITY_NEW_TASK | FLAG_GRANT_READ_URI_PERMISSION
                activity.Call("startActivity", intent);
                Toast.Show($"Opening: {Path.GetFileName(filePath)}");
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to open file: {e.Message}");
                Toast.Show($"Cannot open file. Path copied.");
                TextEditor te = new TextEditor();
                te.text = filePath;
                te.SelectAll();
                te.Copy();
            }
#elif UNITY_IOS && !UNITY_EDITOR
            _CTOpenFile(filePath);
            Toast.Show($"Opening: {Path.GetFileName(filePath)}");
#else
            Application.OpenURL("file://" + filePath);
            Toast.Show($"Opening: {Path.GetFileName(filePath)}");
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _CTOpenFile(string filePath);
#endif

        private static string GetMimeType(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".jpg" || ext == ".jpeg") return "image/jpeg";
            if (ext == ".png") return "image/png";
            if (ext == ".gif") return "image/gif";
            if (ext == ".json") return "application/json";
            if (ext == ".txt") return "text/plain";
            if (ext == ".pdf") return "application/pdf";
            if (ext == ".mp4") return "video/mp4";
            if (ext == ".mp3") return "audio/mpeg";
            return "*/*";
        }
    }
}