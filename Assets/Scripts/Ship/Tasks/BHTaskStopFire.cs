public class BHTaskStopFire : BHTask
{
    public override void Start(Ship Owner)
    {
        Owner.GetCustomComponent<ShipWeaponComponent>().StopFire();
        m_State = TaskState.Done;
    }
}
