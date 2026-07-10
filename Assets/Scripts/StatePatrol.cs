namespace AF
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AF/AI/States/Patrol")]
    public class StatePatrol : AIState
    {
        private int patrolIndex;

        public override void Enter(StateMachine controller)
        {
            patrolIndex = 0;
        }

        public override void Tick(StateMachine controller)
        {
            if (controller.character.perception.CanSeePlayer())
            {
                controller.SwitchState(controller.chaseState);
                return;
            }

            var points = controller.character.patrolPoints;
            if (points == null || points.Length == 0) return;

            if (controller.character.agent.enabled &&
                !controller.character.agent.hasPath ||
                controller.character.agent.remainingDistance < controller.character.agent.stoppingDistance)
            {
                controller.character.agent.SetDestination(points[patrolIndex].position);
                patrolIndex = (patrolIndex + 1) % points.Length;
            }
        }

        public override void Exit(StateMachine controller)
        {
            controller.character.agent.ResetPath();
        }
    }
}
