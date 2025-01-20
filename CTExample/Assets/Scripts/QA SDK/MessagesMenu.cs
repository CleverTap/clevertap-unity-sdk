using System.Collections.Generic;
using CleverTapSDK;
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
            var parent = verticalLayoutGroup.GetComponent<RectTransform>();

            string suspend = "Suspend In-apps";
            var suspendButton = Instantiate(buttonPrefab);
            suspendButton.name = suspend;
            suspendButton.transform.SetParent(parent, false);
            suspendButton.GetComponentInChildren<Text>().text = suspend;
            suspendButton.onClick.AddListener(() =>
            {
                CleverTap.SuspendInAppNotifications();
            });

            string discard = "Discard In-apps";
            var discardButton = Instantiate(buttonPrefab);
            discardButton.name = discard;
            discardButton.transform.SetParent(parent, false);
            discardButton.GetComponentInChildren<Text>().text = discard;
            discardButton.onClick.AddListener(() =>
            {
                CleverTap.DiscardInAppNotifications();
            });

            string resume = "Resume In-apps";
            var resumeButton = Instantiate(buttonPrefab);
            resumeButton.name = resume;
            resumeButton.transform.SetParent(parent, false);
            resumeButton.GetComponentInChildren<Text>().text = resume;
            resumeButton.onClick.AddListener(() =>
            {
                CleverTap.ResumeInAppNotifications();
            });

            string sync = "Sync Custom Templates";
            var syncButton = Instantiate(buttonPrefab);
            syncButton.name = sync;
            syncButton.transform.SetParent(parent, false);
            syncButton.GetComponentInChildren<Text>().text = sync;
            syncButton.onClick.AddListener(() =>
            {
                CleverTap.SyncCustomTemplates(true);
            });

            AddSeparator(parent);

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

            foreach (var message in messages)
            {
                var button = Instantiate(buttonPrefab);
                button.name = message;
                button.transform.SetParent(parent, false);
                button.GetComponentInChildren<Text>().text = message;
                button.onClick.AddListener(() =>
                {
                    CleverTap.RecordEvent(button.name);
                    Toast.Show($"Recording event: {button.name}");
                });
            }

            RefreshContentHelper.RefreshContentFitters(parent);
        }

        void AddSeparator(Transform parent)
        {
            var separator = new GameObject();
            separator.name = "separator";
            RectTransform rt = separator.AddComponent<RectTransform>();

            // Use a constant value or use the button y
            // buttonPrefab.GetComponent<RectTransform>().sizeDelta.y
            float y = 80;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, y);
            separator.transform.SetParent(parent, false);
        }
    }
}