using CleverTapSDK;
using UnityEngine;

namespace CTExample
{
    [RequireComponent(typeof(InputPanel))]
    public class RemoveProperty : MonoBehaviour
    {
        private InputPanel panel;

        private void Awake()
        {
            panel = GetComponent<InputPanel>();
        }

        void Start()
        {
            panel.SetTitle("Remove Property");
            panel.SetPlaceholder("propertyName");
            panel.SetButtonText("Remove");

            panel.OnButtonClickedEvent += OnButtonClick;
        }

        void OnButtonClick(string text)
        {
            CleverTap.ProfileRemoveValueForKey(text);
            Logger.Log($"Remove profile key: {text}");
        }
    }
}