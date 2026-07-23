using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public class EquipmentScreen : MenuScreen
    {
        public override MenuId Id => MenuId.Equipment;

        [Header("Template")]
        [SerializeField] VisualTreeAsset weaponRowTemplate;

        [Header("Systems")]
        [SerializeField] InventoryManager inventory;
        [SerializeField] EquipmentManager equipment;
        [SerializeField] BlacksmithService blacksmith;
        [SerializeField] ShopService shop;

        TabView tabs;

        protected override void Bind()
        {
            var categories = Root.Q("EquipmentCategories");
            var host = Root.Q("TabContentHost");

            tabs = new TabView(categories, host);
        }

        protected override void OnOpen()
        {
            tabs.SelectFirst();
        }
    }
}