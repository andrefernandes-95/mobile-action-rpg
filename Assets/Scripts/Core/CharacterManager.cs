namespace AF
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;

    public class CharacterManager : MonoBehaviour
    {
        [Header("Name")]
        public new string name = "";

        [Header("Components")]
        public Animator animator;
        public NavMeshAgent agent;

        [Header("Character Components")]
        public Health health;
        public CharacterDamageReceiver characterDamageReceiver;
        public Dodge dodge;
        public Perception perception;
        public LockOn lockOn;
        public CombatManager combatManager;

        [Header("Locomotion")]
        public float rotationSpeed = 10f;
        public float walkSpeed = 1.8f;
        public float runSpeed = 5.5f;
        public float animSpeedDamp = 0.12f;

        public bool isBusy = false;

        [Header("Patrol")]
        public Transform[] patrolPoints;

        float stoppingDistance;
        float baseAgentSpeed;

        public float MoveAmount { get; private set; }

        void Awake()
        {
            stoppingDistance = agent.stoppingDistance;
            baseAgentSpeed = agent.speed;

            if (runSpeed <= 0f)
            {
                runSpeed = baseAgentSpeed > 0f ? baseAgentSpeed : 5.5f;
            }
            if (walkSpeed <= 0f)
            {
                walkSpeed = runSpeed * 0.35f;
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent.updateRotation = false;
        }

        public void OnIdle()
        {
            isBusy = false;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateRotation();
            UpdateAnimation();
        }

        public void Move(Vector3 direction, float moveAmount = 1f)
        {
            if (isBusy)
            {
                return;
            }

            if (!agent.enabled || !agent.isOnNavMesh)
            {
                return;
            }

            MoveAmount = Mathf.Clamp01(moveAmount);
            agent.speed = Mathf.Lerp(walkSpeed, runSpeed, MoveAmount);

            float lookAhead = Mathf.Max(0.35f, agent.speed * 0.35f);

            Vector3 targetPos = transform.position + direction.normalized * lookAhead;
            agent.SetDestination(targetPos);
        }

        public void Stop()
        {
            MoveAmount = 0f;

            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }
        }

        void UpdateRotation()
        {
            if (health.IsDead) return;

            if (lockOn != null && lockOn.isLockedOn && lockOn.lockOnTarget != null)
            {
                Vector3 dir = lockOn.lockOnTarget.position - agent.nextPosition;
                dir.y = 0f;
                if (dir.sqrMagnitude < 0.0001f) return;

                Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
                float t = Mathf.Clamp01(lockOn.lockOnRotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
                return;
            }

            Vector3 moveDir = agent.desiredVelocity;
            if (moveDir.sqrMagnitude < 0.01f) return;

            Quaternion moveRot = Quaternion.LookRotation(moveDir.normalized);

            // Rodar mais depressa quando o stick está a fundo
            float rot = rotationSpeed * Mathf.Lerp(0.55f, 1.25f, MoveAmount);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRot, rot * Time.deltaTime);
        }

        void UpdateAnimation()
        {
            float speedParam = MoveAmount;

            if (!IsPlayer())
            {
                float max = Mathf.Max(0.01f, runSpeed);
                speedParam = Mathf.Clamp01(agent.desiredVelocity.magnitude / max);
            }

            animator.SetFloat(AnimHashes.Speed, speedParam, animSpeedDamp, Time.deltaTime);
        }

        public bool IsPlayer()
        {
            return GameContext.Player == this;
        }

        public CharacterManager GetPlayer()
        {
            return GameContext.Player;
        }

        public void GiveControlToAI()
        {
            agent.stoppingDistance = stoppingDistance;
        }

        public void GiveControlToPlayer()
        {
            agent.stoppingDistance = 0;
            GameContext.SetPlayer(this);
        }
    }
}
