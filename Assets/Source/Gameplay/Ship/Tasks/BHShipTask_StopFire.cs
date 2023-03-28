using UnityEngine;

public class BHShipTask_StopFire : BHTask
{
    public override void Start(MonoBehaviour Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StopFire();
        m_State = TaskState.Done;
    }
}
