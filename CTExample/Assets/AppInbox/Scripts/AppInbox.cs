using System.Collections.Generic;
using CleverTapSDK;
using UnityEngine;

namespace CTExample
{
    public sealed class AppInbox : MonoBehaviour
    {
        private static bool _hasInboxInitialized = false;
        private List<CleverTapInboxMessage> _inboxMessages = new List<CleverTapInboxMessage>();
        private List<CleverTapInboxMessage> _unreadMessages = new List<CleverTapInboxMessage>();

        private void OnEnable()
        {
            if (!_hasInboxInitialized)
            {
                Logger.Log("Initializing CleverTap Inbox");
                CleverTap.InitializeInbox();
                CleverTap.OnCleverTapInboxDidInitializeCallback += OnInboxInitialize;
                CleverTap.OnCleverTapInboxMessagesDidUpdateCallback += OnInboxMessageUpdate;
            }
        }

        private void OnInboxInitialize()
        {
            _hasInboxInitialized = true;
            _inboxMessages = CleverTap.GetAllInboxMessagesParsed();
            _unreadMessages = CleverTap.GetUnreadInboxMessagesParsed();
            Logger.Log($"Inbox Initialized with {_inboxMessages.Count} messages, {_unreadMessages.Count} unread messages.");
            Toast.Show("CleverTap Inbox Initialized");
        }

        private void OnInboxMessageUpdate()
        {
            Toast.Show("Inbox Message Updaterd");
        }

        public void ReadMessage(string messageId)
        {
            CleverTap.MarkReadInboxMessageForID(messageId);
            Logger.Log($"Message Read ID: {messageId}");
        }

        public void ReadBulkMessages(string[] messageIds)
        {
            CleverTap.MarkReadInboxMessagesForIDs(messageIds);
            Logger.Log($"Read messages count : {messageIds.Length}");
        }

        public void DeleteMessage(string messageId)
        {
            CleverTap.DeleteInboxMessageForID(messageId);
            Logger.Log($"Deleted message with ID: {messageId}");
        }

        public void DeleteBulkMessages(string[] messageIds)
        {
            CleverTap.DeleteInboxMessagesForIDs(messageIds);
            Logger.Log($"Deleted messages count : {messageIds.Length}");
        }
    }
}