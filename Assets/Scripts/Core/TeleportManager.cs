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
            GameObject spawn = GameObject.Find("Spawn");

            if (spawn != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();

                cc.enabled = false;
                player.transform.position = spawn.transform.position;
                player.transform.rotation = spawn.transform.rotation;
                cc.enabled = true;
            }

            FindAnyObjectByType<ScreenFader>().FadeIn(null);
        }

        public void Teleport(string sceneName)
        {
            FindAnyObjectByType<ScreenFader>().FadeOut(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
    }
}
