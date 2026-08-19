using UnityEngine;

namespace AF
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] Transform player;

        void Start()
        {
            TeleportManager.Instance.TryToSpawnPlayer(player);
        }
    }
}