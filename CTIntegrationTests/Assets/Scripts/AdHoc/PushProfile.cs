using System;
using System.Collections.Generic;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class PushProfile : MonoBehaviour
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
            string profileInput = input.text;
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

            int age = UnityEngine.Random.Range(20, 80);
            Dictionary<string, object> profileProperties = new Dictionary<string, object>
            {
                { "DOB", DateTime.Now.AddYears(-age) }
            };

            CleverTap.ProfilePush(profileProperties);
        }
    }
}