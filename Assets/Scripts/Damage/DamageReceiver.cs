namespace AF
{
    using UnityEngine;

    public class DamageReceiver : MonoBehaviour, IDamageable
    {
        [SerializeField] ResistanceManager resistanceManager;

        public DamageResult Receive(in DamagePacket packet)
        {
            if (IsInvulnerable())
            {
                return DamageResult.Blocked(packet);
            }

            int final = packet.Amount;
            if (resistanceManager != null)
            {
                final = resistanceManager.ApplyReduction(packet.DamageType, packet.Amount);
            }

            DamageResult result = new(
                packet.Amount,
                final,
                packet.DamageType,
                packet.Source,
                false
            );

            TakeDamage(result);
            return result;
        }

        public virtual void TakeDamage(in DamageResult result)
        {
        }

        public virtual bool IsInvulnerable()
        {
            return false;
        }
    }
}
