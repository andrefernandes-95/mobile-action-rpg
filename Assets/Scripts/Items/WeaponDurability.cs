using System;
using UnityEngine;

namespace AF
{
    public class WeaponDurability : MonoBehaviour
    {
        [SerializeField] CombatManager combatManager;
        [SerializeField] EquipmentManager equipmentManager;
        [SerializeField] InventoryManager inventoryManager;

        public event Action<WeaponInstance> OnWeaponBroke;

        void OnEnable()
        {
            if (combatManager != null)
            {
                combatManager.onEnemyHit.AddListener(HandleHit);
            }
        }

        void OnDisable()
        {
            if (combatManager != null)
            {
                combatManager.onEnemyHit.RemoveListener(HandleHit);
            }
        }

        void HandleHit(Vector3 _)
        {
            WeaponInstance weaponInstance = equipmentManager != null ? equipmentManager.weaponInstance : null;

            if (weaponInstance == null)
            {
                return;
            }

            weaponInstance.ConsumeDurability(1);

            if (!weaponInstance.IsBroken)
            {
                return;
            }

            BreakWeapon(weaponInstance);
        }

        void BreakWeapon(WeaponInstance weaponInstance)
        {
            equipmentManager.UnequipWeapon();
            inventoryManager.ownedWeapons.Remove(weaponInstance);
            OnWeaponBroke?.Invoke(weaponInstance);
        }
    }
}
