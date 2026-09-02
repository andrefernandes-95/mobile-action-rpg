using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Weapon")]
    public class Weapon : Item
    {
        [Header("Weapon Details")]
        public WeaponType weaponType = WeaponType.Unarmed;
        public Ability attackAbility;

        [Header("Damage")]
        public DamageType damageType;
        public int amount;

        [Header("Audios")]
        public AudioClip swing;
        public AudioClip hit;

        [Header("Range")]
        public float engageRadius = 3f;

        [Header("Transform")]
        public Vector3 position;
        public Vector3 rotation;

        [Header("Animations")]
        public AnimatorOverrideController overrideController;

        public override string GetDescription()
        {
            return $"Attack: {amount}";
        }

        public override int Difference(Item item)
        {
            if (item == null || item is not Weapon weapon)
            {
                return amount;
            }

            return this.amount - weapon.amount;
        }

    }
}
