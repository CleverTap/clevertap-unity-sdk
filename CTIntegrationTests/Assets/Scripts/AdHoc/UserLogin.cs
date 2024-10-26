using System.Collections.Generic;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class UserLogin : MonoBehaviour
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
            string id = input.text;
            Dictionary<string, string> login = new Dictionary<string, string>
            {
                { "Identity", id }
            };
            CleverTap.OnUserLogin(login);
        }
    }
}