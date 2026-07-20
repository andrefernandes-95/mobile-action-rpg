using UnityEngine;

namespace AF
{
    /// <summary>
    /// Place in Player prefab, handles the respawn after the checkpoint scene is loaded
    /// </summary>
    public class RespawnReactor : MonoBehaviour
    {
        [SerializeField] CharacterController characterController;

        void Start()
        {
            if (PlayerProgress.Instance != null && PlayerProgress.Instance.LastCheckpoint != null && PlayerProgress.Instance.LastCheckpoint.ShouldRespawn)
            {
                PlayerProgress.Instance.LastCheckpoint.ShouldRespawn = false;

                characterController.enabled = false;
                transform.position = PlayerProgress.Instance.LastCheckpoint.worldPos;
                characterController.enabled = true;
            }
        }
    }
}