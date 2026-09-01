using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public class PlayerInventory : MonoBehaviour, IInventoryData
    {
        [SerializeField] WeaponInstance weapon;

        public static PlayerInventory Instance
        {
            get;
            private set;
        }

        public WeaponInstance GetWeapon()
        {
            return weapon;
        }

        public void SetWeapon(WeaponInstance weaponInstance)
        {
            this.weapon = weaponInstance;
        }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
