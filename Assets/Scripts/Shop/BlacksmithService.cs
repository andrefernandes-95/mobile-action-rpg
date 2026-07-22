using UnityEngine;

namespace AF
{
    public sealed class BlacksmithService : MonoBehaviour
    {
        public bool Repair(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null || weaponInstance.durability >= weaponInstance.maxDurability)
            {
                return false;
            }

            if (PlayerProgress.Instance == null)
            {
                return false;
            }

            int cost = weaponInstance.RepairCost;
            if (cost <= 0)
            {
                return false;
            }

            if (!PlayerProgress.Instance.wallet.TrySpend(cost))
            {
                return false;
            }

            weaponInstance.Repair();

            return true;
        }

        public bool Extend(WeaponInstance weaponInstance)
        {

            if (weaponInstance == null || weaponInstance.weaponData == null)
            {
                return false;
            }

            if (PlayerProgress.Instance == null)
            {
                return false;
            }

            int cost = weaponInstance.ExtendCost;
            if (!PlayerProgress.Instance.wallet.TrySpend(cost))
            {
                return false;
            }

            weaponInstance.Extend(weaponInstance.weaponData.extendAmount);
            return true;
        }
    }
}