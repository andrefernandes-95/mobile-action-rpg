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
        [Header("Range")]
        public float engageRadius = 3f;

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
