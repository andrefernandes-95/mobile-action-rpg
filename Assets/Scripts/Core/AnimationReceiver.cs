namespace AF
{
    using UnityEngine;

    /// <summary>
    /// Unity Animation Events
    /// </summary>
    public class AnimationReceiver : MonoBehaviour
    {
        [SerializeField] CharacterManager characterManager;
        [SerializeField] EquipmentManager equipmentManager;

        public void Hit() => equipmentManager.OpenHitbox();
        public void HitEnd() => equipmentManager.CloseHitbox();

    }
}
