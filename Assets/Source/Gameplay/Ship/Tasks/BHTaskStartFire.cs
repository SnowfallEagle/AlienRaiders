using UnityEngine;

public class BHTaskStartFire : BHTask
{
    public override void Start(MonoBehaviour Owner)
    {
        Owner.GetComponent<ShipWeaponComponent>().StartFire();
        m_State = TaskState.Done;
    }
}
