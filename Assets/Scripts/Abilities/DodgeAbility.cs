using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Ability/Dodge Ability")]
    public class DodgeAbility : Ability
    {
        public AnimationName dodgeAnimation;

        public override void OnStart(CharacterManager characterManager)
        {
            characterManager.dodge.PerformDodge(dodgeAnimation.name);
        }

        public override void OnEnd(CharacterManager characterManager)
        {
        }

    }
}
