using System.Collections.Generic;

namespace AF
{
    public interface IInventoryData
    {
        WeaponInstance GetWeapon();
        void SetWeapon(WeaponInstance weapon);
    }
}