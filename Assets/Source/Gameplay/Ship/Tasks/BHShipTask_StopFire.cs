public class BHShipTask_StopFire : BHTask
{
    public override void Start()
    {
        m_Owner.GetComponent<ShipWeaponComponent>().StopFire();
        m_State = TaskState.Done;
    }
}
