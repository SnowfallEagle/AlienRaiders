public class BHTaskStartFire : BHTask
{
    public override void Start(Ship Owner)
    {
        Owner.WeaponComponent.StartFire();
        m_State = TaskState.Done;
    }
}
