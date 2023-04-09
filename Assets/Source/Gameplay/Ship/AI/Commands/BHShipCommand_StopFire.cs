public class BHShipCommand_StopFire : BHCommand
{
    public override void Process(BehaviorComponent Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StopFire();
    }
}
