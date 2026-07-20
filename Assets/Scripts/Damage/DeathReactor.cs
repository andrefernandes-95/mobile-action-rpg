using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public sealed class DeathReactor : MonoBehaviour
    {
        [SerializeField] CharacterManager characterManager;
        [SerializeField] Health health;

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

            if (characterManager.IsPlayer())
            {
                StartCoroutine(ReloadGame());
            }
        }

        IEnumerator ReloadGame()
        {
            yield return new WaitForSeconds(2f);

            PlayerProgress.Instance.DropBloodstain(characterManager.transform.position);

            Checkpoint lastCheckpoint = PlayerProgress.Instance.LastCheckpoint;
            if (lastCheckpoint != null && !string.IsNullOrEmpty(lastCheckpoint.scene))
            {
                lastCheckpoint.ShouldRespawn = true;
                SceneManager.LoadScene(lastCheckpoint.scene);
            }
        }
    }
}
