
# Unity SDK WebGL Quick Start Guide (SDK v4.0.0)

## Overview

This guide will help you install the CleverTap Unity SDK, track your first user event, and see this information in the CleverTap dashboard.

## Installation

You can install the CleverTap Unity SDK using the `.unitypackage` or as a local package through the Unity Package Manager (UPM).

### Import the CleverTap Unity Package

1. Download the latest CleverTap Unity package.
2. Import the `.unitypackage` into your Unity project via Assets > Import Package > Custom Package.

### Import as a Local Dependency

Clone the latest CleverTap Unity SDK release and import it as a local package through the Unity Package Manager.

### Set Up the Unity SDK

CleverTap API can be accessed anywhere in your project by calling the static CleverTap class. Initialize CleverTap using your CleverTap Account ID, Token, and Region.

```csharp
CleverTap.LaunchWithCredentialsForRegion("YOUR_CLEVERTAP_ACCOUNT_ID", "YOUR_CLEVERTAP_ACCOUNT_TOKEN", "CLEVERTAP_ACCOUNT_REGION");
CleverTap.EnablePersonalization();
```

### Callbacks

Handle callbacks by adding an event listener through CleverTap static events.

```csharp
CleverTap.OnCleverTapDeepLinkCallback += YOUR_CALLBACK_METHOD;
CleverTap.OnCleverTapProfileInitializedCallback += YOUR_CALLBACK_METHOD;
```

## Initialize CleverTap SDK

```csharp
CleverTap.LaunchWithCredentialsForRegion("YOUR_CLEVERTAP_ACCOUNT_ID", "YOUR_CLEVERTAP_ACCOUNT_TOKEN", "CLEVERTAP_ACCOUNT_REGION");
CleverTap.EnablePersonalization();
```

# Unity User Profiles

### Update User Profile (Push Profile)

```csharp
Dictionary<string, string> newProps = new Dictionary<string, string>();
newProps.Add("Name", "Jack Montana");
newProps.Add("Identity", "61026032");
newProps.Add("Email", "jack@example.com");
newProps.Add("Phone", "+14155551234");
newProps.Add("Gender", "M");
CleverTap.ProfilePush(newProps);
```

### Set Multi Values for Key

```csharp
List<string> stringList = new List<string> { "one", "two" };
CleverTap.ProfileSetMultiValuesForKey("userProps", stringList);
```

### Remove Multi Value for Key

```csharp
List<string> stringList = new List<string> { "two" };
CleverTap.ProfileRemoveMultiValuesForKey("userProps", stringList);
```

### Add Multi Value for Key

```csharp
List<string> stringList = new List<string> { "three", "four" };
CleverTap.ProfileAddMultiValuesForKey("multiIOS", stringList);
```

### Create a User Profile on User Login

```csharp
Dictionary<string, string> newProps = new Dictionary<string, string>();
newProps.Add("email", "jack@example.com");
newProps.Add("Identity", "123456");
newProps.Add("Name", "Jack Montana");
newProps.Add("Phone", "+14155551234");
newProps.Add("Gender", "M");
CleverTap.OnUserLogin(newProps);
```

### Get CleverTap Reference ID

```csharp
string cleverTapID = CleverTap.ProfileGetCleverTapID();
```

### Set Location to User Profile

```csharp
CleverTap.SetLocation(34.147785, -118.144516);
```

### Increment a User Profile Property

```csharp
CleverTap.ProfileIncrementValueForKey("add_int", 2);
CleverTap.ProfileIncrementValueForKey("add_double", 3.5);
```

### Decrement a User Profile Property

```csharp
CleverTap.ProfileDecrementValueForKey("minus_int", 2);
CleverTap.ProfileDecrementValueForKey("minus_double", 3.5);
```

For more detailed instructions, visit the [Unity User Profiles](https://developer.clevertap.com/docs/unity-user-profiles).

# Unity User Events

### Record Event without Properties

```csharp
CleverTap.RecordEvent("Product Viewed");
```

### Record Event with Properties

```csharp
Dictionary<string, object> props = new Dictionary<string, object>();
props.Add("Product Name", "Casio Chronograph Watch");
props.Add("Category", "Mens Accessories");
props.Add("Price", 59.99);
props.Add("Date", new DateTime());
CleverTap.RecordEvent("Product Viewed", props);
```

### Record Charged Event

```csharp
Dictionary<string, object> chargedProps = new Dictionary<string, object>();
chargedProps.Add("Total Amount", 300);
chargedProps.Add("Payment Mode", "Credit Card");
chargedProps.Add("Charged ID", 24052013);

List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

Dictionary<string, object> item1 = new Dictionary<string, object>();
item1.Add("Product Category", "Books");
item1.Add("Book Name", "The Millionaire next door");
item1.Add("Quantity", 1);
items.Add(item1);

Dictionary<string, object> item2 = new Dictionary<string, object>();
item2.Add("Product Category", "Books");
item2.Add("Book Name", "Achieving inner zen");
item2.Add("Quantity", 1);
items.Add(item2);

CleverTap.RecordChargedEvent(chargedProps, items);
```

### Record Event with Multi-Value Properties

```csharp
CleverTap.Event.AddMultiValuesForKey("Visited Location", new List<string> { "India", "China" });
CleverTap.Event.RemoveMultiValuesForKey("Visited Location", new List<string> { "China" });
CleverTap.Event.SetMultiValuesForKey("Visited Location", new List<string> { "India", "China", "Australia" });
```

For more detailed instructions, visit the [CleverTap Unity SDK Quick Start Guide](https://developer.clevertap.com/docs/unity-sdk-quick-start-guide-sdk-v300).

