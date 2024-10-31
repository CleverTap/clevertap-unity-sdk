using System.Collections.Generic;
using CleverTapSDK;
using CleverTapSDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    internal delegate void ButtonAction(Button button);

    internal class ButtonActionModel
    {
        internal string Name { get; set; }
        internal string Tag { get; set; }
        internal ButtonAction Action { get; set; }

        internal ButtonActionModel(string name, ButtonAction action) : this(name, null, action)
        {
        }

        internal ButtonActionModel(string name, string tag, ButtonAction action)
        {
            Name = name;
            Tag = tag;
            Action = action;
        }
    }

    public class AppInbox : MonoBehaviour
    {
        public Button buttonPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;

        private bool hasInboxInitialized = false;
        private bool shouldShowInbox = false;

        void Start()
        {
            CleverTap.OnCleverTapInboxDidInitializeCallback += CleverTap_OnCleverTapInboxDidInitializeCallback;
            CreateButtonActions();
        }

        private void CreateButtonActions()
        {
            var models = new List<ButtonActionModel>
            {
                new ButtonActionModel("Initialize", (button) => CleverTap.InitializeInbox()),
                new ButtonActionModel("Show Inbox", (button) => ShowInbox())
            };

            var parent = verticalLayoutGroup.GetComponent<RectTransform>();

            foreach (var model in models)
            {
                var button = Instantiate(buttonPrefab);
                button.name = model.Name;
                button.transform.SetParent(parent, false);
                button.GetComponentInChildren<Text>().text = model.Name;
                if (!string.IsNullOrEmpty(model.Tag))
                {
                    button.gameObject.tag = model.Tag;
                }
                button.onClick.AddListener(() =>
                {
                    model.Action.Invoke(button);
                });
            }
        }

        private void ShowInbox()
        {
            if (!hasInboxInitialized)
            {
                Logger.LogWarning("Inbox not initialized.");
                shouldShowInbox = true;
                return;
            }

            Dictionary<string, object> styleConfig = new Dictionary<string, object>
            {
                { "navBarTitle", "My App Inbox" },
                { "navBarTitleColor", "#FF0000" },
                { "navBarColor", "#FFFFFF" },
                { "inboxBackgroundColor", "#AED6F1" },
                { "backButtonColor", "#00FF00" },
                { "unselectedTabColor", "#0000FF" },
                { "selectedTabColor", "#FF0000" },
                { "noMessageText", "No message(s)" },
                { "noMessageTextColor", "#FF0000" }
            };

            // Convert the Dictionary parameters to a string and pass it to `ShowAppInbox()`
            string jsonStr = Json.Serialize(styleConfig);
            CleverTap.ShowAppInbox(jsonStr);
        }

        private void CleverTap_OnCleverTapInboxDidInitializeCallback()
        {
            hasInboxInitialized = true;
            if (shouldShowInbox)
            {
                ShowInbox();
                shouldShowInbox = false;
            }
        }

        public void Restore()
        {

        }
    }
}