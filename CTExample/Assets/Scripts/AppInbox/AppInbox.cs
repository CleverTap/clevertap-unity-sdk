using System.Collections.Generic;
using CleverTapSDK;
using CleverTapSDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class AppInbox : MonoBehaviour
    {
        public Button buttonPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;
        [SerializeField] private GameObject inboxPanel;
        private bool hasInboxInitialized = false;
        private bool shouldShowInbox = false;

        private void Awake()
        {
            CleverTap.OnCleverTapInboxDidInitializeCallback += CleverTap_OnCleverTapInboxDidInitializeCallback;
            CreateButtonActions();
        }

        private void CreateButtonActions()
        {
            var models = new List<ButtonActionModel>
            {
                new ButtonActionModel("Initialize", (button) => CleverTap.InitializeInbox()),
            #if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
                // This button is only available on Android and iOS
                new ButtonActionModel("Show Inbox", (button) => ShowInbox()),
            #elif UNITY_EDITOR
                new ButtonActionModel("Show Unity App Inbox", (button) => ShowUnityAppInbox())
            #endif
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
            Logger.Log($"Showing app inbox with config: {jsonStr}");
        }

        private void ShowUnityAppInbox()
        {
            if (!hasInboxInitialized)
            {
                Logger.LogWarning("Inbox not initialized.");
                return;
            }

            inboxPanel.SetActive(true);
        }

        private void CleverTap_OnCleverTapInboxDidInitializeCallback()
        {
            hasInboxInitialized = true;

            if (shouldShowInbox)
            {
                ShowInbox();
                shouldShowInbox = false;
            }
            Toast.Show("On CleverTap Inbox Initialize");
        }

        public void Restore()
        {
            if (inboxPanel.activeInHierarchy)
            {
                inboxPanel.SetActive(false);
            }
        }

        private void OnDisable()
        {
            inboxPanel.SetActive(false);
        }
    }
}