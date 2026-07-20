namespace AF
{
    using UnityEngine;

    public class CharacterManager : MonoBehaviour
    {
        [Header("Name")]
        public new string name = "";

        [Header("Components")]
        public Animator animator;

        [Header("Movement (DI)")]
        CharacterControllerMotor characterControllerMotor;
        NavMeshAgentMotor navMeshAgentMotor;

        IMovementMotor motor;

        public IMovementMotor Motor => motor;

        [Header("Character Components")]
        public Health health;
        public CharacterDamageReceiver characterDamageReceiver;
        public Dodge dodge;
        public Perception perception;
        public LockOn lockOn;
        public CombatManager combatManager;

        [Header("Locomotion")]
        public float rotationSpeed = 10f;
        public float animSpeedDamp = 0.12f;

        public bool isBusy = false;

        [Header("Patrol")]
        public Transform[] patrolPoints;

        float inputMoveAmount;
        Vector3 inputMoveDirection;

        public float MoveAmount
        {
            get
            {
                if (IsPlayer())
                {
                    return inputMoveAmount;
                }

                if (motor != null)
                {
                    return motor.MoveAmount;
                }

                return 0f;
            }
        }

        void Awake()
        {
            SetupMotor();
        }


        void Update()
        {
            UpdateRotation();
            UpdateAnimation();
        }

        void SetupMotor()
        {
            characterControllerMotor = GetComponent<CharacterControllerMotor>();
            navMeshAgentMotor = GetComponent<NavMeshAgentMotor>();

            // Default: AI (NavMesh). Player chama GiveControlToPlayer e troca.
            if (navMeshAgentMotor != null)
            {
                UseNavMeshMotor();
            }
            else if (characterControllerMotor != null)
            {
                UseCharacterControllerMotor();
            }
            else
            {
                Debug.LogError($"{name}: falta IMovementMotor (CC ou NavMesh).", this);
            }
        }

        public void OnIdle()
        {
            isBusy = false;
        }

        public void Move(Vector3 direction, float moveAmount = 1f)
        {
            if (isBusy || motor == null)
            {
                return;
            }

            inputMoveAmount = Mathf.Clamp01(moveAmount);
            inputMoveDirection = direction;
            motor.Move(direction, moveAmount);
        }

        public void SetDestination(Vector3 worldPosition)
        {
            if (isBusy || motor == null)
            {
                return;
            }

            motor.SetDestination(worldPosition);
        }

        public void Stop()
        {
            inputMoveAmount = 0f;
            inputMoveDirection = Vector3.zero;

            if (motor != null)
            {
                motor.Stop();
            }
        }

        public void ApplyDisplacement(Vector3 worldDelta)
        {
            if (motor != null)
            {
                motor.ApplyDisplacement(worldDelta);
            }
        }

        void UpdateRotation()
        {
            if (health != null && health.IsDead)
            {
                return;
            }

            if (motor == null)
            {
                return;
            }

            if (lockOn != null && lockOn.isLockedOn && lockOn.lockOnTarget != null)
            {
                Vector3 dir = lockOn.lockOnTarget.position - transform.position;
                dir.y = 0f;

                if (dir.sqrMagnitude < 0.0001f)
                {
                    return;
                }

                Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
                float t = Mathf.Clamp01(lockOn.lockOnRotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
                return;
            }

            Vector3 moveDir = motor.PlanarVelocity;
            // AI: limiar mais alto — velocity pequena perto do destino oscila e faz jitter
            float minSqr = IsPlayer() ? 0.01f : 0.08f;
            if (moveDir.sqrMagnitude < minSqr)
            {
                return;
            }

            Quaternion moveRot = Quaternion.LookRotation(moveDir.normalized);
            float rot = rotationSpeed * Mathf.Lerp(0.35f, 1f, MoveAmount);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRot, rot * Time.deltaTime);
        }

        void UpdateAnimation()
        {
            if (animator == null || motor == null)
            {
                return;
            }

            float vertical = 0f;
            float horizontal = 0f;
            bool lockedOn = lockOn != null && lockOn.isLockedOn && lockOn.lockOnTarget != null;

            if (IsPlayer() && lockedOn)
            {
                float amount = inputMoveAmount;

                if (amount > 0.01f && inputMoveDirection.sqrMagnitude > 0.0001f)
                {
                    Vector3 toTarget = lockOn.lockOnTarget.position - transform.position;
                    toTarget.y = 0f;

                    if (toTarget.sqrMagnitude > 0.0001f)
                    {
                        Quaternion faceRot = Quaternion.LookRotation(toTarget.normalized);
                        Vector3 local = Quaternion.Inverse(faceRot) * inputMoveDirection;
                        local.y = 0f;

                        float maxAxis = Mathf.Max(Mathf.Abs(local.x), Mathf.Abs(local.z));
                        if (maxAxis > 0.0001f)
                        {
                            vertical = (local.z / maxAxis) * amount;
                            horizontal = (local.x / maxAxis) * amount;
                        }
                    }
                }

                animator.SetFloat(AnimHashes.Vertical, vertical);
                animator.SetFloat(AnimHashes.Horizontal, horizontal);
            }
            else
            {
                if (IsPlayer())
                {
                    vertical = inputMoveAmount;
                }
                else
                {
                    float max = Mathf.Max(0.01f, motor.RunSpeed);
                    vertical = Mathf.Clamp01(motor.PlanarVelocity.magnitude / max);
                }

                horizontal = 0f;
                animator.SetFloat(AnimHashes.Vertical, vertical, animSpeedDamp, Time.deltaTime);
                animator.SetFloat(AnimHashes.Horizontal, horizontal, animSpeedDamp, Time.deltaTime);
            }
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
            UseNavMeshMotor();
        }

        public void GiveControlToPlayer()
        {
            GameContext.SetPlayer(this);
            UseCharacterControllerMotor();
        }

        void UseCharacterControllerMotor()
        {
            if (characterControllerMotor == null)
            {
                return;
            }

            if (navMeshAgentMotor != null)
            {
                navMeshAgentMotor.SetMotorEnabled(false);
                navMeshAgentMotor.enabled = false;
            }

            characterControllerMotor.enabled = true;
            characterControllerMotor.SetMotorEnabled(true);
            motor = characterControllerMotor;
        }

        void UseNavMeshMotor()
        {
            if (navMeshAgentMotor == null)
            {
                return;
            }

            if (characterControllerMotor != null)
            {
                characterControllerMotor.SetMotorEnabled(false);
                characterControllerMotor.enabled = false;
            }

            navMeshAgentMotor.enabled = true;
            navMeshAgentMotor.SetMotorEnabled(true);
            motor = navMeshAgentMotor;
        }
    }
}
