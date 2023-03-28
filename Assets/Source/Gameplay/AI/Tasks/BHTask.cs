using UnityEngine;

public class BHTask
{
    public enum TaskState
    {
        InProgress,
        Done,
        Failed
    }

    protected TaskState m_State = TaskState.InProgress;
    public TaskState State => m_State;

    public bool bEnded => State == TaskState.Done || State == TaskState.Failed;

    public virtual void Start(MonoBehaviour Owner)
    { }

    public virtual void Update(MonoBehaviour Owner)
    { }
}
