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
            characterManager.agent.ResetPath();
            characterManager.agent.enabled = false;
            characterManager.isBusy = true;

            if (characterManager.IsPlayer())
            {
                StartCoroutine(ReloadGame());
            }

            // Lock On
        }

        IEnumerator ReloadGame()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("SampleScene");
        }
    }
}
