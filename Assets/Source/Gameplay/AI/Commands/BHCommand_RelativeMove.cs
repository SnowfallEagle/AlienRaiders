using UnityEngine;

public class BHCommand_RelativeMove : BHCommand
{
    private Vector3 m_DeltaPosition;

    public BHCommand_RelativeMove(Vector3 DeltaPosition)
    {
        m_DeltaPosition = DeltaPosition;
    }

    public override void Process(BehaviorComponent Owner)
    {
        Owner.transform.position += m_DeltaPosition;
    }
}
