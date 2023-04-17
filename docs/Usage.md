# Unity Usage


## User Profiles

#### Update User Profile(Push Profile)

```
Dictionary<string, string> props = new Dictionary<string, string>();
props.Add("RegistrationSource", "Android");
CleverTapBinding.ProfilePush(props);
```
#### Set Multi Values For Key 

``` 
List<string> stringList = new List<string>();
stringList.Add("one");
stringList.Add("two");
CleverTapBinding.ProfileSetMultiValuesForKey("multiAndroid", stringList);
```
#### Remove Multi Value For Key

```
List<string> stringList2 = new List<string>();
stringList2.Add("two");
CleverTapBinding.ProfileRemoveMultiValuesForKey("multiAndroid", stringList2);
```
#### Add Multi Value For Key
```
CleverTapBinding.ProfileAddMultiValueForKey("multiAndroid", "five");
```
#### Increment a numerical value for a single-value profile property (if it exists)
```
CleverTapBinding.ProfileIncrementValueForKey("score", 10); // INTEGER PROFERTY
CleverTapBinding.ProfileIncrementValueForKey("profit", 1.5); //DOUBLE PROPERTY
```
#### Decrement a numerical value for a single-value profile property (if it exists)
```
CleverTapBinding.ProfileDecrementValueForKey("score", 10); // INTEGER PROFERTY
CleverTapBinding.ProfileDecrementValueForKey("profit", 1.5); //DOUBLE PROPERTY
```
#### Create a User profile when user logs in (On User Login)

```
Dictionary<string, string> newProps = new Dictionary<string, string>();
newProps.Add("email", "test@test.com");
newProps.Add("Identity", "123456");
CleverTapBinding.OnUserLogin(newProps);
```
#### Set Location to User Profile

```
CleverTapBinding.SetLocation(34.147785, -118.144516);
```

## User Events

#### Record an event  

```
CleverTapBinding.RecordEvent("Button Clicked");
```

#### Record Charged event
```

Dictionary<string, object> chargeDetails = new Dictionary<string, object>();
chargeDetails.Add("Amount", 500);
chargeDetails.Add("Currency", "USD");
chargeDetails.Add("Payment Mode", "Credit card");


Dictionary<string, object> item = new Dictionary<string, object>();
item.Add("price", 50);
item.Add("Product category", "books");
item.Add("Quantity", 1);


Dictionary<string, object> item2 = new Dictionary<string, object>();
item2.Add("price", 100);
item2.Add("Product category", "plants");
item2.Add("Quantity", 10);

List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
items.Add(item);
items.Add(item2);

CleverTapBinding.RecordChargedEventWithDetailsAndItems(chargeDetails, items);

```
## In-App Notifications

#### On In App Button Click

```
public void onInAppButtonClick(HashMap<String, String> payload) {
invokeMethodOnUiThread("onInAppButtonClick", payload);
}
```
#### On Dismissed

```
void CleverTapInAppNotificationDismissedCallback(string message)
{
Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
}
```

## In-App Notification Controls

#### Suspend In-App Notifications  
```
CleverTapBinding.SuspendInAppNotifications();
```
#### Discard In-App Notifications  
```
CleverTapBinding.DiscardInAppNotifications();
```
#### Resume In-App Notifications  
```
CleverTapBinding.ResumeInAppNotifications();
```

## App Inbox

#### Initialize the CleverTap App Inbox Method
```
CleverTapBinding.InitializeInbox();
Debug.Log("InboxInit started");
```

#### Show the App Inbox

```
void LaunchInbox()
{
    CleverTapBinding.ShowAppInbox("");
}
```

#### Dismiss the App Inbox

```
CleverTapBinding.DismissAppInbox();
```
## Enable Debugging

#### Set Debug Level
```
CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
```

## Push Notifications

#### Creating Notification Channel

```
void OnAndroidInit()
{
   CleverTapBinding.Initialize(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN);
   CleverTapBinding.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
}	
```
#### Delete Notification Channel

```
CleverTapBinding.DeleteNotificationChannel("YourChannelId");		
```
#### Creating a group notification channel

``` 
CleverTapBinding.CreateNotificationChannelGroup("YourGroupId", "Your Group Name");		
```
#### Delete a group notification channel

```
CleverTapBinding.DeleteNotificationChannelGroup("YourGroupId");			
```

#### Create Notification

```
CleverTapBinding.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
```

## Push Primer

### Half-Interstial Local In-App 

```
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
CleverTapBinding.PromptPushPrimer(item);
```

### Alert Local In-App

```
Dictionary<string, object> item = new Dictionary<string, object>();
item.Add("inAppType", "half-interstitial");
item.Add("titleText", "Get Notified");
item.Add("messageText", "Please enable notifications on your device to use Push Notifications.");
item.Add("followDeviceOrientation", true);
item.Add("fallbackToSettings", true);
CleverTapBinding.PromptPushPrimer(item);
```

## Prompt to show hard notification permission dialog. 
### true - fallbacks to app's notification settings if permission is denied,
### false - does not fallback to app's notification settings if permission is denied

```
CleverTapBinding.PromptForPushPermission(false);
```

### Returns a boolean to indicate whether notification permission is granted or not

```
bool isPushPermissionGranted = CleverTapBinding.IsPushPermissionGranted();
```

### For more information,

- [See included Starter Application](/example/CleverTapUnity.cs) 

