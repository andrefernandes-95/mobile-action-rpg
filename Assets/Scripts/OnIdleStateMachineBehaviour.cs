namespace AF
{
    using UnityEngine;

    public class OnIdleStateMachineBehaviour : StateMachineBehaviour
    {
        override public void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            CharacterManager manager = animator.GetComponent<CharacterManager>();
            if (manager != null)
            {
                manager.OnIdle();
            }
        }
    }
}

