using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using CleverTapSDK.Utilities;

public class AnalyticsUIManager : MonoBehaviour
{
    private string eventName = "";
    private List<ParameterRow> parameterRows = new List<ParameterRow>();
    private Vector2 scrollPosition;
    private string log = "";
    private Vector2 logScrollPosition;
    private bool isProfilePush = false;

    private void OnGUI()
    {
        // Define proportions
        float screenHeight = Screen.height;
        float loggedEventsHeight = screenHeight * 0.25f;
        float eventCreationHeight = screenHeight * 0.25f;

        // Set font sizes for better visibility on mobile screens
        GUI.skin.label.fontSize = 24;
        GUI.skin.button.fontSize = 30;
        GUI.skin.textField.fontSize = 24;
        GUI.skin.toggle.fontSize = 24;

        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, screenHeight - 20));

        GUILayout.BeginVertical();

        // Logged Events ScrollView
        GUILayout.Label("Logged Events:");
        logScrollPosition = GUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(loggedEventsHeight));
        GUILayout.Label(log);
        GUILayout.EndScrollView();

        // Create Event Button
        if (GUILayout.Button("Create Event", GUILayout.Height(40)))
        {
            eventName = "";
            parameterRows.Clear();
            isProfilePush = false;
        }

        // Event Creation UI
        GUILayout.Space(10);

        // Profile Push Checkbox
        isProfilePush = GUILayout.Toggle(isProfilePush, "Profile Push");

        if (!isProfilePush)
        {
            GUILayout.Label("Event Name:");
            GUI.enabled = !isProfilePush;  // Disable the event name input if Profile Push is checked
            eventName = GUILayout.TextField(eventName, GUILayout.Height(40));
            GUI.enabled = true;  // Re-enable GUI for other elements

        }

        GUILayout.Label("Parameters:");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(eventCreationHeight));
        foreach (var parameterRow in parameterRows)
        {
            parameterRow.OnGUI();
            GUILayout.Space(2); // Reduce gap between rows
        }
        GUILayout.EndScrollView();

        // Add Parameter Button
        if (GUILayout.Button("Add Parameter", GUILayout.Height(40)))
        {
            parameterRows.Add(new ParameterRow());
        }

        // Post Event Button
        if (GUILayout.Button("Post Event", GUILayout.Height(40)))
        {
            PostAnalytics();
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    private void PostAnalytics()
    {
        if (isProfilePush)
        {
            ProfilePush();
            return;
        }

        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Event name cannot be empty.");
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        foreach (var row in parameterRows)
        {
            string key = row.GetKey();
            object value = row.GetValue();
            if (!string.IsNullOrEmpty(key) && value != null)
            {
                parameters[key] = value;
            }
        }

        // Log the analytics event
        string logEntry = $"Event: {eventName}\nParameters: {ParametersToString(parameters)}";
        log += logEntry + "\n\n";

        CleverTapSDK.CleverTap.RecordEvent(eventName, parameters);

        eventName = "";
        parameterRows.Clear();
        isProfilePush = false;
    }

    private void ProfilePush()
    {
        Dictionary<string, object> profileData = new Dictionary<string, object>();
        foreach (var row in parameterRows)
        {
            string key = row.GetKey();
            object value = row.GetValue();
            if (!string.IsNullOrEmpty(key) && value != null)
            {
                profileData[key] = value;
            }
        }

        // Log the profile push
        string logEntry = $"Profile Push\nData: {ParametersToString(profileData)}";
        log += logEntry + "\n\n";

        // Optionally, send this data to your profile push service here.
        CleverTapSDK.CleverTap.ProfilePush(profileData);
        eventName = "";
        parameterRows.Clear();
        isProfilePush = false;
    }

    private string ParametersToString(Dictionary<string, object> parameters)
    {
       return Json.Serialize(parameters);
    }
}

public class ParameterRow
{
    private string key = "";
    private string value = "";
    private string error = "";

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Key:", GUILayout.Width(70));
        key = GUILayout.TextField(key, GUILayout.Width(100), GUILayout.Height(35));
        GUILayout.Label("Value:", GUILayout.Width(70));
        value = GUILayout.TextField(value, GUILayout.Width(100), GUILayout.Height(35));
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(error))
        {
            GUILayout.Label($"<color=red>{error}</color>", GUILayout.ExpandWidth(true));
        }
    }

    public string GetKey()
    {
        return key;
    }

    public object GetValue()
    {
        error = "";

        if (int.TryParse(value, out int intValue))
        {
            return intValue;
        }
        else if (float.TryParse(value, out float floatValue))
        {
            return floatValue;
        }
        else if (long.TryParse(value, out long longValue))
        {
            return longValue;
        }
        else if (double.TryParse(value, out double doubleValue))
        {
            return doubleValue;
        }
        else if (DateTime.TryParse(value, out DateTime dateTimeValue))
        {
            return dateTimeValue;
        }

        return value; // Default to string if no other type matches
    }
}
