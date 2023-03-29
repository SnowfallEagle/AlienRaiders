using UnityEngine;

public class BHCommand_MoveForward : BHCommand
{
    private float m_Speed;

    public BHCommand_MoveForward(float Speed)
    {
        m_Speed = Speed;
    }

    public override void Process(Temp.BehaviorComponent Owner)
    {
        base.Process(Owner);

        Owner.transform.position += Owner.transform.up * (m_Speed * Time.deltaTime);
    }
}
