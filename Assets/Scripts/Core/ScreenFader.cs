using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] UIDocument uiDocument;
        [SerializeField] float fadeDuration = 0.5f;
        VisualElement blackScreen;

        void Awake()
        {
            blackScreen = uiDocument.rootVisualElement;
            blackScreen.style.opacity = 0f;
        }

        public Coroutine FadeIn(Action next) => StartCoroutine(Fade(1, 0, next));

        public Coroutine FadeOut(Action next) => StartCoroutine(Fade(0, 1, next));

        IEnumerator Fade(float from, float to, Action callback)
        {
            float t = 0;

            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                blackScreen.style.opacity = Mathf.Lerp(from, to, t / fadeDuration);
                yield return null;
            }

            blackScreen.style.opacity = to;

            callback?.Invoke();
        }
    }
}
