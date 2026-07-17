using UnityEngine;

namespace AF
{
    public sealed class EquipmentManager : MonoBehaviour
    {
        public WeaponInstance weaponInstance;
        Hitbox worldWeapon;

        [Header("References")]
        [SerializeField] Transform primaryHand;
        [SerializeField] Transform secondaryHand;

        [Header("Components")]
        [SerializeField] InventoryManager inventoryManager;

        public void EquipWeapon(Weapon weapon)
        {
            if (inventoryManager.TryGetWeaponInstance(weapon, out WeaponInstance weaponInstance))
            {
                UnequipWeapon();

                this.weaponInstance = weaponInstance;
                worldWeapon = Instantiate(weapon.prefab, primaryHand.transform).GetComponent<Hitbox>();
            }
        }

        public void UnequipWeapon()
        {
            if (worldWeapon != null)
            {
                Destroy(worldWeapon.gameObject);
            }

            this.weaponInstance = null;
        }

        public void OpenHitbox()
        {
            worldWeapon?.OpenHitbox();
        }

        public void CloseHitbox()
        {
            worldWeapon?.CloseHitbox();
        }
    }
}
