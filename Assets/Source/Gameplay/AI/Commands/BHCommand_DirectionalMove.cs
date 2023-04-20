using UnityEngine;

public class BHCommand_DirectionalMove: BHCommand
{
    private Vector3 m_Direction;
    private float m_Speed;

    public BHCommand_DirectionalMove(Vector3 Direction, float Speed)
    {
        m_Direction = Direction;
        m_Speed = Speed;
    }

    public override void Process(BehaviorComponent Owner)
    {
        Owner.transform.position += m_Direction * (m_Speed * Time.deltaTime);
    }
}
