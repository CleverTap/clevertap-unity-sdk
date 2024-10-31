using System;
using System.Collections.Generic;
using System.Linq;
using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
{
    [RequireComponent(typeof(InputPanel))]
    public class MultiProperty : MonoBehaviour
    {
        public bool useAddValues;

        private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();
        }

        void Start()
        {
            panel.SetTitle("Multi Property");
            panel.SetPlaceholder("propName:value1,value2");
            panel.SetButtonText(useAddValues ? "Add values" : "Set multi property");

            panel.OnButtonClickedEvent += OnButtonClick;
        }

        void OnButtonClick(string text)
        {
            string multiProps = text;
            string[] keyValues = multiProps.Split(":", StringSplitOptions.RemoveEmptyEntries);
            string key = keyValues[0];
            List<string> values = keyValues[1].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!useAddValues)
            {
                CleverTap.ProfileSetMultiValuesForKey(key, values);
            }
            else
            {
                CleverTap.ProfileAddMultiValuesForKey(key, values);
            }
        }
    }
}