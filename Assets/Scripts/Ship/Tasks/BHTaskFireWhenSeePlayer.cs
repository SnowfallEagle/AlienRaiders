using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHTaskFireWhenSeePlayer : BHTask
{
    private float m_FOV;

    public BHTaskFireWhenSeePlayer(float FOV)
    {
        m_FOV = FOV;
    }

    public override void Update(Ship Owner)
    {
        PlayerShip PlayerShip = ServiceLocator.Instance.Get<PlayerState>().PlayerShip;
        if (!PlayerShip)
        {
            Owner.AddTask(new BHTaskStopFire());
            return;
        }

        Vector3 ToPlayerVector = Vector3.Normalize(PlayerShip.transform.position - Owner.transform.position);
        float CosAngle = Vector3.Dot(Owner.transform.up, ToPlayerVector);
        float Angle = Mathf.Acos(CosAngle);

#if UNITY_EDITOR && DEBUG
        Debug.DrawRay(Owner.transform.position, Owner.transform.up * 10f, Color.blue);
        Debug.DrawRay(Owner.transform.position, PlayerShip.transform.position - Owner.transform.position, Color.red);
#endif

        if (Mathf.Abs(Angle) < m_FOV)
        {
            Owner.AddTask(new BHTaskStartFire());
        }
        else
        {
            Owner.AddTask(new BHTaskStopFire());
        }
    }
}
