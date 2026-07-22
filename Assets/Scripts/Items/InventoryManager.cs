using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AF
{
    public class InventoryManager : MonoBehaviour
    {
        public List<WeaponInstance> ownedWeapons = new();
        public List<ConsumableStack> ownedConsumables = new();

        [Header("Default Equipment")]
        [SerializeField] Weapon defaultWeapon;
        [SerializeField] Weapon[] randomDefaultWeapons;

        [Header("Default Consumables")]
        [SerializeField] ConsumableStack[] defaultConsumables;

        [Header("Components")]
        [SerializeField] EquipmentManager equipmentManager;
        [SerializeField] CharacterManager characterManager;

        void Awake()
        {
            SetupDefaultInventory();
        }

        public void AddItem<T>(T item, int count = 1) where T : Item
        {
            string id = Guid.NewGuid().ToString();

            if (item is Weapon weapon)
            {
                for (int i = 0; i < count; i++)
                {
                    WeaponInstance weaponInstance = new(id, weapon);
                    ownedWeapons.Add(weaponInstance);
                }
            }

            if (item is Consumable consumable)
            {
                ConsumableStack existingStack = ownedConsumables.FirstOrDefault(x => x.item == consumable);

                if (existingStack != null)
                {
                    existingStack.count++;
                }
                else
                {
                    ownedConsumables.Add(new()
                    {
                        item = consumable,
                        count = count
                    });
                }
            }
        }

        public bool TryGetWeaponInstance(Weapon weapon, out WeaponInstance weaponInstance)
        {
            weaponInstance = ownedWeapons.FirstOrDefault(ownedWeapon => ownedWeapon.itemData.Equals(weapon));

            return weaponInstance != null;
        }

        void SetupDefaultInventory()
        {
            if (randomDefaultWeapons.Length > 0)
            {
                Weapon selected = randomDefaultWeapons[UnityEngine.Random.Range(0, randomDefaultWeapons.Length)];
                AddItem(selected);
                selected.Equip(equipmentManager);
            }
            else if (defaultWeapon != null)
            {
                AddItem(defaultWeapon);
                defaultWeapon.Equip(equipmentManager);
            }

            if (defaultConsumables != null && defaultConsumables.Length > 0)
            {
                foreach (ConsumableStack consumableStack in defaultConsumables)
                {
                    AddItem(consumableStack.item, consumableStack.count);
                }
            }
        }

        public bool UseConsumable(Consumable item)
        {
            var stack = ownedConsumables.FirstOrDefault(ownedConsumable => ownedConsumable.item == item);
            if (stack == null || stack.count <= 0)
            {
                return false;
            }

            if (characterManager == null || characterManager.health == null || characterManager.health.IsDead)
            {
                return false;
            }

            foreach (Effect effect in item.effects)
            {
                effect.OnStart(characterManager);
            }

            stack.count--;

            if (stack.count <= 0)
            {
                ownedConsumables.Remove(stack);
            }

            return true;
        }

    }
}
