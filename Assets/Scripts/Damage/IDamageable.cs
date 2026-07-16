namespace AF
{
    public interface IDamageable
    {
        DamageResult Receive(in DamagePacket packet); // Receive packet as reference (not copied)
    }
}
