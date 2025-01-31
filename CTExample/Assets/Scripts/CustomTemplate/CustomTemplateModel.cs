using System;
using CleverTapSDK.Common;

namespace CTExample
{
    public class CustomTemplateModel
    {
        public readonly bool IsFunction;

        public readonly string Id;
        public readonly string Title;
        public readonly string Message;

        internal Action<string> OnTriggerAction { get; set; }
        internal Action OnAccept { get; set; }
        internal Action OnDismiss { get; set; }

        public CustomTemplateModel(CleverTapTemplateContext context, bool isFunction = false)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Cannot create template model with null context");

            IsFunction = isFunction;

            Id = context.TemplateName;
            Title = context.TemplateName;
            Message = context.ToString();

            OnAccept += () =>
            {
                context.SetPresented();
            };

            OnDismiss += () =>
            {
                context.SetDismissed();
            };

            OnTriggerAction += (actionName) =>
            {
                context.TriggerAction(actionName);
            };
        }

        internal CustomTemplateModel(string id, string title, string message, bool isFunction = false)
        {
            IsFunction = isFunction;

            Id = id;
            Title = title;
            Message = message;
        }

        public void Show()
        {
            CustomTemplateInstance.Create(this);
        }
    }
}