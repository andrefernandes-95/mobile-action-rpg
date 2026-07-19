namespace AF
{
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerInput : MonoBehaviour
    {
        Camera mainCam;
        public Joystick joystick;
        public CharacterManager startingCharacterManager;

        [Header("Current Character")]
        public CharacterManager characterManager;

        [HideInInspector] public UnityEvent<CharacterManager> onSetCurrentCharacter;

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpecialAbility();
                return;
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

            if (characterManager.isBusy)
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

            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 raw = camForward * v + camRight * h;
            float magnitude = Mathf.Clamp01(raw.magnitude);

            // Só ignora ruído numérico. Stick mexe = personagem mexe.
            if (magnitude < 0.01f)
            {
                characterManager.Stop();
                return;
            }

            Vector3 dir = raw / magnitude;
            MoveAmount = magnitude;
            MoveDirection = dir;
            characterManager.Move(dir, magnitude);
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
            float h = 0f;
            float v = 0f;

            if (joystick != null)
            {
                h += joystick.Horizontal;
                v += joystick.Vertical;
            }

            h += Input.GetAxis("Horizontal");
            v += Input.GetAxis("Vertical");

            float forwardSign = 1f;
            if (isLockedOn)
            {
                forwardSign = -1f;
            }

            Vector3 camForward = mainCam.transform.forward * forwardSign;
            Vector3 camRight = mainCam.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 dodgeDir = camForward * v + camRight * h;
            if (dodgeDir.sqrMagnitude < 0.01f)
            {
                return characterManager.transform.forward;
            }

            return dodgeDir.normalized;
        }
    }
}
