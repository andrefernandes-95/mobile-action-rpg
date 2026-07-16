namespace AF
{
    public readonly struct DamageResult
    {
        public readonly int RawAmount;
        public readonly int FinalDamage;
        public readonly DamageType DamageType;
        public readonly CharacterManager Source;
        public readonly bool WasBlocked;

        public DamageResult(
            int rawAmount,
            int finalDamage,
            DamageType damageType,
            CharacterManager source,
            bool wasBlocked
        )
        {
            RawAmount = rawAmount;
            FinalDamage = finalDamage;
            DamageType = damageType;
            Source = source;
            WasBlocked = wasBlocked;
        }

        public static DamageResult Blocked(DamagePacket packet)
        {
            return new DamageResult(packet.Amount, 0, packet.DamageType, packet.Source, true);
        }
    }
}