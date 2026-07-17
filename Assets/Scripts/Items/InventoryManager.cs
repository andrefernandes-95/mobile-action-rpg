using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AF
{
    public class InventoryManager : MonoBehaviour
    {
        public List<WeaponInstance> ownedWeapons = new();

        [Header("Default Equipment")]
        [SerializeField] Weapon defaultWeapon;

        [Header("Components")]
        [SerializeField] EquipmentManager equipmentManager;

        void Awake()
        {
            if (defaultWeapon != null)
            {
                AddItem(defaultWeapon);
                defaultWeapon.Equip(equipmentManager);
            }
        }

        public void AddItem<T>(T item) where T : Item
        {
            string id = Guid.NewGuid().ToString();

            if (item is Weapon weapon)
            {
                WeaponInstance weaponInstance = new(id, weapon);
                ownedWeapons.Add(weaponInstance);
            }
        }

        public bool TryGetWeaponInstance(Weapon weapon, out WeaponInstance weaponInstance)
        {
            weaponInstance = ownedWeapons.FirstOrDefault(ownedWeapon => ownedWeapon.itemData.Equals(weapon));

            return weaponInstance != null;
        }
    }
}
