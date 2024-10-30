using UnityEngine;
using CleverTapSDK;
using CleverTapSDK.Constants;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using CleverTapSDK.Utilities;
using System.Collections;

namespace CTIntegrationTests
{
    public class AdHoc : MonoBehaviour
    {
        public GameObject ContentView;
        public GameObject KeyValuePrefab;
        public Button ButtonPrefab;

        private GameObject CleverTapIDObject;

        void Start()
        {
            RectTransform parent = ContentView.GetComponent<RectTransform>();

            GameObject sdkVersion = Instantiate(KeyValuePrefab);
            KeyValue sdkVersionKV = sdkVersion.GetComponent<KeyValue>();
            sdkVersionKV.SetKey("SDK Version");
            sdkVersionKV.SetValue(CleverTapVersion.CLEVERTAP_SDK_VERSION);
            sdkVersion.transform.SetParent(parent, false);
            sdkVersion.transform.SetSiblingIndex(0);

            CleverTap.OnCleverTapProfileInitializedCallback += CleverTap_OnCleverTapProfileInitializedCallback;
#if UNITY_ANDROID
            CleverTap.OnCleverTapInitCleverTapIdCallback += CleverTap_OnCleverTapInitCleverTapIdCallback;
            CleverTap.GetCleverTapID();
#endif

            CleverTapIDObject = Instantiate(KeyValuePrefab);
            KeyValue ctidKV = CleverTapIDObject.GetComponent<KeyValue>();
            ctidKV.SetKey("CleverTap ID");
            ctidKV.SetValue(CleverTap.ProfileGetCleverTapID());
            CleverTapIDObject.transform.SetParent(parent, false);
            CleverTapIDObject.transform.SetSiblingIndex(1);

            Button DOB = Instantiate(ButtonPrefab);
            DOB.name = "DOB";
            DOB.transform.SetParent(parent, false);
            DOB.GetComponentInChildren<Text>().text = "Push DOB";
            DOB.onClick.AddListener(() =>
            {
                int age = UnityEngine.Random.Range(20, 80);
                DateTime date = DateTime.Now.AddYears(-age);
                Debug.Log($"[SAMPLE] Setting DOB to: {date}");
                Dictionary<string, object> profileProperties = new Dictionary<string, object>
                {
                    { "DOB", date }
                };

                CleverTap.ProfilePush(profileProperties);
            });
        }

        private void CleverTap_OnCleverTapProfileInitializedCallback(string message)
        {
            Debug.Log($"[SAMPLE] OnCleverTapProfileInitializedCallback: {message}");
            var messageJson = Json.Deserialize(message) as Dictionary<string, object>;
            SetKVValue(CleverTapIDObject, messageJson["CleverTapID"]?.ToString());
        }

#if UNITY_ANDROID
        private void CleverTap_OnCleverTapInitCleverTapIdCallback(string message)
        {
            Debug.Log($"[SAMPLE] OnCleverTapInitCleverTapIdCallback: {message}");
            var messageJson = Json.Deserialize(message) as Dictionary<string, object>;
            SetKVValue(CleverTapIDObject, messageJson["cleverTapID"]?.ToString());
        }
#endif

        private void SetKVValue(GameObject kvObject, string value)
        {
            if (kvObject == null) return;
            KeyValue kv = kvObject.GetComponent<KeyValue>();
            kv.SetValue(value);
        }
    }
}