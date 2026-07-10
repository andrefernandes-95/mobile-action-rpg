namespace AF
{
    using UnityEngine;

    public class CharacterAnimations : MonoBehaviour
    {
        public Animator animator;

        [Header("Override Animations")]
        public AnimationClip idle;
        public AnimationClip run;
        public AnimationClip attack;
        public AnimationClip dodge;
        public AnimationClip hit;
        public AnimationClip death;

        private AnimatorOverrideController overrideController;

        void Start()
        {
            if (animator == null)
            {
                Debug.LogError("Animator not assigned!");
                return;
            }

            // Create override controller from existing controller
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            // Assign overrides
            if (idle != null)
                overrideController["Idle"] = idle;

            if (run != null)
                overrideController["Run"] = run;

            if (attack != null)
                overrideController["Attack A"] = attack;

            if (hit != null)
                overrideController["Hit"] = hit;

            if (dodge != null)
                overrideController["Dodge"] = dodge;

            if (death != null)
                overrideController["Death"] = death;

            // Apply override controller
            animator.runtimeAnimatorController = overrideController;
        }
    }
}
