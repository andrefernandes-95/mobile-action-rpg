using UnityEngine;
using UnityEngine.AI;

namespace AF
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class NavMeshAgentMotor : MonoBehaviour, IMovementMotor
    {
        [SerializeField] float runSpeed = 5.5f;

        NavMeshAgent agent;
        Vector3 lastDestination;
        bool hasDestination;

        public float MoveAmount { get; private set; }
        public float RunSpeed => runSpeed;

        public Vector3 PlanarVelocity
        {
            get
            {
                if (agent == null || !agent.enabled)
                {
                    return Vector3.zero;
                }

                Vector3 v = agent.velocity;
                v.y = 0f;
                return v;
            }
        }

        public float StoppingDistance
        {
            get
            {
                if (agent != null)
                {
                    return agent.stoppingDistance;
                }

                return 1.5f;
            }
            set
            {
                if (agent != null)
                {
                    agent.stoppingDistance = value;
                }
            }
        }

        public float RemainingDistance
        {
            get
            {
                if (agent != null && agent.enabled)
                {
                    return agent.remainingDistance;
                }

                return 0f;
            }
        }

        public bool HasPath
        {
            get
            {
                return agent != null && agent.enabled && agent.hasPath;
            }
        }

        public bool IsMotorEnabled
        {
            get
            {
                return agent != null && agent.enabled;
            }
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;

            if (runSpeed <= 0f)
            {
                runSpeed = agent.speed;
            }
        }

        public void Move(Vector3 direction, float moveAmount)
        {
            if (!IsMotorEnabled || !agent.isOnNavMesh)
            {
                return;
            }

            MoveAmount = Mathf.Clamp01(moveAmount);
            agent.speed = runSpeed * MoveAmount;

            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                Stop();
                return;
            }

            SetDestination(transform.position + direction.normalized * 2f);
        }

        public void SetDestination(Vector3 worldPosition)
        {
            if (!IsMotorEnabled || !agent.isOnNavMesh)
            {
                return;
            }

            MoveAmount = 1f;
            agent.speed = runSpeed;

            if (hasDestination)
            {
                Vector3 delta = worldPosition - lastDestination;
                if (delta.sqrMagnitude < 0.15f && agent.hasPath)
                {
                    return;
                }
            }

            lastDestination = worldPosition;
            hasDestination = true;
            agent.SetDestination(worldPosition);
        }

        public void Stop()
        {
            MoveAmount = 0f;
            hasDestination = false;

            if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            {
                return;
            }

            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }

        public void ApplyDisplacement(Vector3 worldDelta)
        {
            if (!IsMotorEnabled)
            {
                return;
            }

            agent.Move(worldDelta);
        }

        public void SetMotorEnabled(bool enabled)
        {
            if (agent == null)
            {
                return;
            }

            if (!enabled)
            {
                Stop();
            }

            agent.enabled = enabled;
        }
    }
}
