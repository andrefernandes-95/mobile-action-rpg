using UnityEngine;

namespace AF
{
    public class CharacterDamageReceiver : DamageReceiver
    {
        [SerializeField] CharacterManager characterManager;

        public override void TakeDamage(in DamageResult result)
        {
            characterManager.health.Apply(result);
        }

        public override bool IsInvulnerable()
        {
            return characterManager.dodge != null && characterManager.dodge.isDodging;
        }
    }
}
