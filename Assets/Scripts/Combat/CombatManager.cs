using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] Ability attackAbility;
        [SerializeField] Ability specialAbility;


        public Ability AttackAbility => attackAbility;
        public Ability SpecialAbility => specialAbility;

        [Header("Components")]
        [SerializeField] CharacterManager characterManager;

        [Header("Debug")]
        [SerializeField] Ability currentAbility;

        [HideInInspector] public UnityEvent<Hitbox> onHitboxOpen;
        [HideInInspector] public UnityEvent<Hitbox> onHitboxClose;
        [HideInInspector] public UnityEvent<Vector3> onEnemyHit;

        float abilityCooldownEndTime;

        public void ResetStates()
        {

        }

        public void Attack()
        {
            if (characterManager.isBusy)
            {
                return;
            }

            if (Time.time < abilityCooldownEndTime)
            {
                return;
            }

            if (attackAbility == null)
            {
                return;
            }

            characterManager.isBusy = true;
            characterManager.agent.ResetPath();
            SetCurrentAbility(attackAbility);
        }

        public void OnSpecialAbility()
        {
            if (characterManager.isBusy)
            {
                return;
            }

            if (specialAbility == null)
            {
                return;
            }

            characterManager.isBusy = true;
            characterManager.agent.ResetPath();

            SetCurrentAbility(specialAbility);
        }

        public void SetCurrentAbility(Ability next)
        {
            if (currentAbility != null)
            {
                currentAbility.OnEnd(characterManager);
            }

            currentAbility = next;
            currentAbility?.OnStart(characterManager);

            if (currentAbility != null)
            {
                abilityCooldownEndTime = Time.time + currentAbility.cooldown;
            }
        }

        public bool TryBuildDamagePacket(Vector3 hitPoint, out DamagePacket packet)
        {
            if (currentAbility == null)
            {
                packet = default;
                return false;
            }

            return currentAbility.TryBuildDamagePacket(characterManager, hitPoint, out packet);
        }

        public void EquipAttackAbility(Ability ability)
        {
            attackAbility = ability;
        }
    }
}
