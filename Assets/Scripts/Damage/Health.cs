namespace AF
{
    using System;
    using UnityEngine;

    public class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] int maxHealth = 100;

        public int Current { get; private set; }
        public int Max => maxHealth;
        public bool IsDead => Current <= 0;

        public event Action<DamageResult> OnDamaged;
        public event Action OnDied;

        void Awake()
        {
            Current = maxHealth;
        }

        public void SetMax(int value, bool refill = true)
        {
            maxHealth = Mathf.Max(1, value);

            if (refill)
            {
                Current = maxHealth;
            }
            else
            {
                Current = Mathf.Min(Current, maxHealth);
            }
        }

        public void Apply(in DamageResult result)
        {
            if (IsDead || result.WasBlocked || result.FinalDamage <= 0)
            {
                if (result.WasBlocked)
                {
                    OnDamaged?.Invoke(result);
                }

                return;
            }

            Current = Mathf.Max(0, Current - result.FinalDamage);
            OnDamaged?.Invoke(result);

            if (Current == 0)
            {
                OnDied?.Invoke();
            }
        }

    }
}
