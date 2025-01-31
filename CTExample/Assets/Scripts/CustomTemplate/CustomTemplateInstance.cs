using System.Collections;
using UnityEngine;

namespace CTExample
{
    public class CustomTemplateInstance : MonoBehaviour
    {
        public int WordWrapMaxCharacters { get; set; } = 30;

        protected CustomTemplateModel Model { get; set; }

        private static CustomTemplateReference prefabReference;

        protected static CustomTemplateReference PrefabReference
        {
            get
            {
                if (prefabReference == null)
                {
                    prefabReference = FindObjectOfType<CustomTemplateReference>();
                }

                return prefabReference;
            }
        }

        protected static string GetCustomTemplateInstanceGameObjectId(string id)
        {
            return $"CustomTemplateInstance:{id}";
        }

        protected static string GetCustomTemplateGameObjectId(string id)
        {
            return $"CustomTemplate:{id}";
        }

        /// <summary>
        /// Creates a new game object with <see cref="CustomTemplateInstance"/> component.
        /// Sets the <see cref="CustomTemplateInstance"/> component Model to the <paramref name="model"/>.
        /// The new gameObject triggers the <see cref="CustomTemplateInstance"/> <see cref="MonoBehaviour"/> methods.
        /// </summary>
        /// <param name="model">The model to use for the custom template instance.</param>
        internal static void Create(CustomTemplateModel model)
        {
            if (PrefabReference == null)
            {
                Logger.LogError("PrefabReference game object not found. Cannot create custom template instance.");
                return;
            }

            if (PrefabReference.customTemplatePrefab == null)
            {
                Logger.LogError("CustomTemplate prefab reference is null. Cannot create custom template instance.");
                return;
            }

            GameObject gameObject = new GameObject(GetCustomTemplateInstanceGameObjectId(model.Id));
            gameObject.AddComponent<CustomTemplateInstance>();
            gameObject.AddComponent<CanvasGroup>();

            var instance = gameObject.GetComponent<CustomTemplateInstance>();
            instance.Model = model;

            // Set the gameObject parent to be the PrefabReference parent.
            // The gameObject parent will be the same as the PrefabReference gameObject parent.
            gameObject.transform.SetParent(PrefabReference.transform.parent);
        }

        /// <summary>
        /// Instantiates a custom template prefab using the <see cref="CustomTemplateReference"/>.
        /// Sets the <see cref="CustomTemplate"/> UI fields using the <see cref="CustomTemplateModel"/>.
        /// </summary>
        void Start()
        {
            if (PrefabReference == null || PrefabReference.customTemplatePrefab == null)
            {
                Logger.LogError("Cannot instantiate Custom Template. CustomTemplate prefab reference is null.");
                return;
            }

            if (Model == null)
            {
                Logger.LogError("Cannot instantiate Custom Template. Model is null.");
                return;
            }

            GameObject prefab = PrefabReference.customTemplatePrefab;
            GameObject customTemplateGameObject = Instantiate(prefab, gameObject.transform);
            customTemplateGameObject.name = GetCustomTemplateGameObjectId(Model.Id);

            var customTemplate = customTemplateGameObject.GetComponentInChildren<CustomTemplate>();

            customTemplate.Title.text = Model.Title;

            string message = Utils.WrapText(Model.Message, WordWrapMaxCharacters);
            customTemplate.MessageText.text = message;

            if (!Model.IsFunction)
            {
                customTemplate.TriggerActionButton.onClick.AddListener(() =>
                {
                    string actionNameText = customTemplate.TextInput.text;
                    Model.OnTriggerAction?.Invoke(actionNameText);
                });
            }
            else
            {
                customTemplate.TriggerActionButton.gameObject.SetActive(false);
                customTemplate.TextInput.gameObject.SetActive(false);
            }

            customTemplate.AcceptButton.onClick.AddListener(() =>
            {
                Model.OnAccept?.Invoke();
            });

            customTemplate.CancelButton.onClick.AddListener(() =>
            {
                StartCoroutine(FadeOut());
            });
        }

        IEnumerator FadeOut()
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * 2;
                yield return null;
            }
            Destroy(gameObject);
            yield return null;
        }

        private void OnDestroy()
        {
            Model?.OnDismiss?.Invoke();
        }
    }
}