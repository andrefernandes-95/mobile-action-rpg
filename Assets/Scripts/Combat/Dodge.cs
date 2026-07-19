using System.Collections;
using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class Dodge : MonoBehaviour
    {
        public bool isDodging = false;

        [Header("Settings")]
        [SerializeField] float dodgeDistance = 4f;
        [SerializeField] float dodgeDuration = 0.35f;

        [Header("Components")]
        [SerializeField] CharacterManager characterManager;

        [Header("VFX")]
        [SerializeField] TrailRenderer dodgeTrail;

        [Header("Sound")]
        [SerializeField] AudioClip dodgeSfx;

        AudioSource audioSource;
        PlayerInput playerInput;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            playerInput = GetComponent<PlayerInput>();
        }

        public void PerformDodge(string animationName)
        {
            if (isDodging)
            {
                return;
            }

            PlayDodgeSfx();

            characterManager.isBusy = true;
            isDodging = true;

            characterManager.Stop();
            characterManager.animator.Play(animationName);

            Vector3 direction = transform.forward;
            bool isLockedOn = characterManager.lockOn != null && characterManager.lockOn.isLockedOn;

            if (playerInput != null)
            {
                direction = playerInput.GetDodgeDirection(isLockedOn);
            }
            else if (isLockedOn)
            {
                direction *= -1f;
            }

            StartCoroutine(DodgeRoutine(direction));
        }

        IEnumerator DodgeRoutine(Vector3 dir)
        {
            float elapsed = 0f;
            Vector3 startPos = transform.position;
            Vector3 targetPos = startPos + dir * dodgeDistance;

            if (dodgeTrail != null)
            {
                dodgeTrail.emitting = true;
            }

            while (elapsed < dodgeDuration)
            {
                characterManager.ApplyDisplacement((targetPos - startPos) * (Time.deltaTime / dodgeDuration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (dodgeTrail != null)
            {
                dodgeTrail.emitting = false;
            }

            isDodging = false;
            characterManager.isBusy = false;
        }

        void PlayDodgeSfx()
        {
            if (audioSource == null || dodgeSfx == null)
            {
                return;
            }

            audioSource.PlayOneShot(dodgeSfx);
        }
    }
}
