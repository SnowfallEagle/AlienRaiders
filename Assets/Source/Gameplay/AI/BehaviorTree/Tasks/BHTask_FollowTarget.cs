using System;
using UnityEngine;

public class BHTask_FollowTarget : BHTaskNode
{
    private MonoBehaviour m_Target;

    private float m_Speed;
    private float m_DiffModifier;

    public BHTask_FollowTarget(MonoBehaviour Target, float Speed = 5f, float DiffModifier = 0.01f)
    {
        m_Target = Target;
        m_Speed = Speed;
        m_DiffModifier = DiffModifier;
    }

    public override void Update()
    {
        // Move first, because target could been destroyed and we'll stop for 1 frame
        new BHCommand_MoveForward(m_Speed).Process(m_Owner);

        if (!m_Target)
        {
            Finish(NodeStatus.Failed);
            return;
        }

        Vector3 CurrentDirection = m_Owner.transform.up;
        Vector3 TargetDirection = (m_Target.transform.position - m_Owner.transform.position).normalized;

        m_Owner.transform.up = CurrentDirection + (TargetDirection - CurrentDirection) * m_DiffModifier;
    }
}
