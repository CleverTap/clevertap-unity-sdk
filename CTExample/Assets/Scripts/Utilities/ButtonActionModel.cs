using UnityEngine.UI;

namespace CTIntegrationTests
{
    internal delegate void ButtonAction(Button button);

    internal class ButtonActionModel
    {
        internal string Name { get; set; }
        internal string Tag { get; set; }
        internal ButtonAction Action { get; set; }

        internal ButtonActionModel(string name, ButtonAction action) : this(name, null, action)
        {
        }

        internal ButtonActionModel(string name, string tag, ButtonAction action)
        {
            Name = name;
            Tag = tag;
            Action = action;
        }
    }
}