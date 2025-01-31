# Custom Code Templates

CleverTap Unity SDK supports a custom presentation of in-app messages. This allows for utilizing the in-app notifications functionality with custom configuration and presentation logic. Two types of templates can be defined through the SDK: Templates and Functions. Templates can contain action arguments while Functions cannot and Functions can be used as actions while Templates cannot. Functions can be either "visual" or "non-visual". "Visual" functions can contain UI logic and will be part of the [In-App queue](#in-App-queue), while "non-visual" functions will be triggered directly when invoked and should not contain UI logic.

## Creating templates and functions
Templates consist of a name, type, and arguments (and isVisual for Functions). Type and arguments (and isVisual for Functions) are required and names must be unique across the application. The template definitions are validated for correctness on launch of the application. When an invalid template is found the application will raise an exception and close with a message explaining the wrong template definition. Make sure to launch the application after each template change to ensure template definitions are valid and can be triggered correctly.

The templates are defined in а JSON format with the following scheme:
```json
{
  "TemplateName": {
    "type": "template",
    "arguments": {
      "Argument1": {
        "type": "string|number|boolean|file|action|object",
        "value": "val"
        },
      "Argument2": {
        "type": "object",
        "value": {
          "Nested1": {
            "type": "string|number|boolean|object",
            "value": "val"
          },
          "Nested2": {
            "type": "string|number|boolean|object",
            "value": "val"
          }
        }
      }
    }
  },
  "functionName": {
    "type": "function",
    "isVisual": true|false,
    "arguments": {
      "a": {
      "type": "string|number|boolean|file|object",
      "value": "val"
      }
    }
  }
}
```
The JSON definitions should be placed in one or more files in `Assets/CleverTap/CustomTemplates`. They will be copied to the exported projects and automatically registered with the SDK.

For Android, the definitions will be within `clevertap-android-wrapper.androidlib/assets/CleverTap/CustomTemplates` and they will be registered through `CleverTapUnityAPI.initialize(Context context)`. All of this is handled automatically if `CleverTapUnityApplication` is used for the application class.

For iOS, the definitions are copied to the project `CleverTap/CustomTemplates` folder which is added to the "Unity-iPhone" target and "Copy Bundle Resources" Build Phase. The templates are automatically registered through the `CleverTapUnityAppController.mm`.

For a working example, see the CTExample project: `CTExample/Assets/CleverTap/templates.json`.

### Arguments
An `argument` is a structure that represents the configuration of the custom code templates. It consists of a `type` and a `value`.
The supported argument types are:
- Primitives - `boolean`, `number`, and `string`. They must have a default value which would be used if no other value is configured for the notification.
- `object` - An `Object` where keys are the argument names and values are ```Arguments``` with supported primitive values.
- `file` - a file argument that will be downloaded when the template is triggered. Files don't have default values.
- `action` - an action argument that could be a function template or a built-in action like ‘close’ or ‘open url’. Actions don't have default values.

#### Hierarchical arguments
You can group arguments by either using an `object` argument or indicating the group in the argument's name by using a "." symbol. Both definitions are treated the same. `file` and `action` type arguments can only be added to a group by specifying the group in the argument's name.

The following code snippets define identical arguments:
```json
"arguments": {
    "map": {
        "type": "object",
        "value": {
            "a": {
                "type": "number",
                "value": 5
            },
            "b": {
                "type": "number",
                "value": 6
            }
        }
    }
}
```
and
```json
"arguments": {
    "map.a": {
        "type": "number",
        "value": 5
    },
    "map.b": {
        "type": "number",
        "value": 6
    }
}
```

### Template definition example
```json
"Example template": {
    "type": "template",
    "arguments": {
        "boolArg": {
            "type": "boolean",
            "value": false
        },
        "stringArg": {
            "type": "string",
            "value": "Default"
        },
        "mapArg": {
            "type": "object",
            "value": {
                "int": {
                    "type": "number",
                    "value": 0
                },
                "string": {
                    "type": "string",
                    "value": "Default"
                }
            }
        },
        "actionArg": {
            "type": "action"
        },
        "fileArg": {
            "type": "file"
        }
    }
}
```

## Syncing in-app templates to the dashboard

For the templates to be usable in campaigns they must be synced with the dashboard. When all templates and functions are defined, they can be synced by calling `CleverTap.SyncCustomTemplates()` in your Unity application. The syncing can only be done in debug builds and with an SDK user marked as a "test user". We recommend only calling this function while developing the templates and deleting the invocation in release builds.

## Presenting templates

When a custom template is triggered, the corresponding events `CleverTap.OnCustomTemplatePresent` or `CleverTap.OnCustomFunctionPresent` will be invoked. Applications with custom templates or functions must subscribe to those events. The handler is provided with `CleverTapTemplateContext` which can be used to interact with the template. When the event handler is invoked, this template will be considered "active" until `CleverTapTemplateContext.SetDismissed()` is called. While a template is "active" the `CleverTapTemplateContext` can be used to:

- Obtain argument values by using the appropriate `Get*(string argName)` methods.
- Trigger actions by their name through `TriggerAction(string argName)`.
- Set the state of the template invocation. `SetPresented()` and `SetDismissed()` notify the SDK of the state of the current template invocation. The presented state is when an in-app is displayed to the user and the dismissed state is when the in-app is no longer displayed.

Only one visual template or other InApp message can be displayed at a time by the SDK and no new messages can be shown until the current one is dismissed.

Applications should also subscribe to `CleverTap.OnCustomTemplateClose` which will be invoked when a template should be closed (this could occur when an action of type "close" is triggered). Use this listener to remove the UI associated with the template and call `CleverTapTemplateContext.SetDismissed()` to close it.

### Example

```csharp
CleverTap.OnCustomTemplatePresent += (CleverTapTemplateContext context) =>
{
    // show the UI for the template, be sure to keep the context as long as the template UI
    // is being displayed so that context.setDismissed() can be called when the UI is closed.
    ShowTemplateUi(context);
    // call customTemplateSetPresented when the UI has become visible to the user
   context.SetPresented();
};

CleverTap.OnCustomTemplateClose += (CleverTapTemplateContext context) =>
{
    // close the corresponding UI before calling customTemplateSetDismissed
    context.SetDismissed();
};
```

### In-App queue
When an in-app needs to be shown it is added to a queue (depending on its priority) and is displayed when all messages before it have been dismissed. The queue is saved in the storage and kept across app launches to ensure all messages are displayed when possible. The custom code in-apps behave in the same way. They will be triggered once their corresponding notification is the next one in the queue to be shown. However since the control of the dismissal is left to the application's code, the next in-app message will not be displayed until the current code template has called `CleverTapTemplateContext.SetDismissed()`

### File downloading and caching
File arguments are automatically downloaded and are ready for use when an in-app template is presented. The files are downloaded when a file argument has changed and this file is not already cached. For client-side in-apps, this happens at App Launch and is retried if needed when an in-app should be presented. For server-side in-apps, the file downloading happens only before presenting the in-app. If any of the file arguments of an in-app fails to be downloaded, the whole in-app is skipped and the custom template will not be triggered.
