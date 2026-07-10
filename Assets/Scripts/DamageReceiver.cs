namespace AF
{
    using UnityEngine;

    public class DamageReceiver : MonoBehaviour
    {
        [Header("Damage")]
        [Range(0f, 1f)] public float slashReduction = 1f;
        [Range(0f, 1f)] public float bluntReduction = 1f;
        [Range(0f, 1f)] public float pierceReduction = 1f;
        [Range(0f, 1f)] public float fireReduction = 1f;
        [Range(0f, 1f)] public float frostReduction = 1f;

        public void ApplyDamage(CharacterManager attacker)
        {
            if (IsInvulnerable()) return;

            float damage = 0f;

            MeleeAttack attackAbility = attacker.combatManager.GetCurrentAbility() as MeleeAttack;

            if (attackAbility != null)
            {

                if (attackAbility.slashDamage > 0)
                {
                    damage += Mathf.Clamp(attackAbility.slashDamage - slashReduction, 0, Mathf.Infinity);
                }

                if (attackAbility.bluntDamage > 0)
                {
                    damage += Mathf.Clamp(attackAbility.bluntDamage - bluntReduction, 0, Mathf.Infinity);
                }

                if (attackAbility.pierceDamage > 0)
                {
                    damage += Mathf.Clamp(attackAbility.pierceDamage - pierceReduction, 0, Mathf.Infinity);
                }

                if (attackAbility.fireDamage > 0)
                {
                    damage += Mathf.Clamp(attackAbility.fireDamage - fireReduction, 0, Mathf.Infinity);
                }

                if (attackAbility.frostDamage > 0)
                {
                    damage += Mathf.Clamp(attackAbility.frostDamage - frostReduction, 0, Mathf.Infinity);
                }
            }

            TakeDamage((int)damage);
        }

        public virtual void TakeDamage(int damage)
        {
            Debug.Log("Taking damage: " + damage);
        }

        public virtual bool IsInvulnerable()
        {
            return false;
        }
    }
}
