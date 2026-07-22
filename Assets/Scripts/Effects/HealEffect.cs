using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Effects/Heal Effect")]
    public class HealEffect : Effect
    {
        [Header("Healing Effect (Choose one)")]
        [SerializeField] int healAmount = -1;
        [SerializeField][Range(0f, 100f)] float healPercentage = 0f;

        public override void OnStart(CharacterManager characterManager)
        {
        }

        public override void OnApplied(CharacterManager characterManager)
        {
            if (healAmount != -1)
            {
                characterManager.health.Restore(healAmount);
            }
            else if (healPercentage > 0)
            {
                int amount = (int)(healPercentage * characterManager.health.Max / 100);
                characterManager.health.Restore(amount);
            }
        }

        public override void OnEnd(CharacterManager characterManager)
        {
        }

        public override string GetDescription()
        {
            if (healPercentage > 0)
            {
                return $"Restores {healPercentage}% of health";
            }

            return $"Restores {healAmount} health points";
        }
    }
}
