using UnityEngine.Assertions;

public abstract class BHActionNode : BHNode
{
    // Only for parent FlowNode to mark started nodes!
    public bool bActive = false;

    public delegate void OnNodeFinishedSignature(BHNode Node, NodeStatus Status);
    private OnNodeFinishedSignature m_OnNodeFinished;

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

        m_OnNodeFinished?.Invoke(this, FinishStatus);
    }

    public BHActionNode AddOnNodeFinished(OnNodeFinishedSignature Callback)
    {
        if (Callback != null)
        {
            m_OnNodeFinished+= Callback;
        }
        return this;
    }
}
