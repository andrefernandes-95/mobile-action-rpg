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
            if (mainCam == null || characterManager == null)
            {
                return;
            }

            float h = joystick.Horizontal + Input.GetAxis("Horizontal");
            float v = joystick.Vertical + Input.GetAxis("Vertical");

            // Camera-relative movement
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * v + camRight * h;

            if (moveDir.magnitude > 0.1f)
            {
                characterManager.Move(moveDir.normalized);
            }
            else
            {
                characterManager.Stop();
            }
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
