using TMPro;
using UnityEngine;

namespace CTIntegrationTests
{
    public class KeyValue : MonoBehaviour
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool UseTextValues;
        public TMP_Text KeyText;
        public TMP_Text ValueText;

        // Start is called before the first frame update
        void Start()
        {
            if (!UseTextValues)
            {
                KeyText.SetText(Key);
                ValueText.SetText(Value);
            }

            RefreshContentHelper.RefreshContentFitters((RectTransform)transform.parent.transform);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}