using System;
using System.Collections.Generic;
using CleverTapSDK;
using CleverTapSDK.Constants;
using CleverTapSDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
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

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Button getUserEventLog = Instantiate(ButtonPrefab);
            getUserEventLog.name = "Get UserEventLog data";
            getUserEventLog.transform.SetParent(parent, false);
            getUserEventLog.GetComponentInChildren<Text>().text = "Get UserEventLog data";
            getUserEventLog.onClick.AddListener(GetEventLogs);
#endif
        }

        private void AddInfoValues(RectTransform parent)
        {
            GameObject sdkVersion = Instantiate(KeyValuePrefab);
            sdkVersion.name = "SDKVersion";
            KeyValue sdkVersionKV = sdkVersion.GetComponent<KeyValue>();
            sdkVersionKV.SetKey("SDK Version");
            sdkVersionKV.SetValue(CleverTapVersion.CLEVERTAP_SDK_VERSION);
            sdkVersion.transform.SetParent(parent, false);

            GameObject accountId = Instantiate(KeyValuePrefab);
            accountId.name = "AccountId";
            KeyValue accountIdKV = accountId.GetComponent<KeyValue>();
            accountIdKV.SetKey("Account Id");
            accountIdKV.SetValue(CleverTapSettingsRuntime.Instance?.CleverTapAccountId ?? "");
            accountIdKV.transform.SetParent(parent, false);

            CleverTap.OnCleverTapProfileInitializedCallback += CleverTap_OnCleverTapProfileInitializedCallback;
#if UNITY_ANDROID
            CleverTap.OnCleverTapInitCleverTapIdCallback += CleverTap_OnCleverTapInitCleverTapIdCallback;
            CleverTap.GetCleverTapID();
#endif

            CleverTapIDObject = Instantiate(KeyValuePrefab);
            CleverTapIDObject.name = "CleverTapID";
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
            SetKVValue(CleverTapIDObject, messageJson["CleverTapID"]?.ToString());
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
            Logger.Log($"Setting DOB to: {date}");
            Toast.Show($"Setting DOB to: {date}");
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
            Toast.Show("Record \"Date Support Test\" event and Charged event with dates");
        }

        private void GetEventLogs()
        {
            CleverTap.GetUserEventLog("Charged", (userEventLog) =>
            {
                Logger.Log($"Get User Event Log: {userEventLog?.ToString()}");
            });

            CleverTap.GetUserEventLogCount("Home", (count) =>
            {
                Logger.Log($"Get User Event Log Count for: \"Home\": {count}");
            });

            CleverTap.GetUserAppLaunchCount((count) =>
            {
                Logger.Log($"Get User AppLaunch Count: {count}");
            });

            CleverTap.GetUserEventLogHistory((history) =>
            {
                Logger.Log($"Get User Event Log History: \n");
                foreach (var item in history)
                {
                    var userEventLog = item.Value;
                    Logger.Log(userEventLog?.ToString());
                }
            });

            Logger.Log($"Get User Last Visit Ts: {CleverTap.GetUserLastVisitTs()}");
        }
    }
}