using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Weapon")]
    public class Weapon : Item, IEquippable
    {
        [Header("Weapon Details")]
        public WeaponType weaponType = WeaponType.Unarmed;
        public Ability attackAbility;

        [Header("Prefab")]
        public GameObject prefab;

        [Header("Damage")]
        public DamageType damageType;
        public int amount;

        [Header("Durability")]
        public int maxDurability = 50;
        public int repairCostPerPoint = 2; // Repairing should always be lower than buying price
        public int extendCostPerPoint = 100; // Cost of extending durability
        public int extendAmount = 10; // Points gained by extending durability

        [Header("Range")]
        public float engageRadius = 3f;

        [Header("Transform")]
        public Vector3 position;
        public Vector3 rotation;

        [Header("Animations")]
        public AnimatorOverrideController overrideController;

        public void Equip(EquipmentManager equipmentManager)
        {
            equipmentManager.EquipWeapon(this);
        }

        public void Unequip(EquipmentManager equipmentManager)
        {
            equipmentManager.UnequipWeapon();
        }
    }
}
