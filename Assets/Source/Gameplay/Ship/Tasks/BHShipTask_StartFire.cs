public class BHShipTask_StartFire : BHTask
{
    public override void Start()
    {
        m_Owner.GetComponent<ShipWeaponComponent>().StartFire();
        State = TaskState.Done;
    }
}
