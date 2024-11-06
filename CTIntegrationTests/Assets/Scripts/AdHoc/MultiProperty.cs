using System;
using System.Collections.Generic;
using System.Linq;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    [RequireComponent(typeof(InputPanel))]
    public class MultiProperty : MonoBehaviour
    {
        private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();

            panel.AddAdditionalButton("AddValues", "Add Values", OnAddValuesButtonClick);
        }

        void Start()
        {
            panel.SetTitle("Multi Property");
            panel.SetPlaceholder("propName:value1,value2");
            panel.SetButtonText("Set multi property");

            panel.OnButtonClickedEvent += OnSetValuesButtonClick;
        }

        KeyValuePair<string, List<string>> ProcessInput(string text)
        {
            string multiProps = text;
            string[] keyValues = multiProps.Split(":", StringSplitOptions.RemoveEmptyEntries);
            string key = keyValues[0];
            List<string> values = keyValues[1].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            return new KeyValuePair<string, List<string>>(key, values);
        }

        void OnAddValuesButtonClick(string text)
        {
            var keyValues = ProcessInput(text);
            CleverTap.ProfileAddMultiValuesForKey(keyValues.Key, keyValues.Value);
        }

        void OnSetValuesButtonClick(string text)
        {
            var keyValues = ProcessInput(text);
            CleverTap.ProfileSetMultiValuesForKey(keyValues.Key, keyValues.Value);
        }
    }
}