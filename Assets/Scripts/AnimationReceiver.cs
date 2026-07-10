namespace AF
{
    using UnityEngine;

    /// <summary>
    /// Unity Animation Events
    /// </summary>
    public class AnimationReceiver : MonoBehaviour
    {
        [SerializeField] CharacterManager characterManager;

        public void OpenHitbox() => characterManager.hitbox.OpenHitbox();
        public void CloseHitbox() => characterManager.hitbox.CloseHitbox();

    }
}
