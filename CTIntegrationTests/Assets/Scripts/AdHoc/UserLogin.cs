using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;

namespace CTIntegrationTests
{
    [RequireComponent(typeof(InputPanel))]
    public class UserLogin : MonoBehaviour
    {
        private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();
        }

        void Start()
        {
            panel.SetTitle("User Login");
            panel.SetPlaceholder("Identity");
            panel.SetButtonText("On User Login");

            panel.OnButtonClickedEvent += OnButtonClick;
        }

        void OnButtonClick(string text)
        {
            string id = text;
            Dictionary<string, string> login = new Dictionary<string, string>
            {
                { "Identity", id }
            };
            CleverTap.OnUserLogin(login);
            Logger.Log($"User login: {id}");
        }
    }
}