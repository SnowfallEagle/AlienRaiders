public class BuffMultipliers
{
    public float ShipHealth = 1f;
    public float ShipSpeed = 1f;

    public float ProjectileDamage = 1f;
    public float ProjectileSpeed = 1;

    public static BuffMultipliers operator *(BuffMultipliers First, BuffMultipliers Second)
    {
        return new BuffMultipliers
        {
            ShipHealth = First.ShipHealth * Second.ShipHealth,
            ShipSpeed = First.ShipSpeed * Second.ShipSpeed,

            ProjectileDamage = First.ProjectileDamage * Second.ProjectileDamage,
            ProjectileSpeed = First.ProjectileSpeed * Second.ProjectileSpeed
        };
    }
}
