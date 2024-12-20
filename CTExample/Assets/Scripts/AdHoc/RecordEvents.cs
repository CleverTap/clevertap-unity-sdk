using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;

namespace CTExample
{
    [RequireComponent(typeof(InputPanel))]
    public class RecordEvents : MonoBehaviour
    {
        private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();
        }

        void Start()
        {
            panel.SetTitle("Record Event");
            panel.SetPlaceholder("eventName/eventProperty1:value,eventProperty2:value");
            panel.SetButtonText("Record event");

            panel.OnButtonClickedEvent += OnButtonClick;
        }

        private void OnButtonClick(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            string eventInput = text;
            string[] data = eventInput.Split("/", System.StringSplitOptions.RemoveEmptyEntries);
            string eventName = data[0];

            Dictionary<string, object> eventProperties = new Dictionary<string, object>();
            if (data.Length > 1)
            {
                string[] properties = data[1].Split(",", System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in properties)
                {
                    string[] keyValue = pair.Split(":", System.StringSplitOptions.RemoveEmptyEntries);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0];
                        string stringValue = keyValue[1];
                        object value = Utils.ParseValue(stringValue);

                        eventProperties.Add(key, value);
                    }
                }
            }

            if (eventProperties.Count > 0)
            {
                CleverTap.RecordEvent(eventName, eventProperties);
                Logger.Log($"Record event: {eventName} with properties: {CleverTapSDK.Utilities.Json.Serialize(eventProperties)}");
            }
            else
            {
                CleverTap.RecordEvent(eventName);
                Logger.Log($"Record event: {eventName}");
            }
        }
    }
}