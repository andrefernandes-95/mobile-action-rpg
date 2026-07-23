using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public sealed class HudMenuButtons : MonoBehaviour
    {
        [SerializeField] UIDocument document;

        void OnEnable()
        {
            var root = document.rootVisualElement;
            Bind(root, "Equipment", MenuId.Equipment);
            Bind(root, "Shop", MenuId.Shop);
            Bind(root, "SettingsButton", MenuId.Settings);
        }

        void Bind(VisualElement root, string buttonName, MenuId id)
        {
            var btn = root.Q<Button>(buttonName);
            if (btn != null)
            {
                btn.clicked += () => MenuManager.Instance.Toggle(id);
            }
        }
    }
}
