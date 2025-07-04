using System;
using System.Collections;
using System.Collections.Generic;
using CleverTapSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public sealed class DisplayUnit : MonoBehaviour
    {
        private static RectTransform _layoutRoot = null;
        [SerializeField] private RawImage _displayUnitImage = null;
        [SerializeField] private RawImage _displayUnitIconImage = null;
        [SerializeField] private TMP_Text _titleText = null;
        [SerializeField] private TMP_Text _messageText = null;
        [SerializeField] private Button _readButton = null;
        [SerializeField] private Button _callToActionButton = null;

        [Header("Corousel")]
        [Space, SerializeField] private Button _nextButton = null;
        [SerializeField] private Button _prevButton = null;

        private List<CleverTapDisplayUnit.DisplayUnitContent> _displayUnitContents;
        private CleverTapDisplayUnit _displayUnitData = null;
        private Action<string> _onReadAction = null;
        private Action<string> _onDeleteAction = null;
        private CleverTapDisplayUnit.DisplayUnitContent _displayUnitContent = null;
        private bool _isCarousel = false;
        private bool _isMessageIcon = false;

        private void OnEnable()
        {
            _layoutRoot = _layoutRoot ?? transform.parent.GetComponent<RectTransform>();

            _readButton.onClick.AddListener(OnReadButtonClick);
            _callToActionButton.onClick.AddListener(OnCallToAction);
            _nextButton.onClick.AddListener(Next);
            _prevButton.onClick.AddListener(Prev);
        }

        private void OnDisable()
        {
            _readButton.onClick.RemoveListener(OnReadButtonClick);
            _callToActionButton.onClick.RemoveListener(OnCallToAction);
            _nextButton.onClick.RemoveListener(Next);
            _prevButton.onClick.RemoveListener(Prev);
        }

        public void Initialize(CleverTapDisplayUnit itemData)
        {
            _displayUnitData = itemData;
            _displayUnitContents = itemData.Content;
            _displayUnitContent = itemData.Content[0];

            _isCarousel = itemData.MessageType == NativeDisplay.DisplayUnitType.CAROUSEL;
            _isMessageIcon = itemData.MessageType == NativeDisplay.DisplayUnitType.MESSAGE_ICON;

            _nextButton.gameObject.SetActive(_isCarousel);
            _prevButton.gameObject.SetActive(_isCarousel);
            _displayUnitIconImage.gameObject.SetActive(_isMessageIcon);

            UpdateMessageItem();

            CleverTap.RecordDisplayUnitViewedEventForID(_displayUnitData.Id);
        }


        private void Next()
        {
            _displayUnitContent = _displayUnitContents.Count > 1 ? _displayUnitContents[(_displayUnitContents.IndexOf(_displayUnitContent) + 1) % _displayUnitContents.Count] : _displayUnitContent;
            UpdateMessageItem();
        }

        private void Prev()
        {
            _displayUnitContent = _displayUnitContents.Count > 1 ? _displayUnitContents[(_displayUnitContents.IndexOf(_displayUnitContent) - 1 + _displayUnitContents.Count) % _displayUnitContents.Count] : _displayUnitContent;
            UpdateMessageItem();
        }

        private void UpdateMessageItem()
        {
            _titleText.gameObject.SetActive(!string.IsNullOrEmpty(_displayUnitContent.Title.Text));
            _messageText.gameObject.SetActive(!string.IsNullOrEmpty(_displayUnitContent.Message.Text));

            _titleText.text = _displayUnitContent.Title.Text;
            _messageText.text = _displayUnitContent.Message.Text;

            if (_displayUnitContent.Media != null && !string.IsNullOrEmpty(_displayUnitContent.Media.Url))
            {
                DisplayUnitMediaDownloader.Instance.GetImage(_displayUnitContent.Media.Url, SetMessageImage);
            }
        }

        private void SetMessageImage(Texture2D texture)
        {
            if (texture != null)
            {
                if (_isMessageIcon)
                {
                    _displayUnitIconImage.texture = texture;
                    _displayUnitIconImage.gameObject.SetActive(true);
                    _displayUnitImage.gameObject.SetActive(false);
                }
                else
                {
                    _displayUnitImage.texture = texture;
                    _displayUnitImage.gameObject.SetActive(true);
                    _displayUnitIconImage.gameObject.SetActive(false);
                }
            }
            else
            {
                _displayUnitImage.gameObject.SetActive(false);
                _displayUnitIconImage.gameObject.SetActive(false);
            }

            UpdateLayout();
        }

        private void OnReadButtonClick()
        {
            CleverTap.RecordInboxNotificationViewedEventForID(_displayUnitData.Id);
            ReadMessage();
        }

        private void ReadMessage()
        {
            CleverTap.RecordDisplayUnitClickedEventForID(_displayUnitData.Id);
            _onReadAction?.Invoke(_displayUnitData?.Id);
        }

        private void OnCallToAction()
        {
            if (_displayUnitContent.Action.HasUrl)
            {
                string url;

#if UNITY_ANDROID
                url = _displayUnitContent.Action.Url.Android.Text;
#elif UNITY_IOS
                url = _displayUnitContent.Action.Url.IOS.Text;
#else
                url = _displayUnitContent.Action.Url.Android.Text;
#endif
                if (!string.IsNullOrEmpty(url))
                {
                    Application.OpenURL(url);
                }

                OnReadButtonClick();
            }
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
    }
}