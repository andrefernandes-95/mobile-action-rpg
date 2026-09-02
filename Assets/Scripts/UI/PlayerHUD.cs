namespace AF
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class PlayerHUD : MonoBehaviour
    {
        UIDocument uIDocument;
        VisualElement root;

        VisualElement PotionsContainer;
        Label ActiveBuffLabel;

        void Awake()
        {
            uIDocument = GetComponent<UIDocument>();
            root = uIDocument.rootVisualElement;
        }

        void OnEnable()
        {
            PotionsContainer = root.Q<VisualElement>("PotionsContainer");
            OnPotionChanged(0);

            ActiveBuffLabel = root.Q<Label>("ActiveBuffLabel");
            ActiveBuffLabel.text = "";

            PlayerInventory.Instance.OnPotionChanged += OnPotionChanged;
        }

        void OnDisable()
        {
            PlayerInventory.Instance.OnPotionChanged -= OnPotionChanged;
        }

        void OnPotionChanged(int potionCount)
        {
            int count = potionCount;
            foreach (VisualElement child in PotionsContainer.Children())
            {
                child.style.display = count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
                count--;
            }
        }
    }
}
