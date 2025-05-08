using NUnit.Framework;
using CleverTapSDK.Native;

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
        UnityNativeAppInboxPersistence.SaveMessage("msg1", "Hello World");
        string message = UnityNativeAppInboxPersistence.GetMessage("msg1");
        Assert.AreEqual("Hello World", message);
    }

    [Test]
    public void MarkAsReadAndCheckStatus_WorksCorrectly()
    {
        UnityNativeAppInboxPersistence.SaveMessage("msg2", "Read Me");
        UnityNativeAppInboxPersistence.MarkAsRead("msg2");
        Assert.IsTrue(UnityNativeAppInboxPersistence.IsRead("msg2"));
    }

    [Test]
    public void MarkMessagesAsReadForIds_MarksOnlySpecifiedMessagesAsRead()
    {
        UnityNativeAppInboxPersistence.SaveMessage("msg11", "Unread A");
        UnityNativeAppInboxPersistence.SaveMessage("msg12", "Unread B");
        UnityNativeAppInboxPersistence.SaveMessage("msg13", "Unread C");

        var readIds = new string[] { "msg11", "msg13" };
        UnityNativeAppInboxPersistence.MarkMessagesAsReadForIds(readIds);

        Assert.IsTrue(UnityNativeAppInboxPersistence.IsRead("msg11"));
        Assert.IsFalse(UnityNativeAppInboxPersistence.IsRead("msg12"));
        Assert.IsTrue(UnityNativeAppInboxPersistence.IsRead("msg13"));
    }

    [Test]
    public void GetUnreadMessages_ReturnsOnlyUnread()
    {
        UnityNativeAppInboxPersistence.SaveMessage("msg3", "Unread 1");
        UnityNativeAppInboxPersistence.SaveMessage("msg4", "Unread 2");
        UnityNativeAppInboxPersistence.MarkAsRead("msg3");

        var unread = UnityNativeAppInboxPersistence.GetUnreadMessages();

        Assert.IsFalse(unread.ContainsKey("msg3"));
        Assert.IsTrue(unread.ContainsKey("msg4"));
    }

    [Test]
    public void DeleteMessage_RemovesMessageAndMetadata()
    {
        UnityNativeAppInboxPersistence.SaveMessage("msg5", "To be deleted");
        UnityNativeAppInboxPersistence.MarkAsRead("msg5");

        UnityNativeAppInboxPersistence.DeleteMessage("msg5");

        Assert.IsEmpty(UnityNativeAppInboxPersistence.GetMessage("msg5"));
        Assert.IsFalse(UnityNativeAppInboxPersistence.IsRead("msg5"));
    }

    [Test]
    public void DeleteAllMessages_RemovesEverything()
    {
        UnityNativeAppInboxPersistence.SaveMessage("msg6", "One");
        UnityNativeAppInboxPersistence.SaveMessage("msg7", "Two");

        UnityNativeAppInboxPersistence.DeleteAllMessages();

        var allMessages = UnityNativeAppInboxPersistence.GetAllMessages();
        Assert.IsEmpty(allMessages);
    }

    [Test]
    public void DeleteMessagesForIds_DeletesOnlySpecifiedMessages()
    {
        UnityNativeAppInboxPersistence.SaveMessage("msg8", "Message 8");
        UnityNativeAppInboxPersistence.SaveMessage("msg9", "Message 9");
        UnityNativeAppInboxPersistence.SaveMessage("msg10", "Message 10");

        var deleteIds = new string[] { "msg8", "msg9" };
        UnityNativeAppInboxPersistence.DeleteMessagesForIds(deleteIds);

        Assert.IsEmpty(UnityNativeAppInboxPersistence.GetMessage("msg8"));
        Assert.IsEmpty(UnityNativeAppInboxPersistence.GetMessage("msg9"));
        Assert.AreEqual("Message 10", UnityNativeAppInboxPersistence.GetMessage("msg10"));
    }
}