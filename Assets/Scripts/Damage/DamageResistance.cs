using UnityEngine;

namespace AF
{
    [System.Serializable]
    public struct DamageResistance
    {
        public DamageType damageType;

        [Tooltip("0 - No Reduction; 1 - Immune")]
        [Range(0f, 1f)]
        public float reduction;
    }
}
