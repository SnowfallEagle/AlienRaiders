public class BHTaskStopFire : BHTask
{
    public override void Start(Ship Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StopFire();
        m_State = TaskState.Done;
    }
}
