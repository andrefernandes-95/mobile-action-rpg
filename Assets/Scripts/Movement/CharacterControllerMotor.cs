using UnityEngine;

namespace AF
{
    /// <summary>
    /// Movimento do Player: CharacterController + input analógico suave.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterControllerMotor : MonoBehaviour, IMovementMotor
    {
        [Header("Speeds")]
        [SerializeField] float runSpeed = 5.5f;
        [SerializeField] float stoppingDistance = 1.5f;

        [Header("Smoothing")]
        [SerializeField] float accelerateTime = 0.08f;
        [SerializeField] float decelerateTime = 0.12f;

        [Header("Gravity")]
        [SerializeField] float gravity = -20f;
        [SerializeField] float groundedGravity = -2f;

        CharacterController controller;
        Vector3 planarVelocity;
        Vector3 desiredPlanar;
        Vector3 velocitySmoothRef;
        float verticalVelocity;
        Vector3? destination;
        bool motorEnabled = true;

        public float MoveAmount { get; private set; }
        public float RunSpeed => runSpeed;
        public Vector3 PlanarVelocity => planarVelocity;

        public float StoppingDistance
        {
            get
            {
                return stoppingDistance;
            }
            set
            {
                stoppingDistance = Mathf.Max(0f, value);
            }
        }

        public float RemainingDistance
        {
            get
            {
                if (!destination.HasValue)
                {
                    return 0f;
                }

                Vector3 delta = destination.Value - transform.position;
                delta.y = 0f;
                return delta.magnitude;
            }
        }

        public bool HasPath
        {
            get
            {
                return destination.HasValue;
            }
        }

        public bool IsMotorEnabled
        {
            get
            {
                return motorEnabled && controller != null && controller.enabled;
            }
        }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (!IsMotorEnabled)
            {
                return;
            }

            if (destination.HasValue && desiredPlanar.sqrMagnitude < 0.0001f)
            {
                Vector3 to = destination.Value - transform.position;
                to.y = 0f;
                float dist = to.magnitude;

                if (dist <= StoppingDistance)
                {
                    destination = null;
                    MoveAmount = 0f;
                    desiredPlanar = Vector3.zero;
                }
                else
                {
                    desiredPlanar = (to / dist) * runSpeed;
                    MoveAmount = 1f;
                }
            }

            float smoothTime = decelerateTime;
            if (desiredPlanar.sqrMagnitude > planarVelocity.sqrMagnitude)
            {
                smoothTime = accelerateTime;
            }

            planarVelocity = Vector3.SmoothDamp(
                planarVelocity,
                desiredPlanar,
                ref velocitySmoothRef,
                smoothTime
            );

            if (planarVelocity.sqrMagnitude < 0.0001f && desiredPlanar.sqrMagnitude < 0.0001f)
            {
                planarVelocity = Vector3.zero;
                velocitySmoothRef = Vector3.zero;
            }

            TickController(planarVelocity);
        }

        public void Move(Vector3 direction, float moveAmount)
        {
            if (!IsMotorEnabled)
            {
                return;
            }

            destination = null;
            MoveAmount = Mathf.Clamp01(moveAmount);
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f || MoveAmount <= 0f)
            {
                desiredPlanar = Vector3.zero;
                MoveAmount = 0f;
                return;
            }

            // Stick 1:1 — pressão pequena = velocidade pequena (sem salto para walkSpeed)
            desiredPlanar = direction.normalized * (runSpeed * MoveAmount);
        }

        public void SetDestination(Vector3 worldPosition)
        {
            destination = worldPosition;
        }

        public void Stop()
        {
            MoveAmount = 0f;
            destination = null;
            desiredPlanar = Vector3.zero;
        }

        public void ApplyDisplacement(Vector3 worldDelta)
        {
            if (!IsMotorEnabled)
            {
                return;
            }

            Vector3 motion = worldDelta;
            if (controller.isGrounded && motion.y < 0f)
            {
                motion.y = 0f;
            }

            controller.Move(motion);
        }

        public void SetMotorEnabled(bool enabled)
        {
            motorEnabled = enabled;

            if (controller != null)
            {
                controller.enabled = enabled;
            }

            if (!enabled)
            {
                planarVelocity = Vector3.zero;
                desiredPlanar = Vector3.zero;
                velocitySmoothRef = Vector3.zero;
                MoveAmount = 0f;
                destination = null;
            }
        }

        void TickController(Vector3 horizontal)
        {
            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = groundedGravity;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 motion = horizontal;
            motion.y = verticalVelocity;
            controller.Move(motion * Time.deltaTime);
        }
    }
}
