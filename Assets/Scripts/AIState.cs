namespace AF
{
    using UnityEngine;

    public abstract class AIState : ScriptableObject
    {
        public abstract void Enter(StateMachine controller);
        public abstract void Tick(StateMachine controller);
        public abstract void Exit(StateMachine controller);
    }
}
