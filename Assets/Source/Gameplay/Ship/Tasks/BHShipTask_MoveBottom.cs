using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHShipTask_MoveBottom : BHTask
{
    private float m_Speed;
    private BehaviorComponent m_BehaviorComponent;

    public override void Start(MonoBehaviour Owner)
    {
        m_Speed = -Owner.GetComponent<Ship>().Speed;
        m_BehaviorComponent = Owner.GetComponent<BehaviorComponent>();
    }

    public override void Update(MonoBehaviour Owner)
    {
        m_BehaviorComponent.AddTask(new BHTask_RelativeMove(new Vector3(0f, m_Speed * Time.deltaTime, 0f)));
    }
}
