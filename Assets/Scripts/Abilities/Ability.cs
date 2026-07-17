namespace AF
{
    using UnityEngine;

    public class Ability
    {
        [Header("Cooldown")]
        public float cooldown;
        public float engageRadius = 2.5f;

        public virtual void OnStart(CharacterManager characterManager)
        {
        }

        public virtual void OnEnd(CharacterManager characterManager)
        {
        }

        public virtual bool TryBuildDamagePacket(
            CharacterManager source,
            Vector3 hitPoint,
            out DamagePacket packet
        )
        {
            packet = default;
            return false;
        }
    }
}
