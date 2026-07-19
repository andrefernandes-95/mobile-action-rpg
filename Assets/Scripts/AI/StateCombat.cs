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

        public override void Enter(StateMachine controller)
        {
        }

        public override void Tick(StateMachine controller)
        {
            var player = controller.character.GetPlayer();
            if (player == null)
            {
                return;
            }

            if (player.health.IsDead)
            {
                controller.SwitchState(
                    controller.patrolState != null
                        ? controller.patrolState
                        : controller.idleState
                );
                return;
            }

            float sqrDist = (controller.transform.position - player.transform.position).sqrMagnitude;
            float sight = controller.character.perception.sightRange;
            float disengageSqr = (sight * disengageMultiplier) * (sight * disengageMultiplier);

            float stop = 1.5f;
            if (controller.character.Motor != null)
            {
                stop = controller.character.Motor.StoppingDistance;
            }

            float stopSqr = stop * stop;

            if (sqrDist > disengageSqr || sqrDist > stopSqr)
            {
                controller.SwitchState(controller.chaseState);
                return;
            }

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

            if (!controller.character.isBusy &&
                Time.time >= controller.CombatRuntime.LastAttackTime + attackCooldown)
            {
                controller.CombatRuntime.LastAttackTime = Time.time;

                if (Random.Range(0, 1f) >= 0.25f)
                {
                    controller.character.combatManager.Attack();
                }
            }
        }

        public override void Exit(StateMachine controller)
        {
        }
    }
}
