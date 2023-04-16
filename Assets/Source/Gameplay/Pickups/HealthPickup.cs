using UnityEngine;

public class HealthPickup : Pickup
{

    protected override bool GivePickup(PlayerShip Ship)
    {
        Ship.HealthComponent.SetMaxHealth();
        return true;
    }
}
