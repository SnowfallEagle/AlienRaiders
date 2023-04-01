using System;
using UnityEngine;

public class BHTask_FollowTarget : BHTask
{
    private MonoBehaviour m_Target;

    private float m_Speed;
    private float m_DiffModifier;

    public BHTask_FollowTarget(MonoBehaviour Target, float Speed = 5f, float DiffModifier = 0.005f)
    {
        m_Target = Target;
        m_Speed = Speed;
        m_DiffModifier = DiffModifier;
    }

    public override void Update()
    {
        Vector3 CurrentDirection = m_Owner.transform.up;
        Vector3 TargetDirection = m_Target ?
            (m_Target.transform.position - m_Owner.transform.position).normalized :
            CurrentDirection;

        m_Owner.transform.up = CurrentDirection + (TargetDirection - CurrentDirection) * m_DiffModifier;
        m_Owner.AddTask(new BHTask_RelativeMove(m_Owner.transform.up * (m_Speed * Time.deltaTime)));
    }
}
