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
            if (player != null)
            {
                player.lockOn.RegisterChasingEnemy(controller.character);
            }
        }

        public override void Tick(StateMachine controller)
        {
            var player = controller.character.GetPlayer();
            if (player == null) return;

            float dist = Vector3.Distance(
                controller.transform.position,
                player.transform.position
            );

            // Disengage
            if (
                dist > controller.character.perception.sightRange * disengageMultiplier // If far away
                || controller.character.health.IsDead() // Or Dead
                || player.health.IsDead() // Or Player Is Dead
            )
            {
                controller.SwitchState(
                    controller.patrolState != null
                        ? controller.patrolState
                        : controller.idleState
                );

                // If exiting state to patrol or idle, unregister lock on
                if (player != null)
                {
                    player.lockOn.UnregisterChasingEnemy(controller.character);
                }
                return;
            }

            // Enter combat
            if (dist <= controller.character.agent.stoppingDistance)
            {
                controller.SwitchState(controller.combatState);
                return;
            }

            // Chase
            controller.character.agent.SetDestination(player.transform.position);
        }
        public override void Exit(StateMachine controller)
        {
            controller.character.agent.ResetPath();
        }
    }
}
