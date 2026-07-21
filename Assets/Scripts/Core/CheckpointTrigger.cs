using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public sealed class CheckpointTrigger : MonoBehaviour
    {

        [SerializeField] GameObject fireEffect;

        void Awake()
        {
            Deactivate(null);
        }

        void Activate(CharacterManager player)
        {
            player.health.SetMax(player.health.Max, true);

            PlayerProgress.Instance.SetCheckpoint(new()
            {
                worldPos = transform.position,
                scene = SceneManager.GetActiveScene().name,
                ShouldRespawn = false
            });

            CharacterManager[] enemiesInScene = FindObjectsByType<CharacterManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (CharacterManager enemy in enemiesInScene)
            {
                if (!enemy.IsPlayer())
                {
                    enemy.health.SetMax(enemy.health.Max, true);

                    if (enemy.TryGetComponent<DeathReactor>(out DeathReactor deathReactor))
                    {
                        deathReactor.UndoDeath();
                    }

                    enemy.RestoreDefaultPosition();
                }
            }

            fireEffect.SetActive(true);
        }

        void Deactivate(CharacterManager player)
        {
            fireEffect.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Activate(other.GetComponent<CharacterManager>());
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Deactivate(other.GetComponent<CharacterManager>());
        }
    }
}
