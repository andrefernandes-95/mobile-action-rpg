namespace AF
{
    using UnityEngine;

    /// <summary>
    /// Unity Animation Events
    /// </summary>
    public class AnimationReceiver : MonoBehaviour
    {
        [SerializeField] CharacterManager characterManager;

        public void Hit() => characterManager.hitbox.OpenHitbox();
        public void HitEnd() => characterManager.hitbox.CloseHitbox();

    }
}
