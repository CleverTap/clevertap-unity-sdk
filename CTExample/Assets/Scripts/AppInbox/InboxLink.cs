using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CleverTapSDK.CleverTapInboxMessage;

namespace CTExample
{
    public class InboxLink : MonoBehaviour
    {
        [SerializeField] private ClickableText ClickableText = null;
        [SerializeField] private TMP_Text _linkText;
        private Link _link;

        public void Initialize(Link link)
        {
            _linkText.SetText(link.Text);
            _link = link;
            ClickableText.OnTextClickedEvent += OnLinkClick;
        }

        public void OnLinkClick(string text)
        {
            if (_link != null)
            {
                switch (_link.Type)
                {
                    case LinkType.URL:
                        string url = null;

#if UNITY_ANDROID
                        url = _link.Url.Android.Text;
#elif UNITY_IOS
                        url = _link.Url.IOS.Text;
#else
                        url = _link.Url.Android.Text;
#endif
                        if (!string.IsNullOrEmpty(url))
                            Application.OpenURL(url);

                        break;
                 
                    case LinkType.CopyText:
                        CopyText(_link.CopyText.Text);
                        break;
                    case LinkType.KeyValue:
                        string keyvalues = string.Empty;

                        foreach (KeyValuePair<string, string> kvp in _link.KeyValuePairs)
                        {
                            Logger.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
                            keyvalues += $"Key: {kvp.Key}, Value: {kvp.Value} \n";
                        }

                        Toast.Show(keyvalues);
                        break;

                    default:
                        Toast.Show("Unknown link type");
                        break;
                }
            }
        }

        private void OnDisable()
        {
            ClickableText.OnTextClickedEvent -= OnLinkClick;
        }

        private void CopyText(string text)
        {
            TextEditor te = new TextEditor();
            te.text = text;
            te.SelectAll();
            te.Copy();
            Logger.Log($"Copied: {text}");
            Toast.Show($"Copied: {text}");
        }

        private class LinkType
        {
            public const string URL = "url";
            public const string CopyText = "copy";
            public const string KeyValue = "kv";
        }
    }
}