using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHTask_MoveVertical : BHTask
{
    private float m_Speed;
    private BehaviorComponent m_BehaviorComponent;

    public BHTask_MoveVertical(float Speed = 5f)
    {
        m_Speed = Speed;
    }

    public override void Start()
    {
        m_BehaviorComponent = m_Owner.GetComponent<BehaviorComponent>();
    }

    public override void Update()
    {
        m_BehaviorComponent.AddTask(new BHTask_RelativeMove(new Vector3(0f, m_Speed * Time.deltaTime, 0f)));
    }
}
