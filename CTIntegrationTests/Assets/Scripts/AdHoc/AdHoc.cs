using System;
using System.Collections.Generic;
using CleverTapSDK;
using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class AdHoc : MonoBehaviour
    {
        [SerializeField] private GameObject ContentView;
        [SerializeField] private GameObject KeyValuePrefab;
        [SerializeField] private Button ButtonPrefab;
        [SerializeField] private GameObject InputPanelPrefab;

        private GameObject CleverTapIDObject;

        void Start()
        {
            RectTransform parent = ContentView.GetComponent<RectTransform>();

            AddInfoValues(parent);

            GameObject recordEvent = Instantiate(InputPanelPrefab);
            recordEvent.name = "RecordEvents";
            recordEvent.transform.SetParent(parent, false);
            recordEvent.AddComponent<RecordEvents>();

            GameObject profilePush = Instantiate(InputPanelPrefab);
            profilePush.name = "PushProfile";
            profilePush.transform.SetParent(parent, false);
            profilePush.AddComponent<PushProfile>();

            GameObject userLogin = Instantiate(InputPanelPrefab);
            userLogin.name = "UserLogin";
            userLogin.transform.SetParent(parent, false);
            userLogin.AddComponent<UserLogin>();

            Button recordEventsWithDates = Instantiate(ButtonPrefab);
            recordEventsWithDates.name = "RecordEventsWithDates";
            recordEventsWithDates.transform.SetParent(parent, false);
            recordEventsWithDates.GetComponentInChildren<Text>().text = "Record Events With Dates";
            recordEventsWithDates.onClick.AddListener(RecordEventsWithDates);

            Button DOB = Instantiate(ButtonPrefab);
            DOB.name = "DOB";
            DOB.transform.SetParent(parent, false);
            DOB.GetComponentInChildren<Text>().text = "Push DOB";
            DOB.onClick.AddListener(SetDOB);

            GameObject removeProp = Instantiate(InputPanelPrefab);
            removeProp.name = "RemoveProperty";
            removeProp.transform.SetParent(parent, false);
            removeProp.AddComponent<RemoveProperty>();

            GameObject multiProp = Instantiate(InputPanelPrefab);
            multiProp.name = "MultiProperty";
            multiProp.transform.SetParent(parent, false);
            multiProp.AddComponent<MultiProperty>();

            GameObject incrementDecrement = Instantiate(InputPanelPrefab);
            incrementDecrement.name = "Increment/Decrement Property";
            incrementDecrement.transform.SetParent(parent, false);
            incrementDecrement.AddComponent<IncrementDecrementProperty>();
        }

        private void AddInfoValues(RectTransform parent)
        {
            GameObject sdkVersion = Instantiate(KeyValuePrefab);
            KeyValue sdkVersionKV = sdkVersion.GetComponent<KeyValue>();
            sdkVersionKV.SetKey("SDK Version");
            sdkVersionKV.SetValue(CleverTapVersion.CLEVERTAP_SDK_VERSION);
            sdkVersion.transform.SetParent(parent, false);

            App app = FindObjectOfType<App>();
            GameObject accountId = Instantiate(KeyValuePrefab);
            KeyValue accountIdKV = accountId.GetComponent<KeyValue>();
            accountIdKV.SetKey("Account Id");
            accountIdKV.SetValue(app.accountId);
            accountIdKV.transform.SetParent(parent, false);

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
        }


        private void CleverTap_OnCleverTapProfileInitializedCallback(string message)
        {
            Logger.Log($"OnCleverTapProfileInitializedCallback: {message}");
            var messageJson = Json.Deserialize(message) as Dictionary<string, object>;
            SetKVValue(CleverTapIDObject, messageJson["CleverTapID"]?.ToString());
        }

#if UNITY_ANDROID
        private void CleverTap_OnCleverTapInitCleverTapIdCallback(string message)
        {
            Logger.Log($"OnCleverTapInitCleverTapIdCallback: {message}");
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

        private void SetDOB()
        {
            int age = UnityEngine.Random.Range(20, 80);
            DateTime date = DateTime.Now.AddYears(-age);
            Debug.Log($"[SAMPLE] Setting DOB to: {date}");
            Dictionary<string, object> profileProperties = new Dictionary<string, object>
                {
                    { "DOB", date }
                };

            CleverTap.ProfilePush(profileProperties);
        }

        private void RecordEventsWithDates()
        {
            // Record Event with date
            CleverTap.RecordEvent("Date Support Test", new Dictionary<string, object>() {
                { "Date", new DateTime(2000, 01, 01) },
                { "DateNow", DateTime.Now },
                { "DateUtcNow", DateTime.UtcNow }
            });

            // Record charged event with date
            var chargeDetails = new Dictionary<string, object>(){
                { "Amount", 500 },
                { "Currency", "USD" },
                { "Payment Mode", "Credit card" },
                { "Date", new DateTime(2024, 01, 25) }
            };
            var items = new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "Price", 50 },
                    { "Product category", "books" },
                    { "Quantity", 1 }
                },
                new Dictionary<string, object> {
                    { "Price", 100 },
                    { "Product category", "plants" },
                    { "Quantity", 10 }
                }
            };
            CleverTap.RecordChargedEventWithDetailsAndItems(chargeDetails, items);
        }
    }
}