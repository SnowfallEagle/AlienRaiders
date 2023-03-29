using System.Collections.Generic;
using UnityEngine.Assertions;

public class BHNode
{
    private bool m_bActive = false;
    public bool bActive => m_bActive;

    public delegate void OnNodeEndedSignature(BHNode Task);
    private OnNodeEndedSignature m_OnNodeEnded;

    protected Temp.BehaviorComponent m_Owner;
    protected BHNode m_Parent;

    protected List<BHTaskNode> m_Tasks = new List<BHTaskNode>();

    /** FlowNodes must call Start() on their children
    */
    public virtual void Start(Temp.BehaviorComponent Owner, BHNode Parent)
    {
        Assert.IsNotNull(Owner);
        // Parent is null for RootNode

        m_Owner = Owner;
        m_Parent = Parent;

        m_bActive = true;

        foreach (var Task in m_Tasks)
        {
            Task.Start(Owner, Parent);
        }
    }

    /** FlowNodes must call Stop() on their children
    */
    public virtual void Stop()
    {
        m_bActive = false;
        m_OnNodeEnded?.Invoke(this);

        foreach (var Task in m_Tasks)
        {
            if (Task.bActive)
            {
                Task.Stop();
            }
        }
    }

    /** FlowNodes must call Update() on their children
    */
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
        Assert.IsNotNull(Task);

        m_Tasks.Add(Task);

        return this;
    }

    public BHNode AddOnNodeEnded(OnNodeEndedSignature Callback)
    {
        if (Callback != null)
        {
            m_OnNodeEnded += Callback;
        }
        return this;
    }
}
