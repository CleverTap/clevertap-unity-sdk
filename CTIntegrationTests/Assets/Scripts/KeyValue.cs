using TMPro;
using UnityEngine;

namespace CTIntegrationTests
{
    public class KeyValue : MonoBehaviour
    {
        [SerializeField] private TMP_Text KeyText;
        [SerializeField] private TMP_Text ValueText;

        void Start()
        {
            KeyText.GetComponent<ClickableText>().OnTextClickedEvent += KeyValue_OnTextClickedEvent;
            ValueText.GetComponent<ClickableText>().OnTextClickedEvent += KeyValue_OnTextClickedEvent;

            RefreshContentHelper.RefreshContentFitters((RectTransform)transform);
        }

        public string GetKey()
        {
            return KeyText.text;
        }

        public void SetKey(string text)
        {
            if (text != KeyText.text)
            {
                KeyText.SetText(text);
            }
        }

        public string GetValue()
        {
            return ValueText.text;
        }

        public void SetValue(string text)
        {
            if (text != ValueText.text)
            {
                ValueText.SetText(text);
            }
        }

        private void KeyValue_OnTextClickedEvent(string text)
        {
            TextEditor te = new TextEditor();
            te.text = text;
            te.SelectAll();
            te.Copy();

            Debug.Log($"[SAMPLE] Copied: {text}");
        }
    }
}