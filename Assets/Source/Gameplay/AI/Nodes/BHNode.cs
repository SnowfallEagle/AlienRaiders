using System.Collections.Generic;

public class BHNode
{
    public bool bActive = false;

    protected Temp.BehaviorComponent m_Owner;
    protected BHNode m_Parent;

    protected List<BHTaskNode> m_Tasks = new List<BHTaskNode>();

    public void Initialize(Temp.BehaviorComponent Owner, BHNode Parent)
    {
        m_Owner = Owner;
        m_Parent = Parent;
    }

    public virtual void Start()
    {
        bActive = true;

        foreach (var Task in m_Tasks)
        {
            Task.Start();
        }
    }

    public virtual void Update()
    {
        foreach (var Task in m_Tasks)
        {
            if (Task.bActive)
            {
                Task.Update();
            }
        }
    }

    public virtual BHNode AddTask(BHTaskNode Task)
    {
        Task.Initialize(m_Owner, this);
        m_Tasks.Add(Task);

        return this;
    }
}
