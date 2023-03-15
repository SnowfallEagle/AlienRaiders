using UnityEngine;

public class BHTaskMoveByDelta : BHTask
{
    private Vector3 m_DeltaPosition;

    public BHTaskMoveByDelta(Vector3 DeltaPosition)
    {
        m_DeltaPosition = DeltaPosition;
    }

    public override void Start(Ship Owner)
    {
        Owner.transform.position += m_DeltaPosition;
        m_State = TaskState.Done;
    }
}
