using CleverTapSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMenu : MonoBehaviour {
    public Button Login;
    public Button Events;
    public Button Dates;
    public Button Variables;
    public Button InApps;
    public Button ResetId;

    public List<GameObject> Panels = new List<GameObject>();

    public string CTID { get; private set; }

    void Start() {
        Login.onClick.AddListener(didTapLoginButton);
        Dates.onClick.AddListener(didTapDatesButton);
        Variables.onClick.AddListener(didTapVariablesButton);
        InApps.onClick.AddListener(didTapInAppButton);
        Events.onClick.AddListener(didTapEventsButton);
        ResetId.onClick.AddListener(resetDeviceId);

        Panels.Add(GameObject.Find("Login"));
        Panels.Add(GameObject.Find("Dates"));
        Panels.Add(GameObject.Find("Variables"));
        Panels.Add(GameObject.Find("InApps"));
        //Panels.Add(GameObject.Find("Events"));

        DisableAllPanels();
        var loginPanel = FindPanel("Login");
        Enable(loginPanel);

        Dates.gameObject.SetActive(false);
        Variables.gameObject.SetActive(false);
        InApps.gameObject.SetActive(false);

        
#if UNITY_ANDROID
       CleverTap.OnCleverTapInitCleverTapIdCallback += CleverTap_OnCleverTapInitCleverTapIdCallback;
#elif UNITY_IOS
        CTID = CleverTap.ProfileGetCleverTapID();
#else
    CTID = CleverTap.GetCleverTapID();
#endif
    }

    private void CleverTap_OnCleverTapInitCleverTapIdCallback(string message)
    {
        var messageJson = JsonUtility.FromJson<Dictionary<string, object>>(message);
        CTID = messageJson["cleverTapID"]?.ToString();
        Debug.Log($"[Demo] CT_ID : {CTID}");
    }

    public void didTapLoginButton() {
        var login = FindPanel("Login").GetComponent<Login>();
        if (string.IsNullOrEmpty(login.Email.text) || string.IsNullOrEmpty(login.Identity.text)) {
            Debug.Log("Email and Identity is required!");
        }

        CleverTap.OnUserLogin(new Dictionary<string, object>() {
            { "email", login.Email.text },
            { "identity", login.Identity.text },
            { "DOB", new DateTime(2003, 02, 01) },
            { "SomeDate", DateTime.Now }
        });
        Debug.Log($"[Demo] CT_ID : {CTID}");

        Login.gameObject.SetActive(false);
        Dates.gameObject.SetActive(false);
        Variables.gameObject.SetActive(true);
        InApps.gameObject.SetActive(true);

        DisableAllPanels();
        didTapDatesButton();
    }

    public void didTapDatesButton() {
        DisableAllPanels();

        var datesPanel = FindPanel("Dates");
        Enable(datesPanel);
        datesPanel.GetComponent<Dates>().Restore();
    }

    public void didTapVariablesButton() {
        DisableAllPanels();

        var variablesPanel = FindPanel("Variables");
        Enable(variablesPanel);
        variablesPanel.GetComponent<Variables>().Restore();
    }

    public void didTapInAppButton() {
        DisableAllPanels();

        var inAppPanel = FindPanel("InApps");
        Enable(inAppPanel);
        inAppPanel.GetComponent<InAppsUI>().Restore();
    }
    public void didTapEventsButton()
    {
        DisableAllPanels();

        var eventsPanel = FindPanel("Events");
        Enable(eventsPanel);
        eventsPanel.GetComponent<EventsUI>().Restore();
    }

    public void resetDeviceId()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey("device_id");
        PlayerPrefs.Save();
        Debug.Log("RESET ID"+CleverTap.GetCleverTapID());
    }

    private void Enable(GameObject panel) {
        if (panel != null)
            Panels.SingleOrDefault(p => p == panel)?.SetActive(true);
    }

    private void DisableAllPanels() {
        Panels.ForEach(p => p.SetActive(false));
    }

    private GameObject FindPanel(string name) =>
        Panels.Find((p) => p.name == name);
}
