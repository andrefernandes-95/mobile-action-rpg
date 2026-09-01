using UnityEngine;

namespace AF
{
    public sealed class BlacksmithService : MonoBehaviour
    {
        public bool Repair(WeaponInstance weaponInstance)
        {
            if (weaponInstance == null)
            {
                return false;
            }

            if (PlayerProgress.Instance == null)
            {
                return false;
            }

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

            return true;
        }
    }
}