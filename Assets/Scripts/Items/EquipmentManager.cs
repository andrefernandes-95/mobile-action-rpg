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
        [SerializeField] CharacterManager owner;
        [SerializeField] InventoryManager inventoryManager;
        [SerializeField] Animator animator;

        RuntimeAnimatorController defaultOverrideController;

        void Start()
        {
            if (owner.IsPlayer())
            {
                EquipWeapon(inventoryManager.GetWeapon());
            }

            defaultOverrideController = animator.runtimeAnimatorController;
        }

        public void EquipWeapon(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null)
            {
                return;
            }

            UnequipWeapon();

            this.weaponInstance = weaponInstance;

            Weapon weapon = weaponInstance.weaponData;
            worldWeapon = Instantiate(weapon.prefab, primaryHand.transform).GetComponent<Hitbox>();
            worldWeapon.transform.SetLocalPositionAndRotation(weapon.position, Quaternion.Euler(weapon.rotation));

            if (weapon.overrideController != null)
            {
                animator.runtimeAnimatorController = weapon.overrideController;
            }
        }

        public void UnequipWeapon()
        {
            if (worldWeapon != null)
            {
                Destroy(worldWeapon.gameObject);
            }

            this.weaponInstance = null;

            if (defaultOverrideController != null)
            {
                animator.runtimeAnimatorController = defaultOverrideController;
            }
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
