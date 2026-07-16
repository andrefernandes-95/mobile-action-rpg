using UnityEngine;

namespace AF
{
    public readonly struct DamagePacket
    {
        public readonly CharacterManager Source;
        public readonly DamageType DamageType;
        public readonly int Amount;
        public readonly Vector3 HitPoint;

        public DamagePacket(
            CharacterManager source,
            DamageType type,
            int amount,
            Vector3 hitPoint
        )
        {
            Source = source;
            DamageType = type;
            Amount = amount;
            HitPoint = hitPoint;
        }
    }
}
