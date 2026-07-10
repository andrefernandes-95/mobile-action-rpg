namespace AF
{
    using UnityEngine;

    public class CharacterDamageReceiver : DamageReceiver
    {
        [SerializeField] CharacterManager characterManager;

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            characterManager.health.TakeDamage((int)damage);
        }

        public override bool IsInvulnerable()
        {
            return characterManager.dodge.isDodging;
        }
    }
}
