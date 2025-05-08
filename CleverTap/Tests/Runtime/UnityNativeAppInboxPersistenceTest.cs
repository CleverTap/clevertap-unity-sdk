using NUnit.Framework;
using CleverTapSDK.Native;
using System.Collections.Generic;
using CleverTapSDK.Utilities;

public class UnityNativeAppInboxPersistenceTests
{
    private UnityNativePreferenceManager _prefManager;
    private string _accountId = "test_account";
    private string _deviceId = "test_device";

    [SetUp]
    public void Setup()
    {
        _prefManager = new UnityNativePreferenceManager(_accountId);
        UnityNativeAppInboxPersistence.Initialize(_prefManager, _deviceId);
    }

    [Test]
    public void SaveAndRetrieveMessage_WorksCorrectly()
    {
        Dictionary<string, object> messageDict = new Dictionary<string, object>
        {
            {"_id", 1},
            { "isRead", false }
        };

        string messageId = messageDict["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(messageId, Json.Serialize(messageDict));
        string message = UnityNativeAppInboxPersistence.GetMessage(messageId);
        Assert.AreEqual(Json.Serialize(messageDict), message);
    }

    [Test]
    public void MarkAsReadAndCheckStatus_WorksCorrectly()
    {
        Dictionary<string, object> message = new Dictionary<string, object>
        {
            {"_id", 2},
            { "isRead", false }
        };

        string messageId = message["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(messageId, Json.Serialize(message));
        UnityNativeAppInboxPersistence.MarkAsRead(messageId);
        Assert.IsTrue(UnityNativeAppInboxPersistence.IsRead(messageId));
    }


    [Test]
    public void MarkMessagesAsReadForIds_MarksOnlySpecifiedMessagesAsRead()
    {
        Dictionary<string, object> message1 = new Dictionary<string, object>
        {
            {"_id", 3},
            { "isRead", false }
        };

        Dictionary<string, object> message2 = new Dictionary<string, object>
        {
            {"_id", 4},
            { "isRead", false }
        };

        Dictionary<string, object> message3 = new Dictionary<string, object>
        {
            {"_id", 5},
            { "isRead", false }
        };

        string message1Id = message1["_id"].ToString();
        string message2Id = message2["_id"].ToString();
        string message3Id = message3["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(message1Id, Json.Serialize(message1));
        UnityNativeAppInboxPersistence.SaveMessage(message2Id, Json.Serialize(message2));
        UnityNativeAppInboxPersistence.SaveMessage(message3Id, Json.Serialize(message3));

        var readIds = new string[] { message1Id, message3Id };
        UnityNativeAppInboxPersistence.MarkMessagesAsReadForIds(readIds);

        Assert.IsTrue(UnityNativeAppInboxPersistence.IsRead(message1Id));
        Assert.IsFalse(UnityNativeAppInboxPersistence.IsRead(message2Id));
        Assert.IsTrue(UnityNativeAppInboxPersistence.IsRead(message3Id));
    }

    [Test]
    public void GetUnreadMessages_ReturnsOnlyUnread()
    {
        Dictionary<string, object> message1 = new Dictionary<string, object>
        {
            {"_id", 6},
            { "isRead", false }
        };

        Dictionary<string, object> message2 = new Dictionary<string, object>
        {
            {"_id", 7},
            { "isRead", false }
        };

        string message1Id = message1["_id"].ToString();
        string message2Id = message2["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(message1Id, Json.Serialize(message1));
        UnityNativeAppInboxPersistence.SaveMessage(message2Id, Json.Serialize(message2));
        UnityNativeAppInboxPersistence.MarkAsRead(message1Id);

        var unread = UnityNativeAppInboxPersistence.GetUnreadMessages();

        Assert.IsFalse(unread.ContainsKey(message1Id));
        Assert.IsTrue(unread.ContainsKey(message2Id));
    }

    [Test]
    public void DeleteMessage_RemovesMessageAndMetadata()
    {
        Dictionary<string, object> message = new Dictionary<string, object>
        {
            {"_id", 8},
            { "isRead", false }
        };
        
        string messageId = message["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(messageId, Json.Serialize(message));
        UnityNativeAppInboxPersistence.MarkAsRead(messageId);

        UnityNativeAppInboxPersistence.DeleteMessage(messageId);

        Assert.IsEmpty(UnityNativeAppInboxPersistence.GetMessage(messageId));
        Assert.IsFalse(UnityNativeAppInboxPersistence.IsRead(messageId));
    }

    [Test]
    public void DeleteAllMessages_RemovesEverything()
    {
        Dictionary<string, object> message1 = new Dictionary<string, object>
        {
            {"_id", 9},
            { "isRead", false }
        };

        Dictionary<string, object> message2 = new Dictionary<string, object>
        {
            {"_id", 10},
            { "isRead", false }
        };

        string message1Id = message1["_id"].ToString();
        string message2Id = message2["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(message1Id, Json.Serialize(message1));
        UnityNativeAppInboxPersistence.SaveMessage(message2Id, Json.Serialize(message2));

        UnityNativeAppInboxPersistence.DeleteAllMessages();

        var allMessages = UnityNativeAppInboxPersistence.GetAllMessages();
        Assert.IsEmpty(allMessages);
    }

    [Test]
    public void DeleteMessagesForIds_DeletesOnlySpecifiedMessages()
    {
        Dictionary<string, object> message1 = new Dictionary<string, object>
        {
            {"_id", 11},
            { "isRead", false }
        };

        Dictionary<string, object> message2 = new Dictionary<string, object>
        {
            {"_id", 12},
            { "isRead", false }
        };

        Dictionary<string, object> message3 = new Dictionary<string, object>
        {
            {"_id", 13},
            { "isRead", false }
        };

        string message1Id = message1["_id"].ToString();
        string message2Id = message2["_id"].ToString();
        string message3Id = message3["_id"].ToString();

        UnityNativeAppInboxPersistence.SaveMessage(message1Id, Json.Serialize(message1));
        UnityNativeAppInboxPersistence.SaveMessage(message2Id, Json.Serialize(message2));
        UnityNativeAppInboxPersistence.SaveMessage(message3Id, Json.Serialize(message3));

        var deleteIds = new string[] {message1Id, message2Id };
        UnityNativeAppInboxPersistence.DeleteMessagesForIds(deleteIds);

        Assert.IsEmpty(UnityNativeAppInboxPersistence.GetMessage(message1Id));
        Assert.IsEmpty(UnityNativeAppInboxPersistence.GetMessage(message2Id));
        Assert.AreEqual(Json.Serialize(message3), UnityNativeAppInboxPersistence.GetMessage(message3Id));
    }
}