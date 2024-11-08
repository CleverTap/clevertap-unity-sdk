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

        private bool refreshedContentFitter = false;

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

        public void AddAdditionalButton(string name, string text, OnButtonClicked onButtonClicked)
        {
            VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();

            Button newButton = Instantiate(button);
            newButton.name = name;
            newButton.GetComponentInChildren<TMP_Text>().text = text;
            newButton.onClick.AddListener(() =>
            {
                onButtonClicked?.Invoke(textInput.text);
            });
            newButton.transform.SetParent(layout.transform, false);

            // Button was added after Start, refresh the content to fit the additional button
            if (refreshedContentFitter)
            {
                RefreshContentHelper.RefreshContentFitters((RectTransform)transform);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            RefreshContentHelper.RefreshContentFitters((RectTransform)transform);
            refreshedContentFitter = true;
            button.onClick.AddListener(ButtonClick);
        }

        void ButtonClick()
        {
            OnButtonClickedEvent?.Invoke(textInput.text);
        }
    }
}