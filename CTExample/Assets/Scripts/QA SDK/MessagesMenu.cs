using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class MessagesMenu : MonoBehaviour
    {
        public Button buttonPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;

        // Use this for initialization
        void Start()
        {
            var messages = new List<string>(new string[] {
                "Cover",
                "Interstitial",
                "Half Interstitial",
                "Header",
                "Footer",
                "Alert",
                "Cover Image",
                "Interstitial Image",
                "Half Interstitial Image",
                "Custom HTML"
            });

            var parent = verticalLayoutGroup.GetComponent<RectTransform>();

            foreach (var message in messages)
            {
                var button = Instantiate(buttonPrefab);
                button.name = message;
                button.transform.SetParent(parent, false);
                button.GetComponentInChildren<Text>().text = message;
                button.onClick.AddListener(() =>
                {
                    CleverTapSDK.CleverTap.RecordEvent(button.name);
                    Toast.Show($"Recording event: {button.name}");
                });
            }

            RefreshContentHelper.RefreshContentFitters(parent);
        }
    }
}