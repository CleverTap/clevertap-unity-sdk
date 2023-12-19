using CleverTapSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMenu : MonoBehaviour {
    public Button Login;
    public Button Dates;
    public Button Variables;

    public List<GameObject> Panels = new List<GameObject>();

    void Start() {
        Login.onClick.AddListener(didTapLoginButton);
        Dates.onClick.AddListener(didTapDatesButton);
        Variables.onClick.AddListener(didTapVariablesButton);

        Panels.Add(GameObject.Find("Login"));
        Panels.Add(GameObject.Find("Dates"));
        Panels.Add(GameObject.Find("Variables"));

        DisableAllPanels();
        var loginPanel = FindPanel("Login");
        Enable(loginPanel);

        Dates.gameObject.SetActive(false);
        Variables.gameObject.SetActive(false);
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

        Login.gameObject.SetActive(false);
        Dates.gameObject.SetActive(true);
        Variables.gameObject.SetActive(true);

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
