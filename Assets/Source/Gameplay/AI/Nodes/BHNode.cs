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

    protected Temp.BehaviorComponent m_Owner;
    protected BHFlowNode m_Parent;

    public virtual void Initialize(Temp.BehaviorComponent Owner, BHFlowNode Parent)
    {
        Assert.IsNotNull(Owner);
        // Parent is null for RootNode

        m_Owner = Owner;
        m_Parent = Parent;
    }
}
