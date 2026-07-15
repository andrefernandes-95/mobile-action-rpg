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
        AudioSource audioSource => GetComponent<AudioSource>();

        public void PerformDodge(Vector3 direction, string animationName)
        {
            if (isDodging) 
            {
                return;
            }

            PlayDodgeSfx();

            characterManager.isBusy = true;
            isDodging = true;

            characterManager.agent.ResetPath();
            characterManager.agent.velocity = Vector3.zero;

            characterManager.animator.Play(animationName);

            StartCoroutine(DodgeRoutine(direction));
        }

        IEnumerator DodgeRoutine(Vector3 dir)
        {
            float elapsed = 0f;
            Vector3 startPos = transform.position;
            Vector3 targetPos = startPos + dir * dodgeDistance;
            if (dodgeTrail != null) dodgeTrail.emitting = true;

            while (elapsed < dodgeDuration)
            {
                characterManager.agent.Move((targetPos - startPos) * (Time.deltaTime / dodgeDuration));
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (dodgeTrail != null) dodgeTrail.emitting = false;

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
