using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public class HealthDocument : MonoBehaviour
    {
        [SerializeField] UIDocument uIDocument;
        VisualElement root;

        [Header("Components")]
        [SerializeField] Health health;

        void OnEnable()
        {
            if (health == null)
            {
                return;
            }

            health.OnChanged += UpdateHealth;
        }

        void OnDisable()
        {
            if (health == null)
            {
                return;
            }

            health.OnChanged -= UpdateHealth;
        }

        void Awake()
        {
            root = uIDocument.rootVisualElement;
        }

        void UpdateHealth()
        {
            if (health == null || root == null)
            {
                return;
            }

            VisualElement fill = root.Q<VisualElement>("Health").Q<VisualElement>("Fill");

            int currentValue = health.Current;
            int maxValue = health.Max;

            float percentage = (float)currentValue / maxValue * 100f;
            fill.style.width = Length.Percent(percentage);
        }
    }
}
