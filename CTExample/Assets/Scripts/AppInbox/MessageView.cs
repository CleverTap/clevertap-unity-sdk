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

        public void Initialize(CleverTapInboxMessage message)
        {
            if(message?.Message?.Content == null && message.Message.Content.Count == 0)
            {
                _titleText.SetText(InvalidMessageString);
                return;
            }

            _titleText.SetText(message.Message.Content[0].Title.Text);
            _messageText.SetText(message.Message.Content[0].Message.Text);
            CleverTap.RecordInboxNotificationViewedEventForID(message.Id);
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