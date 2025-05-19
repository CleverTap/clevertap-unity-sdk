using System.Collections.Generic;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public sealed class Inbox : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] private TMP_Text _inboxMessagesCountText = null;
        [SerializeField] private TMP_Text _inboxUnreadMessagesCountText = null;

        [Header("Messages")]
        [Space, SerializeField] private MessageItemPool _messageItemPool = null;
        [SerializeField] private TMP_Text _inboxStatus = null;
        [SerializeField] private MessageView _messageView = null;

        [Header("Footer")]
        [Space, SerializeField] private Button _messageButton = null;
        [SerializeField] private Button _unreadButton = null;

        private Dictionary<string, MessageItem> _messageItemRegistry = new Dictionary<string, MessageItem>();

        private void OnEnable()
        {
            _messageButton.onClick.AddListener(OnMessageButtonClicked);
            _unreadButton.onClick.AddListener(OnUnreadButtonClicked);
            SpawnMessageItems(CleverTap.GetAllInboxMessagesParsed());
            CleverTap.OnCleverTapInboxMessagesDidUpdateCallback += CleverTap_OnCleverTapInboxMessagesDidUpdateCallback;
            UpdateHeader();
        }

        private void OnDisable()
        {
            _messageButton.onClick.RemoveListener(OnMessageButtonClicked);
            _unreadButton.onClick.RemoveListener(OnUnreadButtonClicked);
            CleverTap.OnCleverTapInboxMessagesDidUpdateCallback -= CleverTap_OnCleverTapInboxMessagesDidUpdateCallback;
            _messageView.gameObject.SetActive(false);
        }

        private void CleverTap_OnCleverTapInboxMessagesDidUpdateCallback()
        {
            UpdateHeader();
        }

        private void OnMessageButtonClicked()
        {
            SpawnMessageItems(CleverTap.GetAllInboxMessagesParsed());
        }

        private void OnUnreadButtonClicked()
        {
            SpawnMessageItems(CleverTap.GetUnreadInboxMessagesParsed());
        }

        private void SpawnMessageItems(List<CleverTapInboxMessage> messages)
        {
            _messageItemPool.ReleaseAll();
            
            if (messages == null || messages.Count == 0)
            {
                UpdateStatusMessage("No Messages");
                return;
            }
            else
            {
                UpdateStatusMessage(string.Empty);
            }

            foreach (CleverTapInboxMessage message in messages)
            {
                SpawnMessageItem(message);
            }
        }

        private void SpawnMessageItem(CleverTapInboxMessage message)
        {
            MessageItem messageItem = _messageItemPool.Get();
            messageItem.Initialize(_messageView, message, OnMessageRead, OnMessageDelete);
            _messageItemRegistry[message.Id] = messageItem;
        }

        private void UpdateStatusMessage(string message)
        {
            if (_inboxStatus != null)
                _inboxStatus.text = message;
        }

        private void OnMessageRead(string messageId)
        {
            CleverTap.MarkReadInboxMessageForID(messageId);
        }

        private void OnMessageDelete(string messageId)
        {
            CleverTap.DeleteInboxMessageForID(messageId);
            _messageItemPool.ReturnToPool(_messageItemRegistry[messageId]);
            _messageItemRegistry.Remove(messageId);

            if (_messageItemRegistry.Count == 0)
            { 
                UpdateStatusMessage("No Messages");
            }
        }

        private void UpdateHeader()
        {
            int totalMessages = CleverTap.GetInboxMessageCount();
            int unreadMessages = CleverTap.GetInboxMessageUnreadCount();

            if (_inboxMessagesCountText != null)
                _inboxMessagesCountText.text = $"Total Messages: {totalMessages}";

            if (_inboxUnreadMessagesCountText != null)
                _inboxUnreadMessagesCountText.text = $"Unread Messages: {unreadMessages}";
        }
    }
}