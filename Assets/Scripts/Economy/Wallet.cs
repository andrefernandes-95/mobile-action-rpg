using System;
using UnityEngine;

namespace AF
{
    [Serializable]
    public class Wallet
    {
        [SerializeField] int gold;
        public int Gold => gold;

        public event Action<int> OnGoldChanged;

        public void Add(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            gold += amount;
            OnGoldChanged?.Invoke(gold);
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0 || gold < amount)
            {
                return false;
            }

            gold -= amount;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        public int DrainAll()
        {
            int taken = gold;
            gold = 0;

            if (taken > 0)
            {
                OnGoldChanged?.Invoke(gold);
            }

            return taken;
        }

        public void Set(int amount)
        {
            gold = Mathf.Max(0, amount);
            OnGoldChanged?.Invoke(gold);
        }
    }
}
