using UnityEngine;

namespace AF
{
    public sealed class ShopService : MonoBehaviour
    {
        [SerializeField] ShopInventory catalog;
        [SerializeField] InventoryManager inventoryManager;

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

            inventoryManager.AddItem(weapon);

            return true;
        }

    }
}