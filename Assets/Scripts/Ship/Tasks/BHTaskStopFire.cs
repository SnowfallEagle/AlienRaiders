public class BHTaskStopFire : BHTask
{
    public override void Start(Ship Owner)
    {
        Owner.WeaponComponent.StopFire();
        m_State = TaskState.Done;
    }
}
