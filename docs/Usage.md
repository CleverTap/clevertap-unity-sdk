# Unity Usage


## User Profiles

#### Update User Profile(Push Profile)

```csharp
Dictionary<string, object> props = new Dictionary<string, object>();
props.Add("Name", "Jack Montana");
props.Add("DOB", DateTime.Now);
props.Add("FavouriteNumber", 33);
CleverTap.ProfilePush(props);
```

#### Set Multi Values For Key 

```csharp
List<string> stringList = new List<string>();
stringList.Add("one");
stringList.Add("two");
CleverTap.ProfileSetMultiValuesForKey("multiKey", stringList);
```

#### Remove Multi Value For Key

```csharp
List<string> stringList = new List<string>();
stringList.Add("two");
CleverTap.ProfileRemoveMultiValuesForKey("multiKey", stringList);
```

#### Add Multi Value For Key

```csharp
CleverTap.ProfileAddMultiValueForKey("multiKey", "three");
```

#### Increment a numerical value for a single-value profile property (if it exists)

```csharp
CleverTap.ProfileIncrementValueForKey("score", 10); // INTEGER PROFERTY
CleverTap.ProfileIncrementValueForKey("profit", 1.5); // DOUBLE PROPERTY
```

#### Decrement a numerical value for a single-value profile property (if it exists)

```csharp
CleverTap.ProfileDecrementValueForKey("score", 10); // INTEGER PROFERTY
CleverTap.ProfileDecrementValueForKey("profit", 1.5); // DOUBLE PROPERTY
```

#### Create a User profile when user logs in (On User Login)

```csharp
Dictionary<string, object> newProps = new Dictionary<string, object>();
newProps.Add("email", "test@test.com");
newProps.Add("Identity", "123456");
CleverTap.OnUserLogin(newProps);
```

#### Set Location to User Profile

```csharp
CleverTap.SetLocation(34.147785, -118.144516);
```

## User Events

#### Record an event  

```csharp
CleverTap.RecordEvent("My event");
```

```csharp
Dictionary<string, object> eventProperties = new Dictionary<string, object>
{
   { "Item name", "Book 1" },
   { "Product category", "books" },
   { "Price", 14.99 }
};
CleverTap.RecordEvent("Product Viewed", eventProperties);
```

#### Record Charged event

```csharp
var chargeDetails = new Dictionary<string, object>(){
    { "Amount", 53.43 },
    { "Currency", "USD" },
    { "Payment Mode", "Cash" },
    { "Date", DateTime.UtcNow },
    { "Charged ID", 24052013 }
};
var items = new List<Dictionary<string, object>> {
    new Dictionary<string, object> {
        { "Price", 24.99 },
        { "Product category", "books" },
        { "Item name", "Achieving inner zen" },
        { "Quantity", 1 }
    },
    new Dictionary<string, object> {
        { "Price", 24.99 },
        { "Product category", "books" },
        { "Item name", "Taming the chaos" },
        { "Quantity", 1 }
    },
    new Dictionary<string, object> {
        { "Price", 0.49 },
        { "Product category", "supplies" },
        { "Item name", "Ballpoint pen" },
        { "Quantity", 5 }
    },
    new Dictionary<string, object> {
        { "Price", 1 },
        { "Product category", "supplies" },
        { "Item name", "Notebook" },
        { "Quantity", 5 }
    }
};

CleverTap.RecordChargedEventWithDetailsAndItems(chargeDetails, items);
```

## In-App Notifications

#### On In App Show

```csharp
CleverTap.OnCleverTapInAppNotificationShowCallback += CleverTapInAppNotificationShowCallback;
private void CleverTapInAppNotificationShowCallback(string message)
{
   Debug.Log($"InAppNotification Show callback: {message}");
}
```

#### On In App Button Click

```csharp
CleverTap.OnCleverTapInAppNotificationButtonTapped += CleverTapInAppNotificationButtonTapped;
private void CleverTapInAppNotificationButtonTapped(string message)
{
   Debug.Log($"InAppNotification ButtonTapped: {message}");
}
```

#### On Dismissed

```csharp
CleverTap.OnCleverTapInAppNotificationDismissedCallback += CleverTapInAppNotificationDismissedCallback;
private void CleverTapInAppNotificationDismissedCallback(string message)
{
   Debug.Log($"InAppNotification Dismissed callback: {message}");
}
```

## In-App Notification Controls

#### Suspend In-App Notifications

```csharp
CleverTap.SuspendInAppNotifications();
```

#### Discard In-App Notifications

```csharp
CleverTap.DiscardInAppNotifications();
```

#### Resume In-App Notifications

```csharp
CleverTap.ResumeInAppNotifications();
```

## App Inbox

#### Initialize the CleverTap App Inbox Method

```csharp
CleverTap.InitializeInbox();
Debug.Log("InboxInit started");
```

#### Show the App Inbox

#### Use the below snippet to show default CleverTap's Inbox 

```csharp
CleverTap.ShowAppInbox("");
```

#### Use the below snippet to show custom CleverTap's Inbox

```csharp
Dictionary<string, object> styleConfig = new Dictionary<string, object>
{
    { "navBarTitle", "My App Inbox" },
    { "navBarTitleColor", "#FF0000" },
    { "navBarColor", "#FFFFFF" },
    { "inboxBackgroundColor", "#AED6F1" },
    { "backButtonColor", "#00FF00" },
    { "unselectedTabColor", "#0000FF" },
    { "selectedTabColor", "#FF0000" },
    { "noMessageText", "No message(s)" },
    { "noMessageTextColor", "#FF0000" }
};
// Convert the Dictionary parameters to a string and pass it to `ShowAppInbox()`
string jsonStr = Json.Serialize(styleConfig);
CleverTap.ShowAppInbox(jsonStr);
```

#### Dismiss the App Inbox

```csharp
CleverTap.DismissAppInbox();
```

#### Get All Inbox Messages

```csharp
JSONArray jsonMessages = CleverTap.GetAllInboxMessages();
List<CleverTapInboxMessage> messages = CleverTap.GetAllInboxMessagesParsed();
```

#### Get Unread Inbox Messages

```csharp
JSONArray jsonMessages = CleverTap.GetUnreadInboxMessages();
List<CleverTapInboxMessage> messages = CleverTap.GetUnreadInboxMessagesParsed();
```

#### Get Inbox Message

```csharp
JSONClass jsonMessage = CleverTap.GetInboxMessageForId(id);
CleverTapInboxMessage message = CleverTap.GetInboxMessageForIdParsed(id);
```

#### Delete Inbox Message

```csharp
CleverTap.DeleteInboxMessageForID(id);
```

## Enable Debugging

#### Set Debug Level

```csharp
CleverTap.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
```

## Push Notifications

#### Creating Notification Channel

```csharp
CleverTap.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
```

#### Delete Notification Channel

```csharp
CleverTap.DeleteNotificationChannel("YourChannelId");		
```

#### Creating a group notification channel

```csharp
CleverTap.CreateNotificationChannelGroup("YourGroupId", "Your Group Name");		
```

#### Delete a group notification channel

```csharp
CleverTap.DeleteNotificationChannelGroup("YourGroupId");			
```

#### Create Notification Channel

```csharp
CleverTap.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
```

## Push Primer

### Half-Interstitial Local In-App 

```csharp
Dictionary<string, object> item = new Dictionary<string, object>();
item.Add("inAppType", "half-interstitial");
item.Add("titleText", "Get Notified");
item.Add("messageText", "Please enable notifications on your device to use Push Notifications.");
item.Add("followDeviceOrientation", true);
item.Add("positiveBtnText", "Allow");
item.Add("negativeBtnText", "Cancel");
item.Add("backgroundColor", "#FFFFFF");
item.Add("btnBorderColor", "#0000FF");
item.Add("titleTextColor", "#0000FF");
item.Add("messageTextColor", "#000000");
item.Add("btnTextColor", "#FFFFFF");
item.Add("btnBackgroundColor", "#0000FF");
item.Add("imageUrl", "https://icons.iconarchive.com/icons/treetog/junior/64/camera-icon.png");
item.Add("btnBorderRadius", "2");
item.Add("fallbackToSettings", true);

CleverTap.PromptPushPrimer(item);
```

### Alert Local In-App

```csharp
Dictionary<string, object> item = new Dictionary<string, object>();
item.Add("inAppType", "half-interstitial");
item.Add("titleText", "Get Notified");
item.Add("messageText", "Please enable notifications on your device to use Push Notifications.");
item.Add("followDeviceOrientation", true);
item.Add("fallbackToSettings", true);

CleverTap.PromptPushPrimer(item);
```

## Prompt to show hard notification permission dialog. 
### true - fallbacks to app's notification settings if permission is denied,
### false - does not fallback to app's notification settings if permission is denied

```csharp
CleverTap.PromptForPushPermission(false);
```

### Returns a boolean to indicate whether notification permission is granted or not

```csharp
bool isPushPermissionGranted = CleverTap.IsPushPermissionGranted();
```

