public class BHTaskStartFire : BHTask
{
    public override void Start(Ship Owner)
    {
        Owner.GetCustomComponent<ShipWeaponComponent>().StartFire();
        m_State = TaskState.Done;
    }
}
