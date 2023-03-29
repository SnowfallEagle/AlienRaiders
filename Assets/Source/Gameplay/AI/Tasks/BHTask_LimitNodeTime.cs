public class BHTask_LimitNodeTime : BHTaskNode
{
    private float m_TimeLimit;

    public BHTask_LimitNodeTime(float TimeLimit)
    {
        m_TimeLimit = TimeLimit;
    }

    public override void Start()
    {
        base.Start();

        TimerService.Instance.AddTimer(null, m_Owner,
            () =>
            {
                m_Parent.Stop();
            },
            m_TimeLimit
        );
    }
}
