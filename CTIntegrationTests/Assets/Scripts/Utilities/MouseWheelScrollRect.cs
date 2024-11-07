using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ScrollView that enables mousewheel and touchpad scrolling.
/// Enables scrolling over elements inside the scroll view.
///
/// Use this component instead of the default ScrollRect component on a ScrollView.
/// 
/// Script is based on https://discussions.unity.com/t/scroll-view-does-not-scroll-with-mousewheel-when-mouse-is-over-a-button-inside-the-scroll-view/781199/3
/// 
/// Note that to scroll over the ContentView, the ContentView needs to have a raycast target.
/// Ensure it has a graphic or image component and raycast target is enabled. It should also have a Canvas Renderer in that case.
/// </summary>
public class MouseWheelScrollRect : ScrollRect, IPointerEnterHandler, IPointerExitHandler
{
    private static readonly string mouseScrollWheelAxis = "Mouse ScrollWheel";
    private bool swallowMouseWheelScrolls = true;
    private bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    private void Update()
    {
        // Detect the mouse wheel and generate a scroll. This fixes the issue where Unity will prevent our ScrollRect
        // from receiving any mouse wheel messages if the mouse is over a raycast target (such as a button).
        if (isMouseOver && IsMouseWheelRolling())
        {
            var delta = Input.GetAxis(mouseScrollWheelAxis);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.scrollDelta = new Vector2(0f, delta);

            swallowMouseWheelScrolls = false;
            OnScroll(pointerData);
            swallowMouseWheelScrolls = true;
        }
    }

    public override void OnScroll(PointerEventData data)
    {
        if (IsMouseWheelRolling() && swallowMouseWheelScrolls)
        {
            // Eat the scroll so that we don't get a double scroll when the mouse is over an image
        }
        else
        {
            // Amplify the mousewheel so that it matches the scroll sensitivity.
            if (data.scrollDelta.y < -Mathf.Epsilon)
                data.scrollDelta = new Vector2(0f, -scrollSensitivity);
            else if (data.scrollDelta.y > Mathf.Epsilon)
                data.scrollDelta = new Vector2(0f, scrollSensitivity);

            base.OnScroll(data);
        }
    }

    private static bool IsMouseWheelRolling()
    {
        return Input.GetAxis(mouseScrollWheelAxis) != 0;
    }
}