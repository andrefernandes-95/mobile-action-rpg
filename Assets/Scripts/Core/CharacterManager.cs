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

        public bool isBusy = false;

        [Header("Patrol")]
        public Transform[] patrolPoints;


        float stoppingDistance;


        void Awake()
        {
            stoppingDistance = agent.stoppingDistance;
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

        public void Move(Vector3 direction)
        {
            if (isBusy) return;

            Vector3 targetPos = transform.position + direction;
            agent.SetDestination(targetPos);
        }

        public void Stop()
        {
            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.ResetPath();
            }
        }

        void UpdateRotation()
        {
            if (health.IsDead)
            {
                return;
            }

            if (lockOn != null && lockOn.isLockedOn && lockOn.lockOnTarget != null)
            {
                // Direction to target
                Vector3 dir = lockOn.lockOnTarget.position - agent.nextPosition;
                dir.y = 0f;

                // Compute target rotation
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized);

                // Blend rotation smoothly
                float t = Mathf.Clamp01(lockOn.lockOnRotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);

                return;
            }

            // Movement-based rotation fallback
            Vector3 moveDir = agent.desiredVelocity;
            if (moveDir.sqrMagnitude < 0.01f) return;

            Quaternion moveRot = Quaternion.LookRotation(moveDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRot, rotationSpeed * Time.deltaTime);
        }

        void UpdateAnimation()
        {
            animator.SetFloat("Speed", agent.desiredVelocity.magnitude, 0.1f, Time.deltaTime);
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
