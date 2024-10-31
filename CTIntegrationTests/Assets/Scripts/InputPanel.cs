using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTIntegrationTests
{
    public class InputPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_InputField textInput;
        [SerializeField] private TMP_Text placeholder;
        [SerializeField] private Button button;

        public delegate void OnButtonClicked(string text);
        public event OnButtonClicked OnButtonClickedEvent;

        public void SetTitle(string text)
        {
            title.SetText(text);
        }

        public void SetPlaceholder(string text)
        {
            placeholder.SetText(text);
        }

        public void SetButtonText(string text)
        {
            var buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.SetText(text);
        }

        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(ButtonClick);
        }

        void ButtonClick()
        {
            OnButtonClickedEvent?.Invoke(textInput.text);
        }
    }
}