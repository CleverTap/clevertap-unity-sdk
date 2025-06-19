using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public sealed class MessageView : MonoBehaviour
    {
        private const string InvalidMessageString = "Invalid message data.";

        [SerializeField] private TMP_Text _titleText = null;
        [SerializeField] private TMP_Text _messageText = null;
        [SerializeField] private Button _closeButton = null;

        public void Initialize(CleverTapInboxMessage.Content messageContent)
        {
            if(messageContent == null)
            {
                _titleText.SetText(InvalidMessageString);
                return;
            }

            _titleText.SetText(messageContent.Title.Text);
            _messageText.SetText(messageContent.Message.Text);
        }

        private void OnClose()
        {
            gameObject.SetActive(false);
            _messageText.text = string.Empty;
            _titleText.text = string.Empty;
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClose);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClose);
        }
    }
}