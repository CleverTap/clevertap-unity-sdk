using System;
using UnityEngine;
using CleverTapSDK;

namespace CTIntegrationTests
{
    [RequireComponent(typeof(InputPanel))]
    public class IncrementDecrementProperty : MonoBehaviour
    {
        public bool shouldDecrement;

		private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();
        }

        void Start()
        {
            panel.SetTitle(shouldDecrement ? "Decrement property" : "Increment property");
            panel.SetPlaceholder("propertyName:value");
            panel.SetButtonText(shouldDecrement ? "Decrement" : "Increment");

            panel.OnButtonClickedEvent += OnButtonClick;
        }

        private void OnButtonClick(string text)
        {
            Debug.Log($"[SAMPLE] Panel_OnButtonClickedEvent: {text}");

            string[] keyValue = text.Split(":", StringSplitOptions.RemoveEmptyEntries);
            if (keyValue.Length == 2)
            {
                string key = keyValue[0];
                if (int.TryParse(keyValue[1], out int number)) {
                    if (shouldDecrement)
                    {
                        CleverTap.ProfileDecrementValueForKey(key, number);
                    }
                    else
                    {
                        CleverTap.ProfileIncrementValueForKey(key, number);
                    }
                }
                else if (double.TryParse(keyValue[1], out double d))
                {
                    if (shouldDecrement)
                    {
                        CleverTap.ProfileDecrementValueForKey(key, d);
                    }
                    else
                    {
                        CleverTap.ProfileIncrementValueForKey(key, d);
                    }
                }
            }
        }
    }
}