namespace AF
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AF/AI/States/Patrol")]
    public class StatePatrol : AIState
    {
        int patrolIndex;

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
            if (points == null || points.Length == 0)
            {
                return;
            }

            var motor = controller.character.Motor;
            if (motor == null || !motor.IsMotorEnabled)
            {
                return;
            }

            bool needsNewPoint = !motor.HasPath
                || motor.RemainingDistance < motor.StoppingDistance;

            if (needsNewPoint)
            {
                controller.character.SetDestination(points[patrolIndex].position);
                patrolIndex = (patrolIndex + 1) % points.Length;
            }
        }

        public override void Exit(StateMachine controller)
        {
            controller.character.Stop();
        }
    }
}
