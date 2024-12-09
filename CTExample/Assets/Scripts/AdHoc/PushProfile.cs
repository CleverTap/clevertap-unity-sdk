using System;
using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;

namespace CTExample
{
    [RequireComponent(typeof(InputPanel))]
    public class PushProfile : MonoBehaviour
    {
        private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();
        }

        void Start()
        {
            panel.SetTitle("Push Profile");
            panel.SetPlaceholder("profileProperty1:value,profileProperty2:value");
            panel.SetButtonText("Push Profile");

            panel.OnButtonClickedEvent += OnButtonClick;
        }

        void OnButtonClick(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            string profileInput = text;
            string[] pairs = profileInput.Split(",", StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, object> profileData = new Dictionary<string, object>();
            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string stringValue = keyValue[1];
                    object value = Utils.ParseValue(stringValue);
                    profileData.Add(key, value);
                }
            }
            CleverTap.ProfilePush(profileData);
            Logger.Log($"Profile push: {CleverTapSDK.Utilities.Json.Serialize(profileData)}");
        }
    }
}