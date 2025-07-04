using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace CTExample
{
    public class DisplayUnitMediaDownloader : MonoBehaviour
    {
        public static DisplayUnitMediaDownloader Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void GetImage(string url, Action<Texture2D> onImage)
        {
            string fileName = GetFileNameFromUrl(url);
            string savePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(savePath))
            {
                byte[] imageData = File.ReadAllBytes(savePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                onImage?.Invoke(texture);
            }
            else
            {
                StartCoroutine(DownloadImage(url, onImage));
            }
        }

        private IEnumerator DownloadImage(string url, Action<Texture2D> image)
        {
            string fileName = GetFileNameFromUrl(url);
            string savePath = Path.Combine(Application.streamingAssetsPath, fileName);

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Image download failed: " + request.error);
            }
            else
            {
                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request);
                byte[] bytes = downloadedTexture.EncodeToJPG();
                File.WriteAllBytes(savePath, bytes);
                image?.Invoke(downloadedTexture);
            }
        }

        public static string GetFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string filename = Path.GetFileName(uri.AbsolutePath);
            return filename;
        }
    }
}