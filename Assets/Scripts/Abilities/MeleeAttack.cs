using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Ability/Melee Attack")]
    public class MeleeAttack : Ability
    {
        [Header("Animation")]
        public AnimationName[] animations;

        [Header("Damage")]
        public int slashDamage = 0;
        public int bluntDamage = 0;
        public int pierceDamage = 0;
        public int fireDamage = 0;
        public int frostDamage = 0;

        [Header("VFX")]
        [SerializeField] GameObject swingEffect;
        [SerializeField] GameObject hitImpact;

        public override void OnStart(CharacterManager characterManager)
        {
            characterManager.combatManager.onEnemyHit.AddListener(OnEnemyHit);
            characterManager.combatManager.onHitboxOpen.AddListener(OnHitboxOpen);
            characterManager.combatManager.onHitboxClose.AddListener(OnHitboxClose);

            characterManager.animator.Play(animations[Random.Range(0, animations.Length)].name);
        }

        public override void OnEnd(CharacterManager characterManager)
        {
            characterManager.combatManager.onEnemyHit.RemoveListener(OnEnemyHit);
            characterManager.combatManager.onHitboxOpen.RemoveListener(OnHitboxOpen);
            characterManager.combatManager.onHitboxClose.RemoveListener(OnHitboxClose);
        }

        void OnHitboxOpen(Hitbox hitbox)
        {
            if (swingEffect != null)
            {
                Instantiate(swingEffect, hitbox.swingSpawnRef.transform.position, Quaternion.identity);
            }
        }

        void OnHitboxClose(Hitbox hitbox)
        {
            // Optional: handle closing logic
        }

        void OnEnemyHit(Vector3 hitPosition)
        {
            if (hitImpact != null)
            {
                Instantiate(hitImpact, hitPosition, Quaternion.identity);
            }
        }
    }
}
