namespace AF
{
    using UnityEngine;

    public class OnReviveStateExit : StateMachineBehaviour
    {
        override public void OnStateExit(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            CharacterManager manager = animator.GetComponent<CharacterManager>();
            if (manager != null)
            {
                manager.OnIdle();
                manager.health.Revive();
            }
        }
    }
}

