using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AF
{
    public class InventoryManager : MonoBehaviour
    {
        IInventoryData Data;

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
            if (characterManager.IsPlayer())
            {
                Data = FindAnyObjectByType<PlayerInventory>(FindObjectsInactive.Include);
            }
            else
            {
                Data = new CharacterInventory();
            }

            SetupDefaultInventory();
        }

        public void SwitchWeapon(Weapon newWeapon)
        {
            WeaponInstance currentWeapon = Data.GetWeapon();

            equipmentManager.UnequipWeapon();
            Data.SetWeapon(null);

            if (currentWeapon != null && currentWeapon.weaponData != null)
            {
                currentWeapon.weaponData.SpawnInWorld(equipmentManager.transform.position);
            }

            string id = Guid.NewGuid().ToString();
            WeaponInstance weaponInstance = new(id, newWeapon);
            Data.SetWeapon(weaponInstance);
            equipmentManager.EquipWeapon(weaponInstance);
        }

        public WeaponInstance GetWeapon() => Data.GetWeapon();

        void SetupDefaultInventory()
        {
            if (randomDefaultWeapons.Length > 0)
            {
                Weapon selected = randomDefaultWeapons[UnityEngine.Random.Range(0, randomDefaultWeapons.Length)];
                SwitchWeapon(selected);
            }
            else if (defaultWeapon != null)
            {
                SwitchWeapon(defaultWeapon);
            }
        }


    }
}
