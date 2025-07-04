using System.Collections.Generic;
using System.Linq;
using CleverTapSDK;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    public class NativeDisplay : MonoBehaviour
    {
        [SerializeField] private Button _getDisplayUnitForIdButton = null;
        [SerializeField] private Button _getAllDisplayUnitsButton = null;
        [SerializeField] private Button _showDisplayUnitButton = null;
        [SerializeField] private DisplayUnit _displayUnitView = null;

        private void OnEnable()
        {
            _getDisplayUnitForIdButton.gameObject.SetActive(true);
            _getAllDisplayUnitsButton.gameObject.SetActive(true);
            _showDisplayUnitButton.gameObject.SetActive(true);
        }
        private void Start()
        {
            _getDisplayUnitForIdButton.onClick.AddListener(GetDisplayUnit);
            _getAllDisplayUnitsButton.onClick.AddListener(GetAllDisplayUnits);
            _showDisplayUnitButton.onClick.AddListener(ShowDisplayUnit);
        }

        private void GetDisplayUnit()
        {
            CleverTapDisplayUnit displayUnit = CleverTap.GetAllDisplayUnitsParsed().FirstOrDefault();

            if (displayUnit != null)
            {
                Debug.Log($"Display Unit ID: {displayUnit.Id}, Title: {displayUnit.Content.FirstOrDefault().Title.Text}");
            }
            else
            {
                Debug.LogWarning("Display unit not found.");
            }
        }

        private void GetAllDisplayUnits()
        {
            List<CleverTapDisplayUnit> displayUnits = CleverTap.GetAllDisplayUnitsParsed();

            if (displayUnits != null && displayUnits.Count > 0)
            {
                Debug.Log($"Total Display Units: {displayUnits.Count}");
            }
            else if (displayUnits != null && displayUnits.Count == 0)
            {
                Debug.Log("No display units found.");
            }
        }

        private void ShowDisplayUnit()
        {
            CleverTapDisplayUnit displayUnit = CleverTap.GetAllDisplayUnitsParsed().FirstOrDefault();

            if (displayUnit == null)
            {
                Debug.LogWarning("DisplayUnit: ShowDisplayUnit() - No display unit found.");
                return;
            }

            _getDisplayUnitForIdButton.gameObject.SetActive(false);
            _getAllDisplayUnitsButton.gameObject.SetActive(false);
            _showDisplayUnitButton.gameObject.SetActive(false);
            _displayUnitView.gameObject.SetActive(true);
            _displayUnitView.Initialize(displayUnit);
        }

        private void OnDisable()
        {
            _displayUnitView.gameObject.SetActive(false);
        }

        public void Restore()
        {
            _getDisplayUnitForIdButton.gameObject.SetActive(true);
            _getAllDisplayUnitsButton.gameObject.SetActive(true);
            _showDisplayUnitButton.gameObject.SetActive(true);
        }

        public static class DisplayUnitType
        {
            public const string SIMPLE = "simple";
            public const string MESSAGE_ICON = "message-icon";
            public const string CAROUSEL = "carousel";
        }

    }
}