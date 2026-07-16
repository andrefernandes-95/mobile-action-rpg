namespace AF
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AF/AI/States/Idle")]
    public class StateIdle : AIState
    {
        public override void Enter(StateMachine controller)
        {
            controller.character.Stop();
        }

        public override void Tick(StateMachine controller)
        {
            if (controller.character.perception.CanSeePlayer())
            {
                controller.SwitchState(controller.chaseState);
            }
        }

        public override void Exit(StateMachine controller) { }
    }
}
