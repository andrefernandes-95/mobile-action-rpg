using UnityEngine;
using UnityEngine.AI;

namespace AF
{
    /// <summary>
    /// Movimento da AI: NavMeshAgent.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class NavMeshAgentMotor : MonoBehaviour, IMovementMotor
    {
        [Header("Speeds")]
        [SerializeField] float walkSpeed = 1.8f;
        [SerializeField] float runSpeed = 5.5f;

        [Header("Destination")]
        [Tooltip("Só volta a pedir path se o alvo se mexeu mais que isto (evita jitter).")]
        [SerializeField] float destinationRepathThreshold = 0.35f;

        NavMeshAgent agent;
        float cachedStoppingDistance;
        Vector3 lastDestination;
        bool hasLastDestination;

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

                // velocity real — desiredVelocity oscila nos corners e causa jitter na rotação
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
                    agent.stoppingDistance = Mathf.Max(0f, value);
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
            cachedStoppingDistance = agent.stoppingDistance;

            if (runSpeed <= 0f)
            {
                if (agent.speed > 0f)
                {
                    runSpeed = agent.speed;
                }
                else
                {
                    runSpeed = 5.5f;
                }
            }

            if (walkSpeed <= 0f)
            {
                walkSpeed = runSpeed * 0.35f;
            }
        }

        public void Move(Vector3 direction, float moveAmount)
        {
            if (!IsMotorEnabled || !agent.isOnNavMesh)
            {
                return;
            }

            MoveAmount = Mathf.Clamp01(moveAmount);
            agent.speed = Mathf.Lerp(walkSpeed, runSpeed, MoveAmount);

            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                Stop();
                return;
            }

            float lookAhead = Mathf.Max(0.75f, agent.speed * 0.5f);
            SetDestination(transform.position + direction.normalized * lookAhead);
        }

        public void SetDestination(Vector3 worldPosition)
        {
            if (!IsMotorEnabled || !agent.isOnNavMesh)
            {
                return;
            }

            MoveAmount = 1f;
            agent.speed = runSpeed;

            // Evita SetDestination todos os frames (path rebuild = jitter)
            if (hasLastDestination)
            {
                float sqr = (worldPosition - lastDestination).sqrMagnitude;
                float threshold = destinationRepathThreshold * destinationRepathThreshold;

                if (sqr < threshold && agent.hasPath && !agent.pathPending)
                {
                    return;
                }
            }

            lastDestination = worldPosition;
            hasLastDestination = true;
            agent.SetDestination(worldPosition);
        }

        public void Stop()
        {
            MoveAmount = 0f;
            hasLastDestination = false;

            if (agent == null || !agent.enabled)
            {
                return;
            }

            if (!agent.isOnNavMesh)
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

        public void RestoreStoppingDistance()
        {
            StoppingDistance = cachedStoppingDistance;
        }
    }
}
