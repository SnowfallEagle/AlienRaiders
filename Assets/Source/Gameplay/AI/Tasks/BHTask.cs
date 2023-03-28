public class BHTask
{
    public enum TaskState
    {
        InProgress,
        Done,
        Failed
    }

    public TaskState State = TaskState.InProgress;
    protected BehaviorComponent m_Owner;

    public bool bEnded => State == TaskState.Done || State == TaskState.Failed;

    public virtual void Start()
    { }

    public virtual void Update()
    { }

    /* Internal */
    public void InternalInitialize(BehaviorComponent Owner)
    {
        m_Owner = Owner;
    }
}
