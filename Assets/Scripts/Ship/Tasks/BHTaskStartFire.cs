public class BHTaskStartFire : BHTask
{
    public override void Start(Ship Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StartFire();
        m_State = TaskState.Done;
    }
}
