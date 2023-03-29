using UnityEngine;

// TODO: Make command from it
// TODO: Make BHTask_LoopCommand
// TODO: Implement commands
public class BHTask_MoveForward : BHTaskNode
{
    private float m_Speed;

    public BHTask_MoveForward(float Speed)
    {
        m_Speed = Speed;
    }

    public override void Update()
    {
        base.Update();

        m_Owner.transform.position += m_Owner.transform.up * (m_Speed * Time.deltaTime);
    }
}
