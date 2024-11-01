using UnityEngine;

namespace CTIntegrationTests
{
    public enum ToastPosition
    {
        TopCenter = 1,
        MiddleCenter = 4,
        BottomCenter = 7,
    }

    public static class Toast
    {
        public static bool isLoaded = false;

        private static ToastUI toastUI;

        private static void Prepare()
        {
            if (!isLoaded)
            {
                GameObject instance = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Toast"));
                instance.name = "Toast";
                toastUI = instance.GetComponent<ToastUI>();
                isLoaded = true;
            }
        }

        private static void Show(ToastModel toast)
        {
            Prepare();
            toastUI.Show(toast);
        }

        public static void Show(string text)
        {
            ToastModel toast = new ToastModel
            {
                Text = text
            };
            Show(toast);
        }

        public static void Show(string text, float duration)
        {
            ToastModel toast = new ToastModel
            {
                Text = text,
                Duration = duration
            };
            Show(toast);
        }

        public static void Show(string text, ToastPosition position)
        {
            ToastModel toast = new ToastModel
            {
                Text = text,
                Position = position
            };
            Show(toast);
        }

        public static void Show(string text, float duration, ToastPosition position)
        {
            ToastModel toast = new ToastModel
            {
                Text = text,
                Duration = duration,
                Position = position
            };
            Show(toast);
        }

        public static void Dismiss()
        {
            if (isLoaded)
                toastUI.Dismiss();
        }
    }
}