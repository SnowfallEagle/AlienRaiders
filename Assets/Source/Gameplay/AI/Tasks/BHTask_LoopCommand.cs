public class BHTask_LoopCommand : BHTaskNode
{
    private BHCommand m_Command;

    public BHTask_LoopCommand(BHCommand Command)
    {
        m_Command = Command;
    }

    public override void Update()
    {
        base.Update();

        m_Command.Process(m_Owner);
    }
}
