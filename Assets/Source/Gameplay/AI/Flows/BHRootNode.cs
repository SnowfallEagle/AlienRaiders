using UnityEngine;
using UnityEngine.Assertions;

public class BHRootNode : BHFlowNode
{
    private NodeStatus m_LastStartStatus;

    public BHRootNode()
    {
        m_bUseDecorators = false;
    }

    protected override ChildHandle GetNextChildHandle(ChildHandle CurrentChild, NodeStatus LastChildStatus)
    {
        if (CurrentChild == ChildHandle.NotInitialized)
        {
            return (ChildHandle)0;
        }

        return ChildHandle.Done;
    }

    public override NodeStatus Start()
    {
        m_LastStartStatus = base.Start();
        return m_LastStartStatus;
    }

    public override void Update()
    {
        if (m_LastStartStatus != NodeStatus.InProgress && Start() != NodeStatus.InProgress)
        {
            return;
        }

        base.Update();
    }

    public override BHFlowNode AddNode(BHActionNode Node)
    {
        if (m_Children.Count > 1)
        {
            Assert.IsTrue(false, "Root node can have only 1 child!");
            return this;
        }

        return base.AddNode(Node);
    }
}
