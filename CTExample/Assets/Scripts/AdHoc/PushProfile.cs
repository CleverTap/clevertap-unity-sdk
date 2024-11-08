using System;
using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
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
            string profileInput = text;
            string[] pairs = profileInput.Split(",", StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> profileData = new Dictionary<string, string>();
            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (keyValue.Length == 2)
                {
                    profileData.Add(keyValue[0], keyValue[1]);
                }
            }
            CleverTap.ProfilePush(profileData);
            Logger.Log($"Profile push: {CleverTapSDK.Utilities.Json.Serialize(profileData)}");
        }
    }
}