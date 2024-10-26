using System.Collections.Generic;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class RecordEvents : MonoBehaviour
    {
        private TMP_InputField input;
        private Button button;

        // Start is called before the first frame update
        void Start()
        {
            input = GetComponentInChildren<TMP_InputField>();
            button = GetComponentInChildren<Button>();

            button.onClick.AddListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            string eventInput = input.text;
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
                        eventProperties.Add(keyValue[0], keyValue[1]);
                    }
                }
            }

            if (eventProperties.Count > 0)
            {
                CleverTap.RecordEvent(eventName, eventProperties);
            }
            else
            {
                CleverTap.RecordEvent(eventName);
            }
        }
    }
}