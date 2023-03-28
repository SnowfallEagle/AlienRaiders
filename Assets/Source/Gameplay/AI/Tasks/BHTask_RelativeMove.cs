using UnityEngine;

public class BHTask_RelativeMove : BHTask
{
    private Vector3 m_DeltaPosition;

    public BHTask_RelativeMove(Vector3 DeltaPosition)
    {
        m_DeltaPosition = DeltaPosition;
    }

    public override void Start(MonoBehaviour Owner)
    {
        Owner.transform.position += m_DeltaPosition;
        m_State = TaskState.Done;
    }
}
