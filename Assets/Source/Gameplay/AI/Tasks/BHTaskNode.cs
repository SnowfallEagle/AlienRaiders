public class BHTaskNode : BHNode
{
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
