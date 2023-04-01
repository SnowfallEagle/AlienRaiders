using UnityEngine.Assertions;

public abstract class BHActionNode : BHNode
{
    // Only for parent FlowNode to mark started nodes!
    public bool bActive = false;

    public delegate void OnNodeEndedSignature(BHNode Node, NodeStatus Status);
    private OnNodeEndedSignature m_OnNodeEnded;

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

        m_OnNodeEnded?.Invoke(this, FinishStatus);
    }

    public BHActionNode AddOnNodeEnded(OnNodeEndedSignature Callback)
    {
        if (Callback != null)
        {
            m_OnNodeEnded += Callback;
        }
        return this;
    }
}
