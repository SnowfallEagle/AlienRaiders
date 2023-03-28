using UnityEngine;

public class BHTaskStopFire : BHTask
{
    public override void Start(MonoBehaviour Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StopFire();
        m_State = TaskState.Done;
    }
}
