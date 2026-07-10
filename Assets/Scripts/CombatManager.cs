using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] Ability attackAbility;
        public Ability AttackAbility => attackAbility;
        [SerializeField] Ability specialAbility;
        public Ability SpecialAbility => specialAbility;

        [Header("Components")]
        [SerializeField] CharacterManager characterManager;

        [Header("Debug")]
        [SerializeField] Ability currentAbility;

        [HideInInspector] public UnityEvent<Hitbox> onHitboxOpen;
        [HideInInspector] public UnityEvent<Hitbox> onHitboxClose;
        [HideInInspector] public UnityEvent<Vector3> onEnemyHit;

        public void ResetStates()
        {

        }

        public void Attack()
        {
            if (characterManager.isBusy) return;

            characterManager.isBusy = true;
            characterManager.agent.ResetPath();

            SetCurrentAbility(attackAbility);
        }

        public void OnSpecialAbility()
        {
            if (characterManager.isBusy) return;

            characterManager.isBusy = true;
            characterManager.agent.ResetPath();

            SetCurrentAbility(specialAbility);
        }

        public void SetCurrentAbility(Ability next)
        {
            if (currentAbility != null) currentAbility.OnEnd(characterManager);

            currentAbility = Instantiate(next);
            currentAbility?.OnStart(characterManager);
        }

        public Ability GetCurrentAbility() => currentAbility;
    }
}
