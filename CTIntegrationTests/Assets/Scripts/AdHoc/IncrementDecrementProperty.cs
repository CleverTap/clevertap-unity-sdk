using System;
using UnityEngine;
using CleverTapSDK;

namespace CTIntegrationTests
{
    [RequireComponent(typeof(InputPanel))]
    public class IncrementDecrementProperty : MonoBehaviour
    {
		private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();

            panel.AddAdditionalButton("Decrement", "Decrement", OnDecrementButtonClick);
        }

        void Start()
        {
            panel.SetTitle("Increment/Decrement property");
            panel.SetPlaceholder("propertyName:value");
            panel.SetButtonText("Increment");

            panel.OnButtonClickedEvent += OnIncrementButtonClick;
        }

        private void OnDecrementButtonClick(string text)
        {
            string[] keyValue = text.Split(":", StringSplitOptions.RemoveEmptyEntries);
            if (keyValue.Length == 2)
            {
                string key = keyValue[0];
                if (int.TryParse(keyValue[1], out int number))
                {
                    CleverTap.ProfileDecrementValueForKey(key, number);
                }
                else if (double.TryParse(keyValue[1], out double d))
                {
                    CleverTap.ProfileDecrementValueForKey(key, d);
                }
            }
        }

        private void OnIncrementButtonClick(string text)
        {
            string[] keyValue = text.Split(":", StringSplitOptions.RemoveEmptyEntries);
            if (keyValue.Length == 2)
            {
                string key = keyValue[0];
                if (int.TryParse(keyValue[1], out int number))
                {
                    CleverTap.ProfileIncrementValueForKey(key, number);
                }
                else if (double.TryParse(keyValue[1], out double d))
                {
                    CleverTap.ProfileIncrementValueForKey(key, d);
                }
            }
        }
    }
}