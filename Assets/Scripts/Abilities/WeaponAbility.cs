using UnityEngine;

namespace AF
{
    public class WeaponAbility : Ability
    {
        DamageType damageType;
        int amount;

        public WeaponAbility(
            DamageType damageType,
            int amount,
            float engageRadius
        )
        {
            this.damageType = damageType;
            this.amount = amount;
            this.engageRadius = engageRadius;
        }

        public override void OnStart(CharacterManager characterManager)
        {
            characterManager.combatManager.onEnemyHit.AddListener(OnEnemyHit);
            characterManager.combatManager.onHitboxOpen.AddListener(OnHitboxOpen);
            characterManager.combatManager.onHitboxClose.AddListener(OnHitboxClose);

            characterManager.animator.Play(AnimHashes.Attack);
        }

        public override void OnEnd(CharacterManager characterManager)
        {
            characterManager.combatManager.onEnemyHit.RemoveListener(OnEnemyHit);
            characterManager.combatManager.onHitboxOpen.RemoveListener(OnHitboxOpen);
            characterManager.combatManager.onHitboxClose.RemoveListener(OnHitboxClose);
        }

        void OnHitboxOpen(Hitbox hitbox)
        {
        }

        void OnHitboxClose(Hitbox hitbox)
        {
            // Optional: handle closing logic
        }

        void OnEnemyHit(Vector3 hitPosition)
        {
            VFXPoolTag tag = VFXPoolTag.SLASH;

            if (damageType == DamageType.Blunt)
            {
                tag = VFXPoolTag.BLUNT;
            }
            else if (damageType == DamageType.Pierce)
            {
                tag = VFXPoolTag.PIERCE;
            }

            VFXPool.Instance.Play(tag, hitPosition, Quaternion.identity);
        }

        public override bool TryBuildDamagePacket(CharacterManager source, Vector3 hitPoint, out DamagePacket packet)
        {
            if (amount <= 0)
            {
                packet = default;
                return false;
            }

            packet = new DamagePacket(source, damageType, amount, hitPoint);
            return true;
        }
    }
}
