using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AF
{
    [DefaultExecutionOrder(-100)]
    public class PlayerInventory : MonoBehaviour, IInventoryData
    {
        WeaponInstance weapon;
        int potions = 0;

        const int MAX_POTIONS = 3;

        public UnityAction<int> OnPotionChanged;
        public UnityAction OnPotionUsed;

        public static PlayerInventory Instance
        {
            get;
            private set;
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

        public WeaponInstance GetWeapon()
        {
            return weapon;
        }

        public void SetWeapon(WeaponInstance weaponInstance)
        {
            this.weapon = weaponInstance;
        }

        public bool TryAddPotion()
        {
            if (potions == MAX_POTIONS)
            {
                return false;
            }

            potions++;
            OnPotionChanged?.Invoke(potions);
            return true;
        }

        public bool TryUsePotion()
        {
            if (potions <= 0)
            {
                return false;
            }

            potions = Mathf.Clamp(potions - 1, 0, MAX_POTIONS);
            OnPotionChanged?.Invoke(potions);
            OnPotionUsed?.Invoke();
            return true;
        }
    }
}
