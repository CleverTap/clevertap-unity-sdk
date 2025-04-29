using UnityEngine;
using System.Collections.Generic;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    /// <summary>
    /// Handles persistent storage of inbox messages in Unity using a preference manager.
    /// Supports saving, retrieving, deleting, and tracking read/unread status.
    /// </summary>
    internal static class UnityNativeAppInboxPersistence
    {
        #region Constants

        /// <summary>
        /// Key used to store all message IDs in the preferences.
        /// </summary>
        private const string INBOX_IDS_KEY = "INBOX_IDS_KEY";

        /// <summary>
        /// Prefix for keys storing read/unread status for each message.
        /// </summary>
        private const string READ_PREFIX = "INBOX_READ_";

        /// <summary>
        /// Prefix for keys storing message contents.
        /// </summary>
        private const string MSG_PREFIX = "INBOX_MSG_";

        #endregion

        #region Private Static Variables

        /// <summary>
        /// Reference to the Unity preference manager used for storage.
        /// </summary>
        private static UnityNativePreferenceManager _preferenceManager = null;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Initializes the persistence manager with the provided preference manager instance.
        /// Must be called before using any persistence methods.
        /// </summary>
        /// <param name="preferenceManager">The preference manager used to persist data.</param>
        public static void Initialize(UnityNativePreferenceManager preferenceManager)
        {
            _preferenceManager = preferenceManager;
        }

        /// <summary>
        /// Saves a message with the given ID and marks it as unread.
        /// Adds the message ID to the stored list if it's new.
        /// </summary>
        /// <param name="id">Unique identifier for the message.</param>
        /// <param name="message">Content of the message to be saved.</param>
        public static void SaveMessage(string id, string message)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return;
            }

            _preferenceManager.SetString(MSG_PREFIX + id, message);
            MarkAsUnread(id);
            List<string> ids = GetAllMessageIds();

            if (!ids.Contains(id))
            {
                ids.Add(id);
                SaveAllMessageIds(ids);
            }
        }

        /// <summary>
        /// Retrieves the message content for the given ID.
        /// </summary>
        /// <param name="id">The ID of the message to retrieve.</param>
        /// <returns>The message content, or null if not found or not initialized.</returns>
        public static string GetMessage(string id)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return null;
            }

            return _preferenceManager.GetString(MSG_PREFIX + id, string.Empty);
        }

        /// <summary>
        /// Marks a message as read using the given ID.
        /// </summary>
        /// <param name="id">The ID of the message to mark as read.</param>
        public static void MarkAsRead(string id)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return;
            }

            _preferenceManager.SetInt(READ_PREFIX + id, 1);
        }

        /// <summary>
        /// Marks a message as unread using the given ID.
        /// </summary>
        /// <param name="id">The ID of the message to mark as unread.</param>
        public static void MarkAsUnread(string id)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return;
            }

            _preferenceManager.SetInt(READ_PREFIX + id, 0);
        }

        /// <summary>
        /// Checks whether a message is marked as read.
        /// </summary>
        /// <param name="id">The ID of the message to check.</param>
        /// <returns>True if the message is read, false otherwise.</returns>
        public static bool IsRead(string id)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return false;
            }

            return _preferenceManager.GetInt(READ_PREFIX + id, 0) == 1;
        }

        /// <summary>
        /// Deletes a message and its read status, and removes the ID from the stored list.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        public static void DeleteMessage(string id)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return;
            }

            _preferenceManager.DeleteKey(MSG_PREFIX + id);
            _preferenceManager.DeleteKey(READ_PREFIX + id);

            var ids = GetAllMessageIds();
            if (ids.Remove(id))
            {
                SaveAllMessageIds(ids);
            }

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Retrieves all stored messages.
        /// </summary>
        /// <returns>A dictionary containing all messages keyed by their IDs.</returns>
        public static Dictionary<string, string> GetAllMessages()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            List<string> ids = GetAllMessageIds();

            for (int i = 0, count = ids.Count; i < count; i++)
            {
                string id = ids[i];
                string msg = GetMessage(id);

                if (!string.IsNullOrEmpty(msg))
                {
                    dict[id] = msg;
                }
            }

            return dict;
        }

        /// <summary>
        /// Marks all stored messages as read.
        /// </summary>
        public static void MarkAllAsRead()
        {
            List<string> ids = GetAllMessageIds();

            for (int i = 0, count = ids.Count; i < count; i++)
            {
                MarkAsRead(ids[i]);
            }
        }

        /// <summary>
        /// Deletes all stored messages and their metadata (IDs and read flags).
        /// </summary>
        public static void DeleteAllMessages()
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return;
            }

            List<string> ids = GetAllMessageIds();

            for (int i = 0, count = ids.Count; i < count; i++)
            {
                string id = ids[i];
                _preferenceManager.DeleteKey(MSG_PREFIX + id);
                _preferenceManager.DeleteKey(READ_PREFIX + id);
            }

            _preferenceManager.DeleteKey(INBOX_IDS_KEY);
        }

        /// <summary>
        /// Retrieves only the unread messages.
        /// </summary>
        /// <returns>A dictionary of unread messages keyed by their IDs.</returns>
        public static Dictionary<string, string> GetUnreadMessages()
        {
            var allMessages = GetAllMessages();
            var unreadMessages = new Dictionary<string, string>();

            foreach (var kvp in allMessages)
            {
                if (!IsRead(kvp.Key))
                {
                    unreadMessages[kvp.Key] = kvp.Value;
                }
            }

            return unreadMessages;
        }

        /// <summary>
        /// Determines if the new message is updated or new by comparing it with the stored version.
        /// </summary>
        /// <param name="id">The message ID to check.</param>
        /// <param name="newMessage">The new message content.</param>
        /// <returns>True if the message is new or updated; false if it matches the stored version.</returns>
        public static bool IsUpdatedOrNewMessage(string id, string newMessage)
        {
            string existingMessage = GetMessage(id);
            return existingMessage != newMessage;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Retrieves a list of all stored message IDs.
        /// </summary>
        /// <returns>A list of message IDs.</returns>
        private static List<string> GetAllMessageIds()
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return null;
            }

            string joined = _preferenceManager.GetString(INBOX_IDS_KEY, string.Empty);

            if (string.IsNullOrEmpty(joined))
            {
                return new List<string>();
            }

            return new List<string>(joined.Split(','));
        }

        /// <summary>
        /// Stores the given list of message IDs as a comma-separated string.
        /// </summary>
        /// <param name="ids">List of message IDs to store.</param>
        private static void SaveAllMessageIds(List<string> ids)
        {
            if (_preferenceManager == null)
            {
                CleverTapLogger.LogError("UnityNativeInboxPersistence is not initialized. Call Initialize() first.");
                return;
            }

            string joined = string.Join(",", ids);
            _preferenceManager.SetString(INBOX_IDS_KEY, joined);
        }

        #endregion
    }
}