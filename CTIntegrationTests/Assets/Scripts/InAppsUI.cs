using CleverTapSDK.Common;
using CleverTapSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CleverTapSDK.Utilities;

public class InAppsUI : MonoBehaviour {
    public GameObject ButtonPrefab;
    public VerticalLayoutGroup VerticalLayoutGroup;

    private GameObject _fetchButton;

    void Start() {

        InitPanel();
    }

    public void Restore() {
        InitPanel();
    }

    private void InitPanel() {
        foreach (Transform child in VerticalLayoutGroup.GetComponent<RectTransform>()) {
            Destroy(child.gameObject);
        }

        var fetchInAppsButton = AddButton("FetchInApps", "Fetch InApp");
        fetchInAppsButton.GetComponent<Button>().onClick.AddListener(FetchInApps);
        
        var clearInAppResourcesButton = AddButton("Clear Resources", "Clear Resources");
        clearInAppResourcesButton.GetComponent<Button>().onClick.AddListener(ClearInAppResources);

    }

    private GameObject AddButton(string name, string text) {
        var parent = VerticalLayoutGroup.GetComponent<RectTransform>();

        var button = Instantiate(ButtonPrefab);
        button.name = name;
        button.transform.SetParent(parent, false);
        button.GetComponentInChildren<Text>().text = text;

        return button;
    }

    private void FetchInApps() {
        CleverTap.FetchInApps((status)=>{
            Debug.Log("Fetch InApps Status"+status);
        });
    }

    private void ClearInAppResources() {
         Debug.Log("Clear InApp Resources");
        CleverTap.ClearInAppResources(false);
    }
}
