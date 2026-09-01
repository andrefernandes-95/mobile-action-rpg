using UnityEngine;

namespace AF
{
    public sealed class ShopService : MonoBehaviour
    {
        [SerializeField] ShopInventory catalog;
        [SerializeField] InventoryManager inventoryManager;
        [SerializeField] EquipmentManager equipmentManager;

        public ShopInventory Catalog => catalog;

        public bool BuyWeapon(Weapon weapon)
        {
            if (weapon == null || inventoryManager == null)
            {
                return false;
            }

            if (PlayerProgress.Instance == null)
            {
                return false;
            }

            if (!PlayerProgress.Instance.wallet.TrySpend(weapon.price))
            {
                return false;
            }

            inventoryManager.SwitchWeapon(weapon);
            return true;
        }

        public int SellValue(Weapon weapon) => weapon != null
            ? Mathf.RoundToInt(weapon.price) : 0;

        public bool SellWeapon(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null || inventoryManager == null)
            {
                return false;
            }

            if (PlayerProgress.Instance == null)
            {
                return false;
            }

            if (equipmentManager != null && equipmentManager.weaponInstance == weaponInstance)
            {
                equipmentManager.UnequipWeapon();
            }

            if (inventoryManager.GetWeapon() == null)
            {
                return false;
            }

            PlayerProgress.Instance.wallet.Add(SellValue(weaponInstance.weaponData));
            return true;
        }
    }
}
