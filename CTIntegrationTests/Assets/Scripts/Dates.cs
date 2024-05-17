using CleverTapSDK;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Dates : MonoBehaviour {
    public GameObject ButtonPrefab;
    public VerticalLayoutGroup VerticalLayoutGroup;

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

        var profilePushButton = AddButton("ProfilePush", "Profile Push");
        profilePushButton.GetComponent<Button>().onClick.AddListener(ProfilePush);
        
        var recordEventButton = AddButton("RecordEvent", "Record Event");
        recordEventButton.GetComponent<Button>().onClick.AddListener(RecordEvent);

        var recordChargedEventWithDetailsAndItemsButton = AddButton("RecordChargedEventWithDetailsAndItems", "Record Charged Event With Details And Items");
        recordChargedEventWithDetailsAndItemsButton.GetComponent<Button>().onClick.AddListener(RecordChargedEventWithDetailsAndItems);

        var customEventsButton = AddButton("CustomEvents", "Try custom events");
        customEventsButton.GetComponent<Button>().onClick.AddListener(RecordCustomeEvents);
    }

    private GameObject AddButton(string name, string text) {
        var parent = VerticalLayoutGroup.GetComponent<RectTransform>();

        var button = Instantiate(ButtonPrefab);
        button.name = name;
        button.transform.SetParent(parent, false);
        button.GetComponentInChildren<Text>().text = text;

        return button;
    }

    private void ProfilePush() {
        CleverTap.ProfilePush(new Dictionary<string, object>() {
            { "DOB", new DateTime(2003, 02, 01) },
            { "SomeDate", DateTime.Now }
        });
    }

    private void RecordEvent() {
        CleverTap.RecordEvent("Date Support Test", new Dictionary<string, object>() {
            { "Date", new DateTime(2000, 01, 01) },
            { "DateNow", DateTime.Now },
            { "DateUtcNow", DateTime.UtcNow }
        });
    }

    private void RecordChargedEventWithDetailsAndItems() {
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

    public GameObject CustomEventsParent;

    public void CloseCustomEventPanel()
    {
        CustomEventsParent.SetActive(false);
    }
    private void RecordCustomeEvents()
    {
        CustomEventsParent.SetActive(true);
    }
}
