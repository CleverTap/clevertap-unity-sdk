using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CTIntegrationTests
{
    [RequireComponent(typeof(TMP_Text))]
    public class ClickableText : MonoBehaviour, IPointerClickHandler
    {
        public delegate void OnTextClicked(string text);
        public event OnTextClicked OnTextClickedEvent;

        private TMP_Text _tmpText;

        void Start()
        {
            _tmpText = GetComponent<TMP_Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnTextClickedEvent?.Invoke(_tmpText.text);
        }
    }
}