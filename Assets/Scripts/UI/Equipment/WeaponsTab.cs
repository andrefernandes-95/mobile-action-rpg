using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public sealed class WeaponsTab : ITabPage
    {
        public string TabButtonName => "WeaponsTabButton";

        readonly VisualTreeAsset row;
        readonly InventoryManager inventory;
        readonly EquipmentManager equipment;
        readonly BlacksmithService blacksmith;
        readonly ShopService shop;

        VisualElement host;

        public WeaponsTab(
            VisualTreeAsset rowTemplate,
            InventoryManager inventoryManager,
            EquipmentManager equipmentManager,
            BlacksmithService blacksmithService,
            ShopService shopService
        )
        {
            this.row = rowTemplate;
            this.inventory = inventoryManager;
            this.equipment = equipmentManager;
            this.blacksmith = blacksmithService;
            this.shop = shopService;
        }

        public void Enter(VisualElement contentHost)
        {
            host = contentHost;
            host.Clear();

            int equippedDamage = equipment != null && equipment.weaponInstance != null
                ? equipment.weaponInstance.weaponData.amount
                : 0;

            var header = new Label($"Current Attack Damage: {equippedDamage}");
            header.AddToClassList("label");
            host.Add(header);

            var scroll = new ScrollView { style = { marginTop = 16 } };
            host.Add(scroll);

            if (inventory == null)
            {
                return;
            }

            foreach (WeaponInstance weaponInstance in inventory.ownedWeapons)
            {
                scroll.Add(ItemRowView.Build(row, BuildModel(weaponInstance, equippedDamage)));
            }
        }

        public void Leave()
        {
        }

        ItemRowModel BuildModel(WeaponInstance weaponInstance, int equippedDamage)
        {
            Weapon data = weaponInstance.weaponData;
            bool equipped = equipment != null && equipment.weaponInstance == weaponInstance;

            var model = new ItemRowModel
            {
                Name = data.displayName,
                Icon = data.icon,
                Description = $"Attack Damage: {data.amount}",
                Status = $"Current Durability: {weaponInstance.durability}/{weaponInstance.maxDurability}",
                ShowIndicator = true,
                IndicatorColor = data.amount >= equippedDamage ? Color.green : Color.red
            };

            if (equipped)
            {
                model.Actions.Add(new ItemAction("Equipped", null, enabled: false));
            }
            else
            {
                model.Actions.Add(new ItemAction("Equip", () =>
                {
                    equipment.EquipWeapon(data);
                    Enter(host); // Refresh
                }));
            }

            int repairCost = weaponInstance.RepairCost;
            model.Actions.Add(
                new ItemAction(
                    repairCost > 0 ? $"Repair ({repairCost} Gold)" : "Repair",
                    () =>
                    {
                        if (blacksmith.Repair(weaponInstance))
                        {
                            Enter(host); // Refresh
                        }
                    },
                    enabled: repairCost > 0
                )
            );

            int sellValue = data.price;
            model.Actions.Add(
                new ItemAction(
                    $"Sell ({sellValue} Gold)",
                    () =>
                    {
                        // Check Sell — acrescenta ao ShopService (secção 4) in file:///C:/Users/andre/Desktop/Dungeoncrawler%200.1/Docs/Soulslike-Economy-Menu-Dialogue.html#s2
                        //if (shop != null && shop.SellW)
                    }
                )
            );

            return model;
        }
    }
}