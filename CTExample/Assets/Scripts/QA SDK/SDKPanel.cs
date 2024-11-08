using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class SDKPanel : MonoBehaviour
    {
        public GameObject optionsPanel;

        public GameObject messagesPanel;
        public GameObject pushPanel;

        public Button messagesButton;
        public Button pushButton;

        public Button consoleButton;
        public GameObject console;

        void Start()
        {
#if !(UNITY_IOS || UNITY_ANDROID) || UNITY_EDITOR
            messagesButton.interactable = false;
            pushButton.interactable = false;
#endif
            messagesButton.onClick.AddListener(DidTapMessages);
            pushButton.onClick.AddListener(DidTapPush);
            consoleButton.onClick.AddListener(DidTapConsole);

            Restore();
            RefreshContentHelper.RefreshContentFitters((RectTransform)transform);
        }

        public void Restore()
        {
            optionsPanel.SetActive(true);
            messagesPanel.SetActive(false);
            pushPanel.SetActive(false);
        }

        public void DidTapMessages()
        {
            optionsPanel.SetActive(false);
            messagesPanel.SetActive(true);
        }

        public void DidTapPush()
        {
            optionsPanel.SetActive(false);
            pushPanel.SetActive(true);
        }

        public void DidTapConsole()
        {
            console.SetActive(!console.activeInHierarchy);
            var text = consoleButton.GetComponentInChildren<Text>();
            text.text = console.activeInHierarchy ? "Hide Console" : "Show Console";
        }
    }
}