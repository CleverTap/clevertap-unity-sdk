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

#### Set Locale

By default CleverTap reports the device's OS locale, which campaigns use to choose the message language. If your game has its own language setting, override it with the player's choice so campaigns follow the in-game language instead of the phone's. Use the `language_COUNTRY` format. Call it at startup with the saved choice and again whenever the player changes language.

```csharp
CleverTap.SetLocale("fr_FR");
```

Empty strings are ignored. No-op on standalone and Editor builds.

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

## Game Events

`CleverTapGameEvents` provides typed helpers for common gameplay events. Each one wraps `CleverTap.RecordEvent` with a fixed event name and property set, so events are consistent across games and group together in the CleverTap dashboard (all event names start with `GameEvent_`). Optional parameters are left out of the event when not passed.

> **Note:** These are custom events, not CleverTap system events. They work like any event you record yourself with `CleverTap.RecordEvent`. They show up with your other custom events in the dashboard and count toward your custom event limit. CleverTap doesn't treat them specially: for example, a subscription event doesn't record revenue the way a Charged event does. You can use them in segments, campaigns and reports like any other event.

#### Record a Resource Event

Currency or items earned or spent. Sent as `GameEvent_Resource` with properties `flow`, `currency`, `amount`, `category`, `item`.

```csharp
CleverTapGameEvents.RecordResourceEvent(CleverTapGameEvents.ResourceFlow.Earned, "Gems", 100f, "DailyReward", "LoginBonus");
CleverTapGameEvents.RecordResourceEvent(CleverTapGameEvents.ResourceFlow.Spent, "Coins", 250f, "Shop", "SpeedBoost");
```

#### Record a Level Event

Level progress: `Started`, `Completed` or `Failed`. Sent as `GameEvent_Level` with `status`, `world` and optional `level`, `stage`, `score`.

```csharp
CleverTapGameEvents.RecordLevelEvent(CleverTapGameEvents.LevelStatus.Started, "World1", "Level3");
CleverTapGameEvents.RecordLevelEvent(CleverTapGameEvents.LevelStatus.Completed, "World1", "Level3", score: 4200);
```

#### Record a Custom Game Event

Any other gameplay moment, with an optional numeric value. Sent as `GameEvent_Design` with `event` and optional `value`. A `Category:Action:Detail` naming style keeps these easy to filter.

```csharp
CleverTapGameEvents.RecordGameEvent("Chest:Opened:Rare", 1f);
```

#### Record an Ad Event

Ad interactions: `Shown`, `Clicked`, `Skipped`, `RewardGranted`. Sent as `GameEvent_Ad` with `action` and optional `format` (`Banner`, `Interstitial`, `RewardedVideo`, `OfferWall`, `Playable`) and `placement`.

```csharp
CleverTapGameEvents.RecordAdEvent(CleverTapGameEvents.AdEvent.RewardGranted, CleverTapGameEvents.AdFormat.RewardedVideo, "PostLevel");
```

#### Record an Error Event

In-game errors, with severity `Low`, `Medium`, `High` or `Critical`. Sent as `GameEvent_Error` with `severity` and `description`.

```csharp
CleverTapGameEvents.RecordErrorEvent(CleverTapGameEvents.ErrorSeverity.Low, "Inventory failed to load");
```

#### Record a Subscription Event

Subscription lifecycle changes: `TrialStarted`, `PaidConversion`, `Renewed`, `Cancelled`, `BillingFailed`, `Expired`, `Restored`, `Refunded`. Sent as `GameEvent_Subscription` with `status`, `product_id` and optional `price`, `currency`, `period` (`Weekly`, `Monthly`, `Yearly`).

The SDK does not listen to the App Store or Google Play itself. Call this from your own store / IAP callbacks when the subscription state changes.

```csharp
CleverTapGameEvents.RecordSubscriptionEvent(
    CleverTapGameEvents.SubscriptionStatus.PaidConversion,
    "com.mygame.premium_monthly",
    price: 4.99f,
    currency: "USD",
    period: CleverTapGameEvents.SubscriptionPeriod.Monthly);

CleverTapGameEvents.RecordSubscriptionEvent(CleverTapGameEvents.SubscriptionStatus.Cancelled, "com.mygame.premium_monthly");
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

## SDK Lifecycle Controls

Use `PauseSDK` and `ResumeSDK` to temporarily halt CleverTap data collection. Events recorded while the SDK is paused are **buffered in memory** and flushed automatically the moment `ResumeSDK` is called — no events are lost.

**Gaming use case:** Pause the SDK when entering an active gameplay loop where analytics overhead or incomplete mid-session events are undesirable. Resume when the player reaches a natural breakpoint — a menu screen, a lobby, an interstitial, or when the app returns to the foreground — so buffered events are sent together as a coherent batch.

```csharp
// Entering active gameplay — pause tracking
void OnGameplayStart()
{
    CleverTap.PauseSDK();
}

// Returning to menu / lobby — resume and flush buffered events
void OnGameplayEnd()
{
    CleverTap.ResumeSDK();
}
```

#### Pause SDK

```csharp
CleverTap.PauseSDK();
```

#### Resume SDK

```csharp
CleverTap.ResumeSDK();
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

#### Dismiss PiP In-App

Programmatically dismisses a currently visible Picture-in-Picture (PiP) in-app notification.

```csharp
CleverTap.DismissPipInApp();
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

## Native Display

#### Listen for Display Units

```csharp
CleverTap.OnCleverTapNativeDisplayUnitsUpdated += (string message) => {
    JSONArray units = CleverTap.GetAllDisplayUnits();
};
```

#### Get Display Units

```csharp
JSONArray units = CleverTap.GetAllDisplayUnits();
JSONClass unit = CleverTap.GetDisplayUnitForID(unitID);
```

#### Record Display Unit Viewed / Clicked

```csharp
CleverTap.RecordDisplayUnitViewedEventForID(unitID);
CleverTap.RecordDisplayUnitClickedEventForID(unitID);
```

Display unit views count toward Native Display frequency caps. Call `RecordDisplayUnitViewedEventForID` once per real impression, not every time the view is rebound or scrolled back into view.

#### Record Viewed / Clicked for a Single Element (e.g. a Carousel Slide)

Use these when a unit has several items and you want each one attributed separately. Get the item's attribution with `GetDisplayUnitContentMetaData` and pass it back:

```csharp
Dictionary<string, object> slideMeta = CleverTap.GetDisplayUnitContentMetaData(unitID, slideIndex);

CleverTap.RecordDisplayUnitElementViewedEventForID(unitID, slideMeta);   // slide came on screen
CleverTap.RecordDisplayUnitElementClickedEventForID(unitID, slideMeta);  // slide tapped
```

`GetDisplayUnitContentMetaData` returns an empty dictionary for an unknown unit or an invalid index.

#### Refresh Frequency Cap Rules

Cap rules are delivered on app launch. To request fresh rules mid-session:

```csharp
CleverTap.FetchNativeDisplayMeta();
```

The new rules arrive on a later response from CleverTap.

On standalone and Editor builds, the record and fetch APIs have no effect, and the getters return empty values: `GetAllDisplayUnits()` returns an empty `JSONArray`, `GetDisplayUnitForID()` an empty `JSONClass`, and `GetDisplayUnitContentMetaData()` an empty dictionary.

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

