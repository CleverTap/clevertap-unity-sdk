using System;
using System.Collections;
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
        private static RectTransform _layoutRoot = null;
        [SerializeField] private RawImage _messageImage = null;
        [SerializeField] private RawImage _messageIconImage = null;
        [SerializeField] private TMP_Text _titleText = null;
        [SerializeField] private TMP_Text _messageText = null;
        [SerializeField] private Button _readButton = null;
        [SerializeField] private Button _deleteButton = null;
        [SerializeField] private GameObject _unReadIndicator = null;
        [SerializeField] private Button _callToActionButton = null;
        [SerializeField] private Transform _linksParent = null;
        [SerializeField] private InboxLink _linkPrefab = null;
        [SerializeField] private TMP_Text _deliveryTimeText = null;

        [Header("Corousel")]
        [Space, SerializeField] private Button _nextButton = null;
        [SerializeField] private Button _prevButton = null;
        private List<Content> _messageContents = new List<Content>();

        private List<InboxLink> inboxLinks = new List<InboxLink>();
        private MessageView _messageView = null;
        private CleverTapInboxMessage _messageData = null;
        private Action<string> _onReadAction = null;
        private Action<string> _onDeleteAction = null;
        private Content _messageContent = null;
        private bool _isCarousel = false;
        private bool _isMessageIcon = false;

        private void OnEnable()
        {
            _layoutRoot = _layoutRoot ?? transform.parent.GetComponent<RectTransform>();

            _readButton.onClick.AddListener(OnReadButtonClick);
            _deleteButton.onClick.AddListener(OnDeleteButtonClick);
            _callToActionButton.onClick.AddListener(OnCallToAction);
            _nextButton.onClick.AddListener(Next);
            _prevButton.onClick.AddListener(Prev);
        }

        private void OnDisable()
        {
            _readButton.onClick.RemoveListener(OnReadButtonClick);
            _deleteButton.onClick.RemoveListener(OnDeleteButtonClick);
            _callToActionButton.onClick.RemoveListener(OnCallToAction);
            _nextButton.onClick.RemoveListener(Next);
            _prevButton.onClick.RemoveListener(Prev);
        }

        public void Initialize(MessageView messageView, CleverTapInboxMessage itemData, Action<string> OnReadAction, Action<string> OnDeleteAction)
        {
            _messageView = messageView;
            _messageData = itemData;
            _onReadAction = OnReadAction;
            _onDeleteAction = OnDeleteAction;
            _messageContents = itemData.Message.Content;
            _messageContent = itemData.Message.Content[0];

            _isCarousel = itemData.Message.MessageType == Inbox.MessageType.CAROUSEL;
            _isMessageIcon = itemData.Message.MessageType == Inbox.MessageType.MESSAGE_ICON;

            _nextButton.gameObject.SetActive(_isCarousel);
            _prevButton.gameObject.SetActive(_isCarousel);
            _messageIconImage.gameObject.SetActive(_isMessageIcon);

            _unReadIndicator.SetActive(!itemData.IsRead);

            UpdateMessageItem();
            _deliveryTimeText.SetText(GetTimeAgo(itemData.DateUtcDate));
        }


        private void Next()
        {
            _messageContent = _messageContents.Count > 1 ? _messageContents[(_messageContents.IndexOf(_messageContent) + 1) % _messageContents.Count] : _messageContent;
            UpdateMessageItem();
        }

        private void Prev()
        {
            _messageContent = _messageContents.Count > 1 ? _messageContents[(_messageContents.IndexOf(_messageContent) - 1 + _messageContents.Count) % _messageContents.Count] : _messageContent;
            UpdateMessageItem();
        }

        private void UpdateMessageItem()
        {
            _titleText.text = _messageContent.Title.Text;
            _messageText.text = _messageContent.Message.Text;

            if (_messageContent.Media != null && !string.IsNullOrEmpty(_messageContent.Media.Url))
            {
                MessageMediaDownloader.Instance.GetImage(_messageContent.Media.Url, SetMessageImage);
            }

            if (_messageContent.Action.HasLinks)
            {
                CreateLinks();
            }
            else
            {
                _linksParent.gameObject.SetActive(false);
            }
        }

        private void SetMessageImage(Texture2D texture)
        {
            if (texture != null)
            {
                if (_isMessageIcon)
                {
                    _messageIconImage.texture = texture;
                    _messageIconImage.gameObject.SetActive(true);
                    _messageImage.gameObject.SetActive(false);
                }
                else
                {
                    _messageImage.texture = texture;
                    _messageImage.gameObject.SetActive(true);
                    _messageIconImage.gameObject.SetActive(false);
                }
            }
            else
            {
                _messageImage.gameObject.SetActive(false);
                _messageIconImage.gameObject.SetActive(false);
            }

            UpdateLayout();
        }

        private void OnReadButtonClick()
        {
            _messageView.gameObject.SetActive(true);
            _messageView.Initialize(_messageContent);
            CleverTap.RecordInboxNotificationViewedEventForID(_messageData.Id);
            ReadMessage();
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
#elif UNITY_IOS && UNITY_EDITOR
                url = _messageContent.Action.Url.IOS.Text;
#else
                url = _messageContent.Action.Url.Android.Text;
#endif
                if (!string.IsNullOrEmpty(url))
                {
                    ReadMessage();
                    Application.OpenURL(url);
                }

                OnReadButtonClick();
            }
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

        private void UpdateLayout()
        {
            StartCoroutine(UpdateCanvasAndLayout());
        }

        private static IEnumerator UpdateCanvasAndLayout()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutRoot);
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