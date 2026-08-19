using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance
        {
            get;
            private set;
        }


        [Header("Next Spawnpoint")]
        [SerializeField] string nextSpawnpoint;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void TryToSpawnPlayer(Transform player)
        {
            if (nextSpawnpoint == null || string.IsNullOrEmpty(nextSpawnpoint))
            {
                return;
            }

            GameObject spawn = GameObject.Find(nextSpawnpoint);

            if (spawn != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();

                cc.enabled = false;
                player.transform.position = spawn.transform.position;
                player.transform.rotation = spawn.transform.rotation;
                cc.enabled = true;
            }

            this.nextSpawnpoint = null;
            FindAnyObjectByType<ScreenFader>().FadeIn(null);
        }

        public void Teleport(string sceneName, string nextSpawnpoint)
        {
            this.nextSpawnpoint = nextSpawnpoint;
            FindAnyObjectByType<ScreenFader>().FadeOut(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
    }
}
