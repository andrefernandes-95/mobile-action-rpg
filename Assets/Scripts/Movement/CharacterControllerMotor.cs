using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterControllerMotor : MonoBehaviour, IMovementMotor
    {
        [SerializeField] float runSpeed = 5.5f;
        [SerializeField] float gravity = -20f;

        CharacterController controller;
        Vector3 velocity;
        bool motorEnabled = true;

        public float MoveAmount { get; private set; }
        public float RunSpeed => runSpeed;
        public Vector3 PlanarVelocity => new Vector3(velocity.x, 0f, velocity.z);

        public float StoppingDistance
        {
            get { return 0f; }
            set { }
        }

        public float RemainingDistance => 0f;
        public bool HasPath => false;

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

        public void Move(Vector3 direction, float moveAmount)
        {
            if (!IsMotorEnabled)
            {
                return;
            }

            MoveAmount = Mathf.Clamp01(moveAmount);
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f || MoveAmount <= 0f)
            {
                Stop();
                return;
            }

            direction.Normalize();

            float speed = runSpeed * MoveAmount;
            velocity.x = direction.x * speed;
            velocity.z = direction.z * speed;

            if (controller.isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            controller.Move(velocity * Time.deltaTime);
        }

        public void SetDestination(Vector3 worldPosition)
        {
        }

        public void Stop()
        {
            MoveAmount = 0f;
            velocity.x = 0f;
            velocity.z = 0f;

            if (!IsMotorEnabled)
            {
                return;
            }

            if (controller.isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            controller.Move(new Vector3(0f, velocity.y, 0f) * Time.deltaTime);
        }

        public void ApplyDisplacement(Vector3 worldDelta)
        {
            if (!IsMotorEnabled)
            {
                return;
            }

            controller.Move(worldDelta);
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
                velocity = Vector3.zero;
                MoveAmount = 0f;
            }
        }
    }
}
