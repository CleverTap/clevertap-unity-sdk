# Unity Usage


## User Properties

#### Update User Profile(Push Profile)
```Dart
Dictionary<string, string> props = new Dictionary<string, string>();
	        props.Add("RegistrationSource", "Android");
	

	        CleverTapBinding.ProfilePush(props);

```
#### Set Multi Values For Key 
``` Dart
List<string> stringList = new List<string>();
	        stringList.Add("one");
	        stringList.Add("two");
	

	        CleverTapBinding.ProfileSetMultiValuesForKey("multiAndroid", stringList);

```
#### Remove Multi Value For Key 
```Dart
List<string> stringList2 = new List<string>();
	        stringList2.Add("two");
	

	        CleverTapBinding.ProfileRemoveMultiValuesForKey("multiAndroid", stringList2);
```
#### Add Multi Value For Key
```Dart
   CleverTapBinding.ProfileAddMultiValueForKey("multiAndroid", "five");
```
#### Create a User profile when user logs in (On User Login)
```Dart

Dictionary<string, string> newProps = new Dictionary<string, string>();
	        newProps.Add("email", "test@test.com");
	        newProps.Add("Identity", "123456");
	        CleverTapBinding.OnUserLogin(newProps);
	

	

```
#### Set Location to User Profile
```Dart
CleverTapBinding.SetLocation(34.147785, -118.144516);
```
#### Record an event  
```Dart
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

```
#### Record Charged event
```Dart

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
## In App
#### On In App Button Click
```Dart
public void onInAppButtonClick(HashMap<String, String> payload) {
	        invokeMethodOnUiThread("onInAppButtonClick", payload);
	    }
```
#### On Dismissed
```Dart
void CleverTapInAppNotificationDismissedCallback(string message)
	    {
	        Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
	    }
```
## App Inbox
#### Initialize the CleverTap App Inbox Method
```Dart
CleverTapBinding.InitializeInbox();
	        Debug.Log("InboxInit started");

```
#### Show the App Inbox
```Dart
void LaunchInbox()
	    {
	        CleverTapBinding.ShowAppInbox("");
	    }
 ```
## Enable Debugging
#### Set Debug Level
```Dart
CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
```
## Push Notifications
#### Creating Notification Channel
```Dart
void OnAndroidInit()
	    {
	

	        CleverTapBinding.Initialize(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN);
	

	        CleverTapBinding.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
}	
```
#### Delete Notification Channel
```Dart
CleverTapBinding.DeleteNotificationChannel("YourChannelId");		
```
#### Creating a group notification channel
``` Dart
CleverTapBinding.CreateNotificationChannelGroup("YourGroupId", "Your Group Name");		
```
#### Delete a group notification channel
```Dart
CleverTapBinding.DeleteNotificationChannelGroup("YourGroupId");			
```
#### Registering Fcm, Baidu, Xiaomi, Huawei Token
```Dart
void CleverTapPushOpenedCallback(string message)
	    {
	        Debug.Log("unity received push opened: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
	

	        if (String.IsNullOrEmpty(message))
	        {
	            return;
	        }
	

	        try
	        {
	            JSONClass json = (JSONClass)JSON.Parse(message);
	            Debug.Log(String.Format("push notification data is {0}", json));
	        }
	        catch
	        {
	            Debug.Log("unable to parse json");
	        }
	    }
```
 #### Create Notification
```Dart
CleverTapBinding.CreateNotificationChannel("YourChannelId", "Your Channel Name", "Your Channel Description", 5, true);
```
### For more information,
 - [see included Starter Application]( https://github.com/CleverTap/clevertap-unity-sdk/blob/master/example/CleverTapUnity.cs) 

