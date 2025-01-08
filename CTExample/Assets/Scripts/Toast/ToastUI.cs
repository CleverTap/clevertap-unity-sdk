using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CTExample
{
    internal class ToastModel
    {
        internal ToastModel()
        {
        }

        internal ToastModel(string text, float duration, ToastPosition position) : base()
        {
            Text = text;
            Duration = duration;
            Position = position;
        }

        internal string Text { get; set; } = string.Empty;
        internal float Duration { get; set; } = 3F;
        internal ToastPosition Position { get; set; } = ToastPosition.BottomCenter;
    }

    internal class ToastUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform toastRectTransform;
        [SerializeField] private HorizontalLayoutGroup containerLayoutGroup;
        [SerializeField] private TMP_Text textUI;
        [Range(.1f, .8f)][SerializeField] private float fadeDuration = .4f;

        private readonly int maxTextLength = 200;

        private bool toastIsShowing = false;
        private readonly Queue<ToastModel> queue = new Queue<ToastModel>();

        void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        public void Show(ToastModel toastModel)
        {
            if (toastIsShowing)
            {
                queue.Enqueue(toastModel);
                return;
            }
            string text = toastModel.Text;
            textUI.text = (text.Length > maxTextLength) ? text[..(maxTextLength - 3)] + "..." : text;

            containerLayoutGroup.childAlignment = (TextAnchor)(int)toastModel.Position;

            toastIsShowing = true;
            StartCoroutine(FadeInOut(toastModel.Duration, fadeDuration));
        }

        public void Dismiss()
        {
            Stop();
            ShowNext();
        }

        public void DismissAll()
        {
            Stop();
            queue.Clear();
        }

        private void Stop()
        {
            StopAllCoroutines();
            canvasGroup.alpha = 0f;
            toastIsShowing = false;
        }

        private IEnumerator FadeInOut(float toastDuration, float fadeDuration)
        {
            yield return null;
            containerLayoutGroup.CalculateLayoutInputHorizontal();
            containerLayoutGroup.CalculateLayoutInputVertical();
            containerLayoutGroup.SetLayoutHorizontal();
            containerLayoutGroup.SetLayoutVertical();
            yield return null;

            RefreshContentHelper.RefreshContentFitters(toastRectTransform);
            yield return null;

            // Fade In
            yield return Fade(canvasGroup, 0f, 1f, fadeDuration);
            yield return new WaitForSeconds(toastDuration);
            // Fade Out
            yield return Fade(canvasGroup, 1f, 0f, fadeDuration);
            // Animation end
            toastIsShowing = false;
            ShowNext();
        }

        private IEnumerator Fade(CanvasGroup cGroup, float startAlpha, float endAlpha, float fadeDuration)
        {
            float startTime = Time.time;
            float alpha = startAlpha;

            if (fadeDuration > 0f)
            {
                while (alpha != endAlpha)
                {
                    alpha = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / fadeDuration);
                    cGroup.alpha = alpha;

                    yield return null;
                }
            }

            cGroup.alpha = endAlpha;
        }

        private void ShowNext()
        {
            if (queue.TryDequeue(out ToastModel toast))
            {
                Show(toast);
            }
        }

        private void OnDestroy()
        {
            Toast.isLoaded = false;
        }
    }
}