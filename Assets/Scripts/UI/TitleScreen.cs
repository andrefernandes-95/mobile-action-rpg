using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public sealed class TitleScreen : MonoBehaviour
    {
        [SerializeField] float fadeDuration = 1f;
        [SerializeField] float headlinerDuration = 2f;
        [SerializeField] float gameTitleDuration = 3f;

        UIDocument uiDocument;
        VisualElement root;

        [SerializeField] bool skip = false;

        void Awake()
        {
            uiDocument = GetComponent<UIDocument>();


        }

        void Start()
        {
            if (PlayerProgress.Instance.HasShownTitleScreen || skip)
            {
                gameObject.SetActive(false);
            }

            ShowTitleScreen();
        }

        void ShowTitleScreen()
        {
            PlayerProgress.Instance.HasShownTitleScreen = true;

            root = uiDocument.rootVisualElement;

            StartCoroutine(SequenceTitle());
        }

        IEnumerator SequenceTitle()
        {
            Label headliner = root.Q<Label>("Headliner");
            Label gameTitle = root.Q<Label>("GameTitle");
            Label author = root.Q<Label>("Author");

            // Começam invisíveis
            SetOpacity(headliner, 0f);
            SetOpacity(gameTitle, 0f);
            SetOpacity(author, 0f);

            // Game title aparece
            List<VisualElement> headliners = new()
            {
                headliner
            };

            // Headliner aparece
            yield return FadeIn(headliners.ToArray());

            yield return new WaitForSeconds(headlinerDuration);
            yield return StartCoroutine(FadeOut(headliners.ToArray()));

            // Game title aparece
            List<VisualElement> list = new()
            {
                gameTitle,
                author
            };

            yield return FadeIn(list.ToArray());

            yield return new WaitForSeconds(gameTitleDuration);

            // Tudo desaparece
            yield return StartCoroutine(FadeOut(list.ToArray()));

            gameObject.SetActive(false);
        }

        IEnumerator FadeIn(VisualElement[] elements)
        {
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;

                float alpha = Mathf.Clamp01(time / fadeDuration);

                foreach (VisualElement element in elements)
                {
                    SetOpacity(element, alpha);
                }

                yield return null;
            }

            foreach (VisualElement element in elements)
            {
                SetOpacity(element, 1f);
            }
        }

        IEnumerator FadeOut(VisualElement[] elements)
        {
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;

                float alpha = 1f - Mathf.Clamp01(time / fadeDuration);
                foreach (VisualElement element in elements)
                {
                    SetOpacity(element, alpha);
                }

                yield return null;
            }
            foreach (VisualElement element in elements)
            {
                SetOpacity(element, 0f);
            }
        }

        void SetOpacity(VisualElement element, float opacity)
        {
            element.style.opacity = opacity;
        }
    }
}
