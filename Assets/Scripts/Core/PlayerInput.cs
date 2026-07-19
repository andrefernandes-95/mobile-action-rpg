namespace AF
{
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerInput : MonoBehaviour
    {
        private Camera mainCam;
        public Joystick joystick;
        public CharacterManager startingCharacterManager;

        [Header("Current Character")]
        public CharacterManager characterManager;

        [HideInInspector] public UnityEvent<CharacterManager> onSetCurrentCharacter;

        [Header("Analog")]
        [SerializeField] float moveDeadzone = 0.12f;
        [SerializeField] float inputExponent = 1.35f;

        public float MoveAmount { get; private set; }
        public Vector3 MoveDirection { get; private set; }

        void OnEnable()
        {
            mainCam = Camera.main;

            SetCharacter(startingCharacterManager);
        }

        void SetCharacter(CharacterManager character)
        {
            if (characterManager != null)
            {
                characterManager.GiveControlToAI();
            }

            characterManager = character;
            characterManager.GiveControlToPlayer();
            onSetCurrentCharacter?.Invoke(characterManager);
        }

        void Update()
        {
            // --- Special Input ---
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpecialAbility();
                return; // prevent movement on same frame
            }

            HandleMovement();
        }

        void HandleMovement()
        {
            MoveAmount = 0f;
            MoveDirection = Vector3.zero;

            if (mainCam == null || characterManager == null)
            {
                return;
            }

            float h = 0f;
            float v = 0f;

            if (joystick != null)
            {
                h += joystick.Horizontal;
                v += joystick.Vertical;
            }

            h += Input.GetAxis("Horizontal");
            v += Input.GetAxis("Vertical");

            // Camera-relative movement
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 raw = camForward * v + camRight * h;
            float magnitude = Mathf.Clamp01(raw.magnitude);

            if (magnitude < moveDeadzone)
            {
                characterManager.Stop();

            }

            float remapped = Mathf.InverseLerp(moveDeadzone, 1f, magnitude);
            float amount = Mathf.Pow(remapped, inputExponent);

            Vector3 dir = raw / magnitude;
            MoveAmount = amount;
            MoveDirection = dir;

            characterManager.Move(dir, amount);
        }

        public void OnAttack()
        {
            if (characterManager != null)
            {
                characterManager.combatManager.Attack();
            }
        }

        public void OnSpecialAbility()
        {
            if (characterManager != null)
            {
                characterManager.combatManager.OnSpecialAbility();
            }
        }

        public Vector3 GetDodgeDirection(bool isLockedOn)
        {
            float h = joystick.Horizontal + Input.GetAxis("Horizontal");
            float v = joystick.Vertical + Input.GetAxis("Vertical");

            Vector3 camForward = mainCam.transform.forward * (isLockedOn ? -1f : 1f);
            Vector3 camRight = mainCam.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 dodgeDir = (camForward * v + camRight * h).normalized;

            if (dodgeDir.magnitude < 0.1f)
            {
                dodgeDir = characterManager.transform.forward;
            }

            return dodgeDir;
        }

    }
}
