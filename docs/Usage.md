# Unity Usage


## User Properties

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
#### Record an event  

```

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
## In App Messages

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
#### Registering Fcm, Baidu, Xiaomi, Huawei Token

```

```
#### Create Notification

```
CleverTapBinding.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
```

### For more information,

- [See included Starter Application](/example/CleverTapUnity.cs) 

