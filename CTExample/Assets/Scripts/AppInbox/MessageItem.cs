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
        [SerializeField] private Button _urlActionButton = null;
        [SerializeField] private Transform _linksParent = null;
        [SerializeField] private InboxLink _linkPrefab = null;
        [SerializeField] private TMP_Text _deliveryTimeText = null;

        private List<InboxLink> inboxLinks = new List<InboxLink>();
        private MessageView _messageView = null;
        private CleverTapInboxMessage _messageData = null;
        private List<Content> _contentList = null;
        private Action<string> _onReadAction = null;
        private Action<string> _onDeleteAction = null;

        public string MessageId => _messageData?.Id;

        private void OnEnable()
        {
            _readButton.onClick.AddListener(OnReadButtonClick);
            _deleteButton.onClick.AddListener(OnDeleteButtonClick);
            _urlActionButton.onClick.AddListener(OnURLButtonClick);
        }

        private void OnDisable()
        {
            _readButton.onClick.RemoveListener(OnReadButtonClick);
            _deleteButton.onClick.RemoveListener(OnDeleteButtonClick);
            _urlActionButton.onClick.RemoveListener(OnURLButtonClick);
        }

        public void Initialize(MessageView messageView, CleverTapInboxMessage itemData, Action<string> OnReadAction, Action<string> OnDeleteAction)
        {
            _messageView = messageView;
            _messageData = itemData;
            _onReadAction = OnReadAction;
            _onDeleteAction = OnDeleteAction;

            _contentList = itemData.Message.Content;
            Content content = itemData.Message.Content[0];

            _unReadIndicator.SetActive(!itemData.IsRead);
            _titleText.text = content.Title.Text;
            _messageText.text = content.Message.Text;
            _urlActionButton.gameObject.SetActive(_contentList[0].Action.HasUrl);

            if (_contentList[0].Action.HasLinks)
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
            _onReadAction?.Invoke(_messageData?.Id);
            _unReadIndicator.SetActive(false);
            _messageView.gameObject.SetActive(true);
            _messageView.Initialize(_messageData);
        }

        private void OnDeleteButtonClick()
        {
            _onDeleteAction?.Invoke(_messageData?.Id);
        }

        private void OnURLButtonClick()
        {
            Application.OpenURL(_contentList[0].Action.Url.Android.Text);
        }

        private void CreateLinks()
        {
            ClearLinks();

            _linksParent.gameObject.SetActive(true);

            for (int i = 0; i < _contentList[0].Action.Links.Count; i++)
            {
                InboxLink link = Instantiate(_linkPrefab, _linksParent);
                link.gameObject.SetActive(true);
                link.Initialize(_contentList[0].Action.Links[i]);
                inboxLinks.Add(link);
            }
        }

        private void ClearLinks()
        {
            inboxLinks.ForEach((i) => { Destroy(i.gameObject); });
            inboxLinks.Clear();
        }

        public string GetTimeAgo(DateTime? deliveryTime)
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