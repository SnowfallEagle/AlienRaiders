using System.Collections.Generic;
using System.Linq;

public class BHSequenceNode : BHFlowNode
{
    private List<BHNode> m_Children = new List<BHNode>();
    private int m_NumChildren;

    private BHNode m_CurrentChild;
    private int m_CurrentChildIndex;

    public override void Start(Temp.BehaviorComponent Owner, BHNode Parent)
    {
        base.Start(Owner, Parent);

        m_CurrentChildIndex = 0;
        m_NumChildren = m_Children.Count;

        if (m_NumChildren > 0)
        {
            m_CurrentChild = m_Children[m_CurrentChildIndex];
            m_CurrentChild.Start(Owner, this);
        }
    }

    public override void Stop()
    {
        base.Stop();

        if (m_CurrentChild != null && m_CurrentChild.bActive)
        {
            m_CurrentChild.Stop();
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_CurrentChild == null)
        {
            return;
        }

        if (m_CurrentChild.bActive)
        {
            m_CurrentChild.Update();
            return;
        }

        if (++m_CurrentChildIndex < m_NumChildren)
        {
            m_CurrentChild = m_Children[m_CurrentChildIndex];
            m_CurrentChild.Start(m_Owner, this);
        }
        else
        {
            Restart();
        }
    }

    public override BHFlowNode AddNode(BHNode Node)
    {
        m_Children.Add(Node);

        return base.AddNode(Node);
    }
}