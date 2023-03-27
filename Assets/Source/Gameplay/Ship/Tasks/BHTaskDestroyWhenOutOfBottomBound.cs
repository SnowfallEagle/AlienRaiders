using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHTaskDestroyWhenOutOfBottomBound : BHTask
{
    private float YBound;

    public override void Start(Ship Owner)
    {
        var Renderer = RenderingService.Instance;
        YBound = Renderer.TargetCenter.y - (Renderer.TargetSize.y * 0.6f);
    }

    public override void Update(Ship Owner)
    {
        if (Owner.transform.position.y < YBound)
        {
            Owner.GetComponent<ShipHealthComponent>().Kill();
            m_State = TaskState.Done;
        }
    }
}
