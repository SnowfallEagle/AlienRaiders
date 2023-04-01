using System.Collections.Generic;
using UnityEngine.Assertions;

#if null
public class BHSequenceNode : BHFlowNode
{
    private List<BHNode> m_Children = new List<BHNode>();
    private int m_NumChildren;

    private BHNode m_CurrentChild;
    private int m_CurrentChildIndex;

    public override NodeStatus Start()
    {
        m_CurrentChildIndex = 0;
        m_NumChildren = m_Children.Count;

        if (m_NumChildren <= 0)
        {
            return NodeStatus.Done;
        }

        m_CurrentChild = m_Children[m_CurrentChildIndex];
        NodeStatus StartStatus = m_CurrentChild.Start();

        switch (StartStatus)
        {
            case NodeStatus.InProgress:
                m_CurrentChild.bActive = true;
                break;

            case NodeStatus.Done:
                m_CurrentChild.Stop(StartStatus);
                // TODO: Try start next while we have remaining children
                break;

            case NodeStatus.Failed:
                m_CurrentChild.Stop(StartStatus);
                break;
        }

        return StartStatus;
    }

    public override void Stop(NodeStatus StopStatus)
    {
        base.Stop(StopStatus);

        if (m_CurrentChild != null && m_CurrentChild.bActive)
        {
            m_CurrentChild.Stop(StopStatus);
        }
    }

    public override NodeStatus Update()
    {
        Assert.IsNotNull(m_CurrentChild);

        NodeStatus UpdateStatus = m_CurrentChild.Update();
        if (UpdateStatus == NodeStatus.Failed)
        {
            m_CurrentChild.Stop(UpdateStatus);
            return NodeStatus.Failed;
        }

        if (UpdateStatus == NodeStatus.Done)
        {
            m_CurrentChild.Stop(UpdateStatus);

            if (++m_CurrentChildIndex < m_NumChildren)
            {
                m_CurrentChild = m_Children[m_CurrentChildIndex];
                m_CurrentChild.Start();
                // TODO: Try to start while we have children
            }
            else
            {
                return NodeStatus.Done;
            }

            // TODO
        }

        return NodeStatus.InProgress;
    }

    public override BHFlowNode AddNode(BHNode Node)
    {
        m_Children.Add(Node);

        return base.AddNode(Node);
    }
}
#endif