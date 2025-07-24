using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace CTExample
{
    public class MessageMediaDownloader : MonoBehaviour
    {
        public static MessageMediaDownloader Instance { get; private set; }

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
            string savePath = Path.Combine(Application.persistentDataPath, "ImageCache", fileName);

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Image download failed: " + request.error);
                image?.Invoke(null);
            }
            else
            {
                try
                {
                    Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request);
                    byte[] bytes = downloadedTexture.EncodeToJPG();

                    // Ensure directory exists before writing
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    File.WriteAllBytes(savePath, bytes);

                    image?.Invoke(downloadedTexture);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to save downloaded image: {ex.Message}");
                    image?.Invoke(null);
                }
            }

            request.Dispose();
        }

        public static string GetFileNameFromUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string filename = Path.GetFileName(uri.AbsolutePath);
                return string.IsNullOrEmpty(filename) ? "default_image.jpg" : filename;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Invalid URL format: {ex.Message}");
                return $"image_{url.GetHashCode()}.jpg";
            }
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}