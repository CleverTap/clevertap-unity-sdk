using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class NavigationMenu : MonoBehaviour
    {
        public Button SDK;
        public Button AppInbox;
        public Button Variables;
        public Button AdHoc;

        public List<GameObject> panels = new List<GameObject>();

        void Start()
        {
            SDK.onClick.AddListener(DidTapSDKButton);
            AppInbox.onClick.AddListener(DidTapAppInboxButton);
            Variables.onClick.AddListener(DidTapVariablesButton);
            AdHoc.onClick.AddListener(DidTapAdHocButton);

            panels.Add(GameObject.Find("AdHoc"));
            panels.Add(GameObject.Find("Variables"));
            panels.Add(GameObject.Find("AppInbox"));
            panels.Add(GameObject.Find("QASDK"));

            DidTapAdHocButton();
        }

        public void DidTapSDKButton()
        {
            var sdkPanel = FindPanel("QASDK");
            Enable(sdkPanel);
            sdkPanel.GetComponent<SDKPanel>().Restore();
        }

        public void DidTapAppInboxButton()
        {
            var appInboxPanel = FindPanel("AppInbox");
            Enable(appInboxPanel);
            appInboxPanel.GetComponent<AppInbox>().Restore();
        }

        public void DidTapVariablesButton()
        {
            var variables = FindPanel("Variables");
            Enable(variables);
            variables.GetComponent<Variables>().Restore();
        }

        public void DidTapAdHocButton()
        {
            var adHoc = FindPanel("AdHoc");
            Enable(adHoc);
        }

        void Enable(GameObject panel)
        {
            var currentPanels = panels.Where(p => p != null);
            foreach (var p in currentPanels)
            {
                if (p == panel)
                {
                    p.SetActive(true);
                }
                else
                {
                    p.SetActive(false);
                }
            }
        }

        GameObject FindPanel(string name)
        {
            return panels.Find((first) =>
            {
                return first.name == name;
            });
        }
    }
}
