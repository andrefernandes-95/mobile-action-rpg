namespace AF
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AF/AI/States/Chase")]
    public class StateChase : AIState
    {
        public float disengageMultiplier = 1.3f;

        public override void Enter(StateMachine controller)
        {
        }

        public override void Tick(StateMachine controller)
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                return;
            }

            if (controller.character.isBusy)
            {
                return;
            }

            float dist = Vector3.Distance(
                controller.transform.position,
                player.transform.position
            );

            if (controller.character.health.IsDead || player.GetComponent<Health>().IsDeadAndNotReviving())
            {
                controller.SwitchState(
                    controller.patrolState != null
                        ? controller.patrolState
                        : controller.idleState
                );

                return;
            }

            float stop = 1.5f;
            if (controller.character.Motor != null)
            {
                stop = controller.character.Motor.StoppingDistance;
            }

            if (dist <= stop)
            {
                controller.SwitchState(controller.combatState);
                return;
            }

            controller.character.SetDestination(player.transform.position);
        }

        public override void Exit(StateMachine controller)
        {
            controller.character.Stop();
        }
    }
}
