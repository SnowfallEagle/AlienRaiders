public class BHShipTask_StopFire : BHTask
{
    public override void Start()
    {
        m_Owner.GetComponent<ShipWeaponComponent>().StopFire();
        State = TaskState.Done;
    }
}
