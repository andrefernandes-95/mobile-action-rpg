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
            if (currentState == newState) return;

            currentState?.Exit(this);
            currentState = Instantiate(newState);
            currentState?.Enter(this);
        }

    }
}
