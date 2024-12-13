using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CTExample
{
    /// <summary>
    /// Enable scroll over InputFields inside ScrollView.
    /// 
    /// The default behaviour pops up the keyboard on mobile and selects/focuses the input field on web/desktop.
    /// The mouse wheel scroll stops when it reaches an input field.
    /// This script fixes the above behavior.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class ScrollInputFieldFixer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
    {
        private ScrollRect _scrollRect = null;
        private TMP_InputField _input = null;
        private bool _isDragging = false;
        private bool _preventScrollRectDrag;

        private void Start()
        {
            _scrollRect = GetComponentInParent<ScrollRect>();
            if (_scrollRect == null)
            {
                Debug.LogWarning("[ScrollInputFieldFixer]: ScrollRect not found.");
            }

            _input = GetComponent<TMP_InputField>();
            _input.DeactivateInputField();
            _input.onDeselect.AddListener(_ => _preventScrollRectDrag = false);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (_scrollRect != null && !_preventScrollRectDrag)
            {
                _isDragging = true;
                _input.DeactivateInputField();
                _scrollRect.OnBeginDrag(data);
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (_scrollRect != null)
            {
                _isDragging = false;
                _scrollRect.OnEndDrag(data);
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnDrag(data);
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (!_isDragging && !data.dragging)
            {
                _input.ActivateInputField();
                _preventScrollRectDrag = true;
            }
        }
    }
}