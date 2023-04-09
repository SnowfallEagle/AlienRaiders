public class BHShipCommand_StartFire : BHCommand
{
    public override void Process(BehaviorComponent Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StartFire();
    }
}
