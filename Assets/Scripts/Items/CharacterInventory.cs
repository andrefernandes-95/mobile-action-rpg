using System.Collections.Generic;

namespace AF
{
    public class CharacterInventory : IInventoryData
    {
        WeaponInstance weapon;

        public CharacterInventory()
        {
            this.weapon = null;
        }

        public WeaponInstance GetWeapon()
        {
            return weapon;
        }

        public void SetWeapon(WeaponInstance weapon)
        {
            this.weapon = weapon;
        }
    }
}