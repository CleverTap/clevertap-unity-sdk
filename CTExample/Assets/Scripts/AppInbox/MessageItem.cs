using System;
using System.Collections.Generic;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CleverTapSDK.CleverTapInboxMessage;

namespace CTExample
{
    public sealed class MessageItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText = null;
        [SerializeField] private TMP_Text _messageText = null;
        [SerializeField] private Button _readButton = null;
        [SerializeField] private Button _deleteButton = null;
        [SerializeField] private GameObject _unReadIndicator = null;
        [SerializeField] private Button _callToActionButton = null;
        [SerializeField] private Transform _linksParent = null;
        [SerializeField] private InboxLink _linkPrefab = null;
        [SerializeField] private TMP_Text _deliveryTimeText = null;

        private List<InboxLink> inboxLinks = new List<InboxLink>();
        private MessageView _messageView = null;
        private CleverTapInboxMessage _messageData = null;
        private Action<string> _onReadAction = null;
        private Action<string> _onDeleteAction = null;
        private Content _messageContent = null;

        public string MessageId => _messageData?.Id;

        private void OnEnable()
        {
            _readButton.onClick.AddListener(OnReadButtonClick);
            _deleteButton.onClick.AddListener(OnDeleteButtonClick);
            _callToActionButton.onClick.AddListener(OnCallToAction);
        }

        private void OnDisable()
        {
            _readButton.onClick.RemoveListener(OnReadButtonClick);
            _deleteButton.onClick.RemoveListener(OnDeleteButtonClick);
            _callToActionButton.onClick.RemoveListener(OnCallToAction);
        }

        public void Initialize(MessageView messageView, CleverTapInboxMessage itemData, Action<string> OnReadAction, Action<string> OnDeleteAction)
        {
            _messageView = messageView;
            _messageData = itemData;
            _onReadAction = OnReadAction;
            _onDeleteAction = OnDeleteAction;
            _messageContent = itemData.Message.Content[0];

            _titleText.text = _messageContent.Title.Text;
            _messageText.text = _messageContent.Message.Text;

            _unReadIndicator.SetActive(!itemData.IsRead);

            if (_messageContent.Action.HasLinks)
            {
                CreateLinks();
            }
            else
            {
                _linksParent.gameObject.SetActive(false);
            }

            _deliveryTimeText.SetText(GetTimeAgo(itemData.DateUtcDate));
        }

        private void OnReadButtonClick()
        {
            ReadMessage();
            _messageView.gameObject.SetActive(true);
            _messageView.Initialize(_messageData);
        }

        private void ReadMessage()
        {
            _onReadAction?.Invoke(_messageData?.Id);
            _unReadIndicator.SetActive(false);
        }

        private void OnDeleteButtonClick()
        {
            _onDeleteAction?.Invoke(_messageData?.Id);
        }

        private void OnCallToAction()
        {
            if (_messageContent.Action.HasUrl)
            {
                string url;

#if UNITY_ANDROID && UNITY_EDITOR
                url = _messageContent.Action.Url.Android.Text;

                if (!string.IsNullOrEmpty(url))
                {
                    ReadMessage();
                    Application.OpenURL(url);
                }

                return;
#endif

#if UNITY_IOS && UNITY_EDITOR
                url = _messageContent.Action.Url.Android.Text;

                if (!string.IsNullOrEmpty(url))
                { 
                    ReadMessage();
                    Application.OpenURL(url);
                }

                return
#endif
            }
            
            OnReadButtonClick();
        }

        private void CreateLinks()
        {
            ClearLinks();

            _linksParent.gameObject.SetActive(true);

            for (int i = 0, count = _messageContent.Action.Links.Count; i < count; i++)
            {
                InboxLink link = Instantiate(_linkPrefab, _linksParent);
                link.gameObject.SetActive(true);
                link.Initialize(_messageContent.Action.Links[i]);
                inboxLinks.Add(link);
            }
        }

        private void ClearLinks()
        {
            inboxLinks.ForEach((i) => { Destroy(i.gameObject); });
            inboxLinks.Clear();
        }

        private static string GetTimeAgo(DateTime? deliveryTime)
        {
            if (deliveryTime == null)
                return "Unknown time";

            TimeSpan timeDiff = DateTime.UtcNow - deliveryTime.Value;

            if (timeDiff.TotalSeconds < 60) return "Just now";
            else if (timeDiff.TotalMinutes < 60) return $"{(int)timeDiff.TotalMinutes} mins ago";
            else if (timeDiff.TotalHours < 24) return $"{(int)timeDiff.TotalHours} hours ago";
            else if (timeDiff.TotalDays < 7) return $"{(int)timeDiff.TotalDays} days ago";
            else return deliveryTime.Value.ToString("MMM dd, yyyy"); // e.g., Apr 18, 2025
        }
    }
}