using System.Collections.Generic;
using CleverTapSDK;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class Variables : MonoBehaviour
    {
        public GameObject ButtonPrefab;
        public VerticalLayoutGroup VerticalLayoutGroup;

        public GameObject keyValuePrefab;

        private GameObject _fetchButton;
        private static bool VariablesDefined = false;

        // Variables
        Var<string> var_string;
        Var<int> var_int;
        Var<bool> var_bool;
        Var<float> var_float;
        Var<double> var_double;
        Var<short> var_short;
        Var<long> var_long;

        Var<Dictionary<string, object>> var_dict;

        Var<Dictionary<string, object>> var_dict_complex;

        Var<string> var_dictNested_outside;

        Var<Dictionary<string, object>> androidSamsung;
        Var<string> androidSamsungS1;
        Var<string> androidSamsungS2;

        Var<string> varGroupVarGroup;
        Var<Dictionary<string, string>> varGroup;

        Var<int> group1Var1;
        Var<int> group1Group2Var3;
        Var<Dictionary<string, object>> group1;

        Var<string> varHello;

        Var<string> factory_var_file;
        Var<string> folder1FileVariable;

        void Start()
        {
            InitPanel();
        }

        public void Restore()
        {
            InitPanel();
        }

        private void InitPanel()
        {
            foreach (Transform child in VerticalLayoutGroup.GetComponent<RectTransform>())
            {
                Destroy(child.gameObject);
            }

            var defineVariablesButton = AddButton("DefineVariables", "Define Variables");
            defineVariablesButton.GetComponent<Button>().onClick.AddListener(DefineVariables);

            var syncVariablesButton = AddButton("SyncVariables", "Sync Variables");
            syncVariablesButton.GetComponent<Button>().onClick.AddListener(SyncVariables);

            var fetchVariablesButton = AddButton("FetchVariables", "Fetch Variables");
            fetchVariablesButton.GetComponent<Button>().onClick.AddListener(FetchVariables);
            _fetchButton = fetchVariablesButton;

            var printVariablesButton = AddButton("PrintVariables", "Print Variables");
            printVariablesButton.GetComponent<Button>().onClick.AddListener(PrintVariables);
        }

        private GameObject AddButton(string name, string text)
        {
            var parent = VerticalLayoutGroup.GetComponent<RectTransform>();

            var button = Instantiate(ButtonPrefab);
            button.name = name;
            button.transform.SetParent(parent, false);
            button.GetComponentInChildren<Text>().text = text;

            return button;
        }

        private void DefineVariables()
        {
            var_string = CleverTap.Define("var_string", "hello, world");
            var_int = CleverTap.Define("var_int", 10);
            var_bool = CleverTap.Define("var_bool", true);
            var_float = CleverTap.Define("var_float", 5.0f);
            var_double = CleverTap.Define("var_double", 55.999d);
            var_short = CleverTap.Define("var_short", (short)1);
            var_long = CleverTap.Define("var_long", (long)64);

            // Groups
            varHello = CleverTap.Define("var.hello", "hello, group");

            var_dict = CleverTap.Define("var_dict", new Dictionary<string, object>() {
                { "nested_string", "hello, nested" },
                { "nested_double", 10.5d },
            });

            var_dict_complex = CleverTap.Define("var_dict_complex", new Dictionary<string, object>() {
                { "nested_int", 1 },
                { "nested_string", "hello, nested" },
                { "nested_map", new Dictionary<string, object>() {
                    {"nested_map_int", 11 },
                    {"nested_map_string", "hello, nested map" }
                }}
            });

            var_dictNested_outside = CleverTap.Define("var_dict.nested_outside", "hello, outside");

            androidSamsung = CleverTap.Define("android.samsung", new Dictionary<string, object>());
            androidSamsungS1 = CleverTap.Define("android.samsung.s1", "s1");
            androidSamsungS2 = CleverTap.Define("android.samsung.s2", "s2");

            varGroupVarGroup = CleverTap.Define("var.group.varGroup", "This is in a group.");
            varGroup = CleverTap.Define("var.group", new Dictionary<string, string>() {
                { "anotherInner", "This is also in a group" }
            });

            group1Var1 = CleverTap.Define("group1.var1", 1);
            group1Group2Var3 = CleverTap.Define("group1.group2.var3", 3);
            group1 = CleverTap.Define("group1", new Dictionary<string, object>() {
                { "var2", 2 },
                { "group1", new Dictionary<string, object>() {
                    { "var4", 4 }
                }}
            });

            factory_var_file = CleverTap.DefineFileVariable("factory_var_file");
            folder1FileVariable = CleverTap.DefineFileVariable("folder1.fileVariable");

            // Set Callbacks
            CleverTap.OnVariablesChanged += OnVariablesChanged;
            CleverTap.OnOneTimeVariablesChanged += OnOneTimeVariablesChanged;
            CleverTap.OnVariablesChangedAndNoDownloadsPending += OnVariablesChangedAndNoDownloadsPending;
            CleverTap.OnOneTimeVariablesChangedAndNoDownloadsPending += OnOneTimeVariablesChangedAndNoDownloadsPending;
            var_string.OnValueChanged += Var_string_OnValueChanged;
            var_int.OnValueChanged += Var_int_OnValueChanged;
            var_bool.OnValueChanged += Var_bool_OnValueChanged;
            var_float.OnValueChanged += Var_float_OnValueChanged;
            var_double.OnValueChanged += Var_double_OnValueChanged;
            var_short.OnValueChanged += Var_short_OnValueChanged;
            var_long.OnValueChanged += Var_long_OnValueChanged;
            factory_var_file.OnFileReady += Factory_var_file_OnFileReady;

            VariablesDefined = true;

            BuildVarsUI();
        }

        private void SyncVariables()
        {
#if UNITY_ANDROID
        CleverTap.SyncVariables();
#elif UNITY_IOS
            CleverTap.SyncVariables(true);
#endif
        }

        private void FetchVariables()
        {
            var button = _fetchButton.GetComponent<Button>();
            button.interactable = false;
            button.GetComponentInChildren<Text>().text = "Fetching Variables In Progress...";
            CleverTap.FetchVariables(OnFetchVariablesCallback);
        }

        private void BuildVarsUI()
        {
            // Clear the values
            for (int i = 0; i < VerticalLayoutGroup.transform.childCount; i++)
            {
                GameObject child = VerticalLayoutGroup.transform.GetChild(i).gameObject;
                // Do not delete the buttons
                if (child.GetComponent<Button>() == null)
                {
                    Destroy(child);
                }
            }

            Var<int> var1 = CleverTap.Define("var1", 1);
            AddKeyValue(var1.Name, var1.Value.ToString());

            Var<int> var2 = CleverTap.Define("var2", 2);
            AddKeyValue(var2.Name, var2.Value.ToString());

            Var<int> var3 = CleverTap.Define("var3", 3);
            AddKeyValue(var3.Name, var3.Value.ToString());

            AddKeyValue(var_string.Name, var_string.Value?.ToString());
            AddKeyValue(var_int.Name, var_int.Value.ToString());
            AddKeyValue(var_bool.Name, var_bool.Value.ToString());
            AddKeyValue(var_float.Name, var_float.Value.ToString());
            AddKeyValue(var_double.Name, var_double.Value.ToString());
            AddKeyValue(var_short.Name, var_short.Value.ToString());
            AddKeyValue(var_long.Name, var_long.Value.ToString());

            AddKeyValue(var_dict.Name, Json.Serialize(var_dict.Value));
            AddKeyValue(androidSamsung.Name, Json.Serialize(androidSamsung.Value));
            AddKeyValue(varGroup.Name, Json.Serialize(varGroup.Value));
            AddKeyValue(varGroupVarGroup.Name, Json.Serialize(varGroupVarGroup.Value));
            AddKeyValue(var_dict_complex.Name, Json.Serialize(var_dict_complex.Value));
            AddKeyValue($"{var_dict_complex.Name}.nested_string", Json.Serialize(var_dict_complex.Value["nested_string"]));
            AddKeyValue(var_dictNested_outside.Name, Json.Serialize(var_dictNested_outside.Value));

            AddKeyValue(androidSamsungS1.Name, Json.Serialize(androidSamsungS1.Value));
            AddKeyValue(androidSamsungS2.Name, Json.Serialize(androidSamsungS2.Value));
            AddKeyValue(group1Var1.Name, Json.Serialize(group1Var1.Value));
            AddKeyValue(group1Group2Var3.Name, Json.Serialize(group1Group2Var3.Value));
            AddKeyValue(group1.Name, Json.Serialize(group1.Value));
            AddKeyValue(varHello.Name, Json.Serialize(varHello.Value));

            AddKeyValue(factory_var_file.Name, factory_var_file.Value);
            AddKeyValue(factory_var_file.Name, factory_var_file.FileValue);

            AddKeyValue(folder1FileVariable.Name, folder1FileVariable.FileValue);
        }

        private void AddKeyValue(string name, string value)
        {
            var parent = VerticalLayoutGroup.GetComponent<RectTransform>();
            GameObject sdkVersion = Instantiate(keyValuePrefab);
            KeyValue sdkVersionKV = sdkVersion.GetComponent<KeyValue>();
            sdkVersionKV.SetKey(name);
            sdkVersionKV.SetValue(value);
            sdkVersion.transform.SetParent(parent, false);
        }

        private void PrintVariables()
        {
            if (!VariablesDefined)
            {
                Logger.LogWarning("Define variables first.");
                return;
            }

            Var<int> var1 = CleverTap.Define("var1", 1);
            Logger.Log($"Name: {var1.Name}, Default Value:  {var1.DefaultValue}, Value: {var1.Value}");

            Var<int> var2 = CleverTap.Define("var2", 2);
            Logger.Log($"Name: {var2.Name}, Default Value: {var2.DefaultValue}, Value: {var2.Value}");

            Var<int> var3 = CleverTap.Define("var3", 3);
            Logger.Log($"Name: {var3.Name}, Default Value: {var3.DefaultValue}, Value: {var3.Value}");

            Logger.Log($"Name: {var_string.Name}, Default Value: {var_string.DefaultValue}, Value: {var_string.Value}");

            Logger.Log($"Name: {var_int.Name}, Default Value: {var_int.DefaultValue}, Value: {var_int.Value}");

            Logger.Log($"Name: {var_bool.Name}, Default Value: {var_bool.DefaultValue}, Value: {var_bool.Value}");

            Logger.Log($"Name: {var_float.Name}, Default Value: {var_float.DefaultValue}, Value: {var_float.Value}");

            Logger.Log($"Name: {var_double.Name}, Default Value: {var_double.DefaultValue}, Value: {var_double.Value}");

            Logger.Log($"Name: {var_short.Name}, Default Value: {var_short.DefaultValue}, Value: {var_short.Value}");

            Logger.Log($"Name: {var_long.Name}, Default Value: {var_long.DefaultValue}, Value: {var_long.Value}");

            Logger.Log($"Name: {var_dict.Name}, Default Value: {Json.Serialize(var_dict.DefaultValue)}, Value: {Json.Serialize(var_dict.Value)}");

            Logger.Log($"Name: {androidSamsung.Name}, Default Value: {Json.Serialize(androidSamsung.DefaultValue)}, Value: {Json.Serialize(androidSamsung.Value)}");

            Logger.Log($"Name: {varGroup.Name}, Default Value: {Json.Serialize(varGroup.DefaultValue)}, Value: {Json.Serialize(varGroup.Value)}");

            Logger.Log($"Name: {varGroupVarGroup.Name}, Default Value: {Json.Serialize(varGroupVarGroup.DefaultValue)}, Value: {Json.Serialize(varGroupVarGroup.Value)}");

            Logger.Log($"Name: {var_dict_complex.Name}, Default Value: {Json.Serialize(var_dict_complex.DefaultValue)}, Value: {Json.Serialize(var_dict_complex.Value)}");
            Logger.Log($"Name: {var_dict_complex.Name}.nested_string, Default Value: {Json.Serialize(var_dict_complex.DefaultValue["nested_string"])}, Value: {Json.Serialize(var_dict_complex.Value["nested_string"])}");

            Logger.Log($"Name: {var_dictNested_outside.Name}, Default Value: {var_dictNested_outside.DefaultValue}, Value: {var_dictNested_outside.Value}");

            Logger.Log($"Name: {androidSamsungS1.Name}, Default Value: {androidSamsungS1.DefaultValue}, Value: {androidSamsungS1.Value}");
            Logger.Log($"Name: {androidSamsungS2.Name}, Default Value: {androidSamsungS2.DefaultValue}, Value: {androidSamsungS2.Value}");

            Logger.Log($"Name: {group1Var1.Name}, Default Value: {group1Var1.DefaultValue}, Value: {group1Var1.Value}");
            Logger.Log($"Name: {group1Group2Var3.Name}, Default Value: {group1Group2Var3.DefaultValue}, Value: {group1Group2Var3.Value}");
            Logger.Log($"Name: {group1.Name}, Default Value: {Json.Serialize(group1.DefaultValue)}, Value: {Json.Serialize(group1.Value)}");

            Logger.Log($"Name: {varHello.Name}, Default Value: {varHello.DefaultValue}, Value: {varHello.Value}");
        }

        private void OnVariablesChanged()
        {
            Logger.Log("[SAMPLE] OnVariablesChanged: Unity received variables changed");
            BuildVarsUI();
        }

        private void OnOneTimeVariablesChanged()
        {
            Logger.Log("[SAMPLE] OnOneTimeVariablesChanged: Unity received variables changed");
        }

        private void OnOneTimeVariablesChangedAndNoDownloadsPending()
        {
            Logger.Log("[SAMPLE] OnOneTimeVariablesChangedAndNoDownloadsPending: Unity received variables changed");
        }

        private void OnVariablesChangedAndNoDownloadsPending()
        {
            Logger.Log("[SAMPLE] OnVariablesChangedAndNoDownloadsPending: Unity received variables changed");
        }

        void OnFetchVariablesCallback(bool isSuccess)
        {
            Logger.Log("On Fetched Variables is success: " + isSuccess);
            var button = _fetchButton.GetComponent<Button>();
            button.interactable = true;
            button.GetComponentInChildren<Text>().text = "Fetch Variables";
        }

        private void Var_string_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_string");
        }

        private void Var_int_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_int");
        }

        private void Var_bool_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_bool");
        }

        private void Var_float_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_float");
        }

        private void Var_double_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_double");
        }

        private void Var_short_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_short");
        }

        private void Var_long_OnValueChanged()
        {
            Logger.Log("Unity received variable changed var_long");
        }

        private void Factory_var_file_OnFileReady()
        {
            Logger.Log("Unity received file ready factory_var_file");
        }
    }
}