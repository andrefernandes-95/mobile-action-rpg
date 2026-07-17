namespace AF
{
    public class WeaponInstance : ItemInstance
    {
        public Weapon weaponData;
        public WeaponAbility ability;

        public WeaponInstance(string id, Weapon weaponData) : base(id, weaponData)
        {
            this.weaponData = weaponData;
            this.ability = new WeaponAbility(weaponData.damageType, weaponData.amount, weaponData.engageRadius);
        }
    }
}
