using CleverTapSDK;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CleverTapTests : MonoBehaviour {
    public string CLEVERTAP_ACCOUNT_ID = "YOUR_CLEVERTAP_ACCOUNT_ID";
    public string CLEVERTAP_ACCOUNT_TOKEN = "YOUR_CLEVERTAP_ACCOUNT_TOKEN";
    public string CLEVERTAP_ACCOUNT_REGION = "";
    public LogLevel CLEVERTAP_LOG_LEVEL = 0;
    public int CLEVERTAP_DEBUG_LEVEL = 0;

    void Awake() {
        DontDestroyOnLoad(gameObject);

        //Disable CleverTap unity logger
        CleverTap.SetLogLevel(LogLevel.None);

        CleverTap.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);

        if (!string.IsNullOrEmpty(CLEVERTAP_ACCOUNT_REGION)) {
            CleverTap.LaunchWithCredentialsForRegion(CLEVERTAP_ACCOUNT_REGION, CLEVERTAP_ACCOUNT_TOKEN, CLEVERTAP_ACCOUNT_REGION);
        } else {
            CleverTap.LaunchWithCredentials(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN);
        }

        CleverTap.SetOptOut(false);
    }

    void Start() {
        TestVariables();
        TestDateSupport();
    }

    void TestVariables() {
        Var<string> var_string = CleverTap.Define("var_string", "hello, world");
        Var<int> var_int = CleverTap.Define("var_int", 10);
        Var<bool> var_bool = CleverTap.Define("var_bool", true);
        Var<float> var_float = CleverTap.Define("var_float", 5.0f);
        Var<double> var_double = CleverTap.Define("var_double", 55.999d);
        Var<short> var_short = CleverTap.Define("var_short", (short)1);
        Var<long> var_long = CleverTap.Define("var_long", (long)64);

        // Groups
        Var<string> varHello = CleverTap.Define("var.hello", "hello, group");

        Var<Dictionary<string, object>> var_dict = CleverTap.Define("var_dict", new Dictionary<string, object>() {
            { "nested_string", "hello, nested" },
            { "nested_double", 10.5d },
        });

        Var<Dictionary<string, object>> var_dict_complex = CleverTap.Define("var_dict_complex", new Dictionary<string, object>() {
            { "nested_int", 1 },
            { "nested_string", "hello, nested" },
            { "nested_map", new Dictionary<string, object>() {
                {"nested_map_int", 11 },
                {"nested_map_string", "hello, nested map" }
            }}
        });

        Var<string> var_dictNested_outside = CleverTap.Define("var_dict.nested_outside", "hello, outside");

        Var<Dictionary<string, object>> androidSamusng = CleverTap.Define("android.samsung", new Dictionary<string, object>());
        Var<string> androidSamsungS1 = CleverTap.Define("android.samsung.s1", "s1");
        Var<string> androidSamsungS2 = CleverTap.Define("android.samsung.s2", "s2");

        Var<string> varGroupVarGroup = CleverTap.Define("var.group.varGroup", "This is in a group.");
        Var<Dictionary<string, string>> varGroup = CleverTap.Define("var.group", new Dictionary<string, string>() {
            { "anotherInner", "This is also in a group" }
        });

        Var<int> group1Var1 = CleverTap.Define("group1.var1", 1);
        Var<int> group1Group2Var3 = CleverTap.Define("group1.group2.var3", 3);
        Var<Dictionary<string, object>> group1 = CleverTap.Define("group1", new Dictionary<string, object>() {
            { "var2", 2 },
            { "group1", new Dictionary<string, object>() {
                { "var4", 4 }
            }}
        });

        CleverTap.OnVariablesChanged += OnVariablesChanged;

        CleverTap.SyncVariables();
        CleverTap.FetchVariables(OnFetchVariablesCallback);

        var_string.OnValueChanged += Var_string_OnValueChanged;
        var_int.OnValueChanged += Var_int_OnValueChanged;
        var_bool.OnValueChanged += Var_bool_OnValueChanged;
        var_float.OnValueChanged += Var_float_OnValueChanged;
        var_double.OnValueChanged += Var_double_OnValueChanged;
        var_short.OnValueChanged += Var_short_OnValueChanged;
        var_long.OnValueChanged += Var_long_OnValueChanged;
    }

    public void TestDateSupport() {

        // Call OnUserLogin with date
        CleverTap.OnUserLogin(new Dictionary<string, object>() {
            { "DOB", new DateTime(2003, 02, 01) },
            { "SomeDate", DateTime.Now }
        });

        // Call ProfilePush with date
        CleverTap.ProfilePush(new Dictionary<string, object>() {
            { "DOB", new DateTime(2003, 02, 01) },
            { "SomeDate", DateTime.Now }
        });

        // Record Event with date
        CleverTap.RecordEvent("Date Support Test", new Dictionary<string, object>() {
            { "Date", new DateTime(2000, 01, 01) },
            { "DateNow", DateTime.Now },
            { "DateUtcNow", DateTime.UtcNow }
        });

        // Record charged event with date
        var chargeDetails = new Dictionary<string, object>(){
            { "Amount", 500 },
            { "Currency", "USD" },
            { "Payment Mode", "Credit card" },
            { "Date", new DateTime(2024, 01, 25) }
        };
        var items = new List<Dictionary<string, object>> {
            new Dictionary<string, object> {
            { "Price", 50 },
            { "Product category", "books" },
            { "Quantity", 1 }
        },
            new Dictionary<string, object> {
            { "Price", 100 },
            { "Product category", "plants" },
            { "Quantity", 10 }
            }
        };
        CleverTap.RecordChargedEventWithDetailsAndItems(chargeDetails, items);
    }

    private void OnVariablesChanged() {
        Debug.Log("Unity received variables changed");
    }

    void OnFetchVariablesCallback(bool isSuccess) {
        Debug.Log("unity received fetched variables is success: " + isSuccess);

        Var<int> var1 = CleverTap.Define("var1", 1);
        Debug.Log($"Name: {var1.Name}, Value: {var1.Value}");
        Debug.Log($"Name: {var1.Name} {var1.DefaultValue}, Value: {var1.Value}");

        Var<int> var2 = CleverTap.Define("var2", 2);
        Debug.Log($"Name: {var2.Name}, Value: {var2.Value}");
        Debug.Log($"Name: {var2.Name} {var2.DefaultValue}, Value: {var2.Value}");

        Var<int> var3 = CleverTap.Define("var3", 3);
        Debug.Log($"Name: {var3.Name}, Value: {var3.Value}");
        Debug.Log($"Name: {var3.Name} {var3.DefaultValue}, Value: {var3.Value}");

        Var<string> var_string = CleverTap.Define("var_string", "hello, world");
        Debug.Log($"Name: {var_string.Name}, Value: {var_string.Value}");
        Debug.Log($"Name: {var_string.Name} {var_string.DefaultValue}, Value: {var_string.Value}");

        Var<int> var_int = CleverTap.Define("var_int", 10);
        Debug.Log($"Name: {var_int.Name}, Value: {var_int.Value}");
        Debug.Log($"Name: {var_int.Name} {var_int.DefaultValue}, Value: {var_int.Value}");

        Var<bool> var_bool = CleverTap.Define("var_bool", true);
        Debug.Log($"Name: {var_bool.Name}, Value: {var_bool.Value}");
        Debug.Log($"Name: {var_bool.Name} {var_bool.DefaultValue}, Value: {var_bool.Value}");

        Var<float> var_float = CleverTap.Define("var_float", 5.0f);
        Debug.Log($"Name: {var_float.Name}, Value: {var_float.Value}");
        Debug.Log($"Name: {var_float.Name} {var_float.DefaultValue}, Value: {var_float.Value}");

        Var<double> var_double = CleverTap.Define("var_double", 55.999d);
        Debug.Log($"Name: {var_double.Name}, Value: {var_double.Value}");
        Debug.Log($"Name: {var_double.Name} {var_double.DefaultValue}, Value: {var_double.Value}");

        Var<short> var_short = CleverTap.Define("var_short", (short)1);
        Debug.Log($"Name: {var_short.Name}, Value: {var_short.Value}");
        Debug.Log($"Name: {var_short.Name} {var_short.DefaultValue}, Value: {var_short.Value}");

        Var<long> var_long = CleverTap.Define("var_long", (long)64);
        Debug.Log($"Name: {var_long.Name}, Value: {var_long.Value}");
        Debug.Log($"Name: {var_long.Name} {var_long.DefaultValue}, Value: {var_long.Value}");

        Var<Dictionary<string, object>> var_dict = CleverTap.Define("var_dict", new Dictionary<string, object>() {
            { "nested_string", "hello, nested" },
            { "nested_double", 10.5d },
        });
        Debug.Log($"Name: {var_dict.Name}, Value: {var_dict.Value}");
        Debug.Log($"Name: {var_dict.Name} {var_dict.DefaultValue}, Value: {var_dict.Value}");

        Var<Dictionary<string, object>> androidSamusng = CleverTap.Define("android.samsung", new Dictionary<string, object>());
        Debug.Log($"Name: {androidSamusng.Name}, Value: {androidSamusng.Value}");
        Debug.Log($"Name: {androidSamusng.Name} {androidSamusng.DefaultValue}, Value: {androidSamusng.Value}");

        Var<Dictionary<string, string>> varGroup = CleverTap.Define("var.group", new Dictionary<string, string>() {
            { "anotherInner", "This is also in a group" }
        });
        Debug.Log($"Name: {varGroup.Name}, Value: {varGroup.Value}");

        Var<string> varGroupVarGroup = CleverTap.Define("var.group.varGroup", "This is in a group.");
        Debug.Log($"Name: {varGroupVarGroup.Name} {varGroupVarGroup.DefaultValue}, Value: {varGroupVarGroup.Value}");

        Var<Dictionary<string, object>> var_dict_complex = CleverTap.Define("var_dict_complex", new Dictionary<string, object>() {
            { "nested_int", 1 },
            { "nested_string", "hello, nested" },
            { "nested_map", new Dictionary<string, object>() {
                {"nested_map_int", 11 },
                {"nested_map_string", "hello, nested map" }
            }}
        });
        Debug.Log($"Name: {var_dict_complex.Name}, Value: {var_dict_complex.Value}");
        Debug.Log($"Name: {var_dict_complex.Name} {var_dict_complex.DefaultValue["nested_string"]}, Value: {var_dict_complex.Value["nested_string"]}");
    }

    private void Var_string_OnValueChanged() {
        Debug.Log("Unity received variable changed var_string");
    }

    private void Var_int_OnValueChanged() {
        Debug.Log("Unity received variable changed var_int");
    }

    private void Var_bool_OnValueChanged() {
        Debug.Log("Unity received variable changed var_bool");
    }

    private void Var_float_OnValueChanged() {
        Debug.Log("Unity received variable changed var_float");
    }

    private void Var_double_OnValueChanged() {
        Debug.Log("Unity received variable changed var_double");
    }

    private void Var_short_OnValueChanged() {
        Debug.Log("Unity received variable changed var_short");
    }

    private void Var_long_OnValueChanged() {
        Debug.Log("Unity received variable changed var_long");
    }
}
