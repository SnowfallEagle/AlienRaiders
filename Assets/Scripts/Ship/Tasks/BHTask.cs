public class BHTask
{
    public enum TaskState
    {
        InProgress,
        Done,
        Failed
    }

    protected TaskState m_State = TaskState.InProgress;
    public TaskState State { get => m_State; }

    public bool bEnded { get => State == TaskState.Done || State == TaskState.Failed; }

    public virtual void Start(Ship Owner)
    { }

    public virtual void Update(Ship Owner)
    { }
}
