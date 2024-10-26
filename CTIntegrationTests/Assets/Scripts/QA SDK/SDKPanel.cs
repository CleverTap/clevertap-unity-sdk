using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class SDKPanel : MonoBehaviour
    {
        public GameObject optionsPanel;

        public GameObject messagesPanel;
        public GameObject pushPanel;

        public Button messagesButton;
        public Button pushButton;

        void Start()
        {
            messagesButton.onClick.AddListener(DidTapMessages);
            pushButton.onClick.AddListener(DidTapPush);

            Restore();
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
    }
}