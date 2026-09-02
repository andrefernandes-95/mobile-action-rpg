using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public sealed class DeathReactor : MonoBehaviour
    {
        [SerializeField] CharacterManager characterManager;
        [SerializeField] Health health;
        [SerializeField] AudioClip deathSound;

        AudioSource deathAudioSource;

        void Awake()
        {
            deathAudioSource = this.gameObject.AddComponent<AudioSource>();
            deathAudioSource.playOnAwake = false;
            deathAudioSource.spatialBlend = 1;
        }

        void OnEnable()
        {
            if (health != null)
            {
                health.OnDied += HandleDeath;
            }
        }

        void OnDisable()
        {
            if (health != null)
            {
                health.OnDied -= HandleDeath;
            }
        }

        void HandleDeath()
        {
            if (characterManager == null)
            {
                return;
            }

            characterManager.animator.Play(AnimHashes.Death);
            characterManager.Stop();

            if (characterManager.Motor != null)
            {
                characterManager.Motor.SetMotorEnabled(false);
            }

            characterManager.isBusy = true;

            if (characterManager.TryGetComponent(out CapsuleCollider collider))
            {
                collider.enabled = false;
            }

            if (characterManager.IsPlayer())
            {
                HandlePlayerGameOver();
            }
        }

        void HandlePlayerGameOver()
        {
            if (PlayerInventory.Instance.TryUsePotion())
            {
                characterManager.health.IsReviving = true;
                UndoDeath();
                return;
            }

            StartCoroutine(ReloadGame());
        }

        IEnumerator ReloadGame()
        {
            yield return new WaitForSeconds(2f);

            Checkpoint lastCheckpoint = PlayerProgress.Instance.LastCheckpoint;
            if (lastCheckpoint != null && !string.IsNullOrEmpty(lastCheckpoint.scene))
            {
                lastCheckpoint.ShouldRespawn = true;
            }
        }

        public void UndoDeath()
        {
            if (characterManager.Motor != null)
            {
                characterManager.Motor.SetMotorEnabled(true);
            }


            if (characterManager.TryGetComponent(out CapsuleCollider collider))
            {
                collider.enabled = true;
            }

            characterManager.animator.Play(AnimHashes.Revive);
            characterManager.isBusy = true;
        }


        /// <summary>
        /// Animation Event
        /// </summary>
        public void OnDeath()
        {
            if (deathSound != null)
            {
                deathAudioSource.PlayOneShot(deathSound);
            }
        }
    }
}
