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
        private static readonly object _lock = new object();

        private void Awake()
        {
            lock (_lock)
            {
                if (Instance == null)
                    Instance = this;
                else if (Instance != this)
                    Destroy(gameObject);
            }
        }

        public void GetImage(string url, Action<Texture2D> onImage)
        {
            string fileName = GetFileNameFromUrl(url);
            string streamingAssetsPath = Application.streamingAssetsPath;

            if (!Directory.Exists(streamingAssetsPath))
                Directory.CreateDirectory(streamingAssetsPath);

            string savePath = Path.Combine(streamingAssetsPath, fileName);

            if (File.Exists(savePath))
            {
                byte[] imageData = File.ReadAllBytes(savePath);
                Texture2D texture = new Texture2D(1, 1);

                if (texture.LoadImage(imageData))
                {
                    onImage?.Invoke(texture);
                }
                else
                {
                    Debug.LogError($"Failed to load cached image: {fileName}");
                    StartCoroutine(DownloadImage(url, onImage));
                }
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
            try
            {
                Uri uri = new Uri(url);
                string filename = Path.GetFileName(uri.AbsolutePath);
                return string.IsNullOrEmpty(filename) ? "cached_image" : filename;
            }
            catch (UriFormatException ex)
            {
                Debug.LogError($"Invalid URL format: {url}, Error: {ex.Message}");
                return url.GetHashCode().ToString(); // Use hash as fallback filename
            }
        }
    }
}