using UnityEngine;

namespace AF
{
    public static class AnimHashes
    {
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int Idle = Animator.StringToHash("Locomotion");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Attack = Animator.StringToHash("Attack");
    }
}
