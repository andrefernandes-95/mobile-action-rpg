using System;

namespace AF
{
    public class WeaponInstance : ItemInstance
    {
        public Weapon weaponData;
        public WeaponAbility ability;
        public int durability;
        public int maxDurability;
        public bool IsBroken => durability <= 0;
        public float DurabilityNormalized => maxDurability > 0 ? (float)(durability / maxDurability) : 0f;

        public WeaponInstance(string id, Weapon weaponData) : base(id, weaponData)
        {
            this.weaponData = weaponData;
            this.ability = new WeaponAbility(weaponData.damageType, weaponData.amount, weaponData.engageRadius);
            this.maxDurability = weaponData.maxDurability;
            this.durability = weaponData.maxDurability;
        }

        public void ConsumeDurability(int amount = 1)
        {
            if (amount <= 0)
            {
                return;
            }

            durability = Math.Max(0, durability - amount);
        }

        public void Repair()
        {
            durability = maxDurability;
        }

        public void Extend(int points)
        {
            if (points <= 0)
            {
                return;
            }

            maxDurability += points;
            durability += points;
        }

        public int RepairCost => (maxDurability - durability) * weaponData.repairCostPerPoint;
        public int ExtendCost => weaponData.extendAmount * weaponData.extendCostPerPoint;

    }
}
