## Variables

With the CleverTap Unity SDK, you can create variables on your app that take on new values from the CleverTap dashboard. Using variables in the CleverTap dashboard allows you to roll out changes without pushing an update through the Apple App Store/Google Play Store. These can also be used in A/B tests to test features for only a percentage of your users.

You can define variables using the CleverTap Unity SDK. When you define a variable in your code, you can sync them to the CleverTap Dashboard via the provided SDK methods.

# Supported Variable Types

Currently, CleverTap SDK supports the following variable types:

- string
- bool
- byte
- short
- int
- long
- float
- double
- file

# Define Variables

Variables can be defined using the `Define<T>` method. You must provide a name and a default value for the variable. This method will return an object of type `Var<T>` where T is the type of default value. For example, int.

Every variable must have a unique name and cannot be defined multiple times for each consecutive CleverTap call. Define with the same variable name will result in the same instance of object from the cache as the first one.

```csharp
Var<int> intVariable = CleverTap.Define("int_variable", 10);  
Var<double> doubleVariable = CleverTap.Define("double_variable", 11.5);  
Var<string> stringVariable = CleverTap.Define("string_variable", "hello, world");
```

# Define File Variables
Clevertap supports File Types for Variables from `v4.1.0+`. Supported file types include but are not limited to images (jpg, jpeg, png, gif), text files, and PDFs.

Use the `DefineFileVariable(...)` method.

```csharp
Var<string> fileVar = CleverTap.DefineFileVariable("var_file");
```

The file variables value is a string with the file path to the file on the device.

_Note that file variables can be defined and synced in Unity Editor and other platforms. However, the use of File Variables is limited to Mobile platforms (iOS, Android) only. The File variables callbacks are not supported for Unity Editor and other builds. In those cases, the File variable value is a string - the file URL. The files are not downloaded._

# Setup Callbacks

CleverTap Unity SDK provides several callbacks for the developer to receive feedback from the SDK. You can use them as per your requirements. Using all of them is not mandatory. They are as follows:

- Status of fetch variables request
- `OnVariablesChanged`
- `OnOneTimeVariablesChanged`
- `OnVariablesChangedAndNoDownloadsPending`
- `OnOneTimeVariablesChangedAndNoDownloadsPending`
- `OnValueChanged`
- `OnFileReady`

## Status of Variables Fetch Request

This method provides a boolean flag to ensure the variables are successfully fetched from the server.

```csharp
CleverTap.FetchVariables(OnFetchVariablesCallback);  
void OnFetchVariablesCallback(bool isSuccess) {  
        Debug.Log("Unity received fetched variables with success: " + isSuccess);  
    }
```

## OnVariablesChanged

This callback is invoked when variables are initialized with values fetched from the server. It is called each time new values are fetched.

```csharp
CleverTap.OnVariablesChanged += OnVariablesChanged;  
private void OnVariablesChanged() {  
Debug.Log("Unity received variables changed");  
    }
```

## OnOneTimeVariablesChanged

This callback is invoked when variables are initialized with values fetched from the server. It is called once.

```csharp
CleverTap.OnOneTimeVariablesChanged += OnOneTimeVariablesChanged;  
private void OnOneTimeVariablesChanged()  
    {  
Debug.Log("Unity received variables changed");  
 }
```

## OnVariablesChangedAndNoDownloadsPending

This callback is invoked when variables are initialized with values fetched from the server and files download is complete. It is called each time new values are fetched and downloads completed.

```csharp
CleverTap.OnVariablesChangedAndNoDownloadsPending += OnVariablesChangedAndNoDownloadsPending;  
private void OnVariablesChangedAndNoDownloadsPending() {  
Debug.Log("Unity received variables changed and no downloads pending");  
    }
```

## OnOneTimeVariablesChangedAndNoDownloadsPending

This callback is invoked when variables are initialized with values fetched from the server and files download is complete. It is called once.

```csharp
CleverTap.OnOneTimeVariablesChangedAndNoDownloadsPending += OnOneTimeVariablesChangedAndNoDownloadsPending;  
private void OnOneTimeVariablesChangedAndNoDownloadsPending()  
    {  
Debug.Log("Unity received variables changed and no downloads pending");  
 }
```

## OnValueChanged

This callback is invoked when the value of the variable changes. You attach the callback to the variable object:

```csharp
Var<string> stringVariable = CleverTap.Define("string_variable", "hello, world");  
stringVariable.OnValueChanged += stringVariable_OnValueChanged;  
private void stringVariable_OnValueChanged() {  
        Debug.Log("Unity received variable changed stringVariable");  
    }
```

## OnFileReady

This individual callback is registered per file variable. It is called when the file associated with the variable is downloaded and ready to be used.

```csharp
Var<string> fileVar = CleverTap.DefineFileVariable("var_file");
fileVar.OnFileReady += fileVar_OnFileReady;  
private void fileVar_OnFileReady() {  
        Debug.Log("Unity received file ready fileVar");  
    }
```

# Sync Defined Variables

After defining your variables in the code, you must send/sync variables to the server. To do so, the app must be in DEBUG mode and mark a particular CleverTap user profile as a test profile from the CleverTap dashboard. Learn how to mark a profile as a Test Profile. After marking the profile as a test profile, you must sync the app variables in DEBUG mode:

```csharp
// 1. Define CleverTap variables  
// …  
// 2. Add variables/values changed callbacks  
// …  
// 3. Sync CleverTap Variables from DEBUG mode/builds  
CleverTap.SyncVariables();
```

> 📘 Key Points to Remember
> 
> - In a scenario where there is already a draft created by another user profile in the dashboard, the sync call will fail to avoid overriding important changes made by someone else. In this case, **Publish** or **Dismiss** the existing draft before you proceed with syncing variables again. However, you can override a draft you created via the sync method previously to optimize the integration experience.
> - You can receive the following logs from the CleverTap SDK:
>   - Variables synced successfully.
>   - Unauthorized access from a non-test profile. To address this, mark the profile as a test profile from the CleverTap dashboard.

# Fetch Variables During a Session

During a session, you can fetch the updated values for your CleverTap variables from the server. If variables have changed, the appropriate callbacks will be fired. The callback provides a boolean flag indicating if the fetch call was successful. The callback is fired regardless of whether the variables have changed or not.

```csharp
CleverTap.FetchVariables(OnFetchVariablesCallback);  
void OnFetchVariablesCallback(bool isSuccess) {  
        Debug.Log("Unity received fetched variables with success: " + isSuccess);  
    }
```

# Use Fetched Variables Values

This process involves the following two major steps:

- Fetch variable values
- Access variable values

## Fetch Variable Values

Variables are updated automatically when server values are received. If you want to receive feedback when a specific variable is updated, use the individual callback:

```csharp
Var<string> stringVariable = CleverTap.Define("string_variable", "hello, world");  
stringVariable.OnValueChanged += stringVariable_OnValueChanged;  
private void stringVariable_OnValueChanged() {  
        Debug.Log("Unity received variable changed stringVariable");  
    }
```

## Access Variable Values

Access the variable values inside the `OnVariablesChanged`, `OnOneTimeVariablesChanged`, and `onValueChanged` callbacks.

```csharp
Var<string> stringVariable = CleverTap.Define("string_variable", "hello, world");

CleverTap.OnVariablesChanged += OnVariablesChanged;  
private void OnVariablesChanged() {  
// Access the value  
string val = stringVariable.Value;  
// Access the default value the variable was defined with  
string defaultVal = stringVariable.DefaultValue;  
    }
```

### Getting a Specific Variable

```csharp
// Define the variable  
var doubleVariable = CleverTap.Define("double_variable", 11.5);  
// …  
// Get the variable instance for already defined variable  
var doubleVariable = CleverTap.GetVariable<double>("double_variable");
```