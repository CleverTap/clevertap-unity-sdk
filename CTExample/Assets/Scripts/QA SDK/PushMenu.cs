using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class PushMenu : MonoBehaviour
    {
        public Button buttonPrefab;
        public VerticalLayoutGroup verticalLayoutGroup;

        void Start()
        {
            var pushEvents = new List<string>(new string[] {
                "BasicTemplate_NotificationSent",
                "CarouselTemplate_NotificationSent",
                "TimerTemplate_NotificationSent",
                "CutomBasicTemplate_NotificationSent",
                "CustomAutoCarouselTemplate_NotificationSent",
            });

            var parent = verticalLayoutGroup.GetComponent<RectTransform>();

            AddRegisterForIOSPushButton(parent);

            foreach (var e in pushEvents)
            {
                var button = Instantiate(buttonPrefab);
                button.name = e;
                button.transform.SetParent(parent, false);
                button.GetComponentInChildren<Text>().text = e;
                button.onClick.AddListener(() =>
                {
                    CleverTap.RecordEvent(button.name);
                    Toast.Show($"Recording event: {button.name}");
                });
            }

            RefreshContentHelper.RefreshContentFitters(parent);
        }

        void AddRegisterForIOSPushButton(Transform parent)
        {
            var button = Instantiate(buttonPrefab);
            button.name = "Prompt For Push Permission";
            button.transform.SetParent(parent, false);
            button.GetComponentInChildren<Text>().text = "Prompt For Push Permission";
            button.onClick.AddListener(() =>
            {
                CleverTap.PromptForPushPermission(true);
            });
        }
    }
}