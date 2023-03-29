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

    public virtual void Start(Temp.BehaviorComponent Owner, BHNode Parent)
    {
        Assert.IsNotNull(Owner);
        // Parent is null for RootNode

        m_Owner = Owner;
        m_Parent = Parent;

        m_bActive = true;

        foreach (var Task in m_Tasks)
        {
            Task.Start(Owner, this);
        }
    }

    // TODO: Maybe make enum StopReason: Ended, Restarted?
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

    public virtual void Restart()
    {
        Stop();
        Start(m_Owner, m_Parent);
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

    /** Task must be BHTaskNode
        BHNode here because otherwise we may have to cast argument explicitly to
        BHTaskNode (e.g. after calling .AddOnNodeEnded())
    */
    public virtual BHNode AddTask(BHNode Task)
    {
        Assert.IsNotNull(Task as BHTaskNode);

        m_Tasks.Add((BHTaskNode)Task);

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
