namespace AF
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AF/AI/States/Chase")]
    public class StateChase : AIState
    {
        public float disengageMultiplier = 1.3f;

        public override void Enter(StateMachine controller)
        {
            var player = controller.character.GetPlayer();

            if (player != null && !controller.character.health.IsDead)
            {
                player.lockOn.RegisterChasingEnemy(controller.character);
            }
        }

        public override void Tick(StateMachine controller)
        {
            var player = controller.character.GetPlayer();
            if (player == null)
            {
                return;
            }

            float dist = Vector3.Distance(
                controller.transform.position,
                player.transform.position
            );

            if (
                dist > controller.character.perception.sightRange * disengageMultiplier
                || controller.character.health.IsDead
                || player.health.IsDead
            )
            {
                controller.SwitchState(
                    controller.patrolState != null
                        ? controller.patrolState
                        : controller.idleState
                );

                player.lockOn.UnregisterChasingEnemy(controller.character);
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
