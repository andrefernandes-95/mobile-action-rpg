namespace AF
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AF/AI/States/Combat")]
    public class StateCombat : AIState
    {
        [Header("Attack")]
        public float attackCooldown = 1.2f;

        [Header("Disengage")]
        public float disengageMultiplier = 1.3f;

        private float lastAttackTime;

        public override void Enter(StateMachine controller)
        {
            lastAttackTime = -attackCooldown;
            controller.character.Stop();
        }

        public override void Tick(StateMachine controller)
        {
            var player = controller.character.GetPlayer();
            if (player == null) return;

            float dist = Vector3.Distance(
                controller.transform.position,
                player.transform.position
            );

            // Target dead → leave combat
            if (player.health.IsDead())
            {
                controller.SwitchState(
                    controller.patrolState != null
                        ? controller.patrolState
                        : controller.idleState
                );
                return;
            }

            // Too far → chase again
            if (dist > controller.character.perception.sightRange * disengageMultiplier)
            {
                controller.SwitchState(controller.chaseState);
                return;
            }

            // Out of attack range → chase
            if (dist > controller.character.agent.stoppingDistance)
            {
                controller.SwitchState(controller.chaseState);
                return;
            }

            // Face target
            Vector3 dir = player.transform.position - controller.transform.position;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                controller.transform.rotation = Quaternion.Slerp(
                    controller.transform.rotation,
                    rot,
                    Time.deltaTime * controller.character.rotationSpeed
                );
            }

            // Attack with cooldown
            if (!controller.character.isBusy &&
                Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                controller.character.combatManager.Attack();
            }
        }

        public override void Exit(StateMachine controller)
        {
            controller.character.Stop();
        }
    }
}
