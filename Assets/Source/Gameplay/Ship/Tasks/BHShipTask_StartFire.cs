using UnityEngine;

public class BHShipTask_StartFire : BHTask
{
    public override void Start(MonoBehaviour Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StartFire();
        m_State = TaskState.Done;
    }
}
