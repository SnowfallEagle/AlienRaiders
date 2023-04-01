using System.Collections.Generic;
using UnityEngine.Assertions;

public abstract class BHNode
{
    public enum NodeStatus
    {
        InProgress,
        Done,
        Failed
    }

    // Only for parent FlowNode to mark started nodes!
    public bool bActive = false;

    public delegate void OnNodeEndedSignature(BHNode Task, NodeStatus Status);
    private OnNodeEndedSignature m_OnNodeEnded;

    protected Temp.BehaviorComponent m_Owner;
    protected BHFlowNode m_Parent;

    protected bool m_bUseDecorators = true;
    /* TODO
    private List<BHDecorator> m_Decorators = new List<BHDecorator>();

    public void AddDecorator(BHDecorator Decorator)
    {
        Assert.IsTrue(m_bUseDecorators, "This node does not use decorators!");
        Assert.IsNotNull(Decorator);

        m_Decorators.Add(Decorator);
    }
    */

    public virtual void Initialize(Temp.BehaviorComponent Owner, BHFlowNode Parent)
    {
        Assert.IsNotNull(Owner);
        // Parent is null for RootNode

        m_Owner = Owner;
        m_Parent = Parent;
    }

    public virtual NodeStatus Start()
    {
        return NodeStatus.InProgress;
    }

    public virtual void Update()
    { }

    /** Can be called only in Update()!
        Finish status must be Done or Failed.
    */
    public virtual void Finish(NodeStatus FinishStatus)
    {
        if (!bActive && FinishStatus == NodeStatus.InProgress)
        {
            Assert.IsTrue(false, "Child wanted to finish with being not activated or InProgress status!");
            return;
        }

        if (m_Parent != null)
        {
            m_Parent.OnChildFinished(this, FinishStatus);
        }
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
