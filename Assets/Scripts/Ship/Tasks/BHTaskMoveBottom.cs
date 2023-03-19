using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHTaskMoveBottom : BHTask
{
    public override void Update(Ship Owner)
    {
        Owner.AddTask(new BHTaskRelativeMove(new Vector3(0f, -Owner.Speed * Time.deltaTime, 0f)));
    }
}
