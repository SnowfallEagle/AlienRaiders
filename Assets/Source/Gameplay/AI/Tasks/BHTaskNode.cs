public class BHTaskNode : BHNode
{
    // TODO: Maybe make NodeState?
    public enum TaskState
    {
        InProgress,
        Done,
        Failed
    }

    private TaskState m_State = TaskState.InProgress;
    public TaskState State
    {
        get => m_State;
        set
        {
            m_State = value;
            if (m_State == TaskState.Done || m_State == TaskState.Failed)
            {
                Stop();
            }
        }
    }
}

// TODO: Delete
/* DEPRECATED */
public class BHTask
{
    public enum TaskState
    {
        InProgress,
        Done,
        Failed
    }

    public TaskState State = TaskState.InProgress;

    public bool bEnded => State == TaskState.Done || State == TaskState.Failed;

    public delegate void OnEndedSignature(BHTask Task);
    public OnEndedSignature OnEnded;

    protected BehaviorComponent m_Owner;

    public virtual void Start()
    { }

    public virtual void Update()
    { }

    public void Initialize(BehaviorComponent Owner, BHNode Parent)
    {
        m_Owner = Owner;
    }
}
