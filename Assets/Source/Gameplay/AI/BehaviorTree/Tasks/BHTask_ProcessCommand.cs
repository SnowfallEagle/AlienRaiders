public class BHTask_ProcessCommand : BHTaskNode
{
    private BHCommand m_Command;

    public BHTask_ProcessCommand(BHCommand Command)
    {
        m_Command = Command;
    }

    public override void Update()
    {
        m_Command.Process(m_Owner);
        Finish(NodeStatus.Done);
    }
}
