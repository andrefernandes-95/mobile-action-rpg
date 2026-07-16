namespace AF
{
    using UnityEngine;

    public class StateMachine : MonoBehaviour
    {
        public CharacterManager character;

        [Header("AI States")]
        public AIState idleState;
        public AIState patrolState;
        public AIState chaseState;
        public AIState combatState;

        [HideInInspector] public AIState currentState;

        public readonly CombatStateRuntime CombatRuntime = new();


        void Start()
        {
            SwitchState(patrolState != null ? patrolState : idleState);
        }

        void Update()
        {
            if (!character.IsPlayer())
            {
                currentState?.Tick(this);
            }
        }

        public void SwitchState(AIState newState)
        {
            if (newState == null)
            {
                return;
            }

            if (currentState == newState)
            {
                return;
            }

            currentState?.Exit(this);
            currentState = newState;
            currentState?.Enter(this);
        }
    }
}
