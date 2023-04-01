using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

// TODO: Don't finish everything after 1 failed child...
public abstract class BHFlowNode : BHActionNode
{
    protected enum ChildHandle
    {
        NotInitialized,
        Done
    }

    protected List<BHActionNode> m_Children = new List<BHActionNode>();
    private BHActionNode m_CurrentChild;
    private ChildHandle m_CurrentChildHandle;
    private NodeStatus m_LastChildStatus;

    private List<BHDecorator> m_Decorators = new List<BHDecorator>();
    protected bool m_bUseDecorators = true;

    protected abstract ChildHandle GetNextChildHandle(ChildHandle CurrentChild, NodeStatus LastChildStatus);

    public void AddDecorator(BHDecorator Decorator)
    {
        if (!m_bUseDecorators || Decorator == null)
        {
            Assert.IsTrue(false, "Node doesn't use Decorators or given Decorator is null!");
            return;
        }

        m_Decorators.Add(Decorator);
    }

    public override void Initialize(Temp.BehaviorComponent Owner, BHFlowNode Parent)
    {
        base.Initialize(Owner, Parent);

        foreach (var Decorator in m_Decorators)
        {
            Decorator.Initialize(Owner, this);
        }

        foreach (var Child in m_Children)
        {
            Child.Initialize(Owner, this);
        }
    }

    public override NodeStatus Start()
    {
        if (!m_Children.Any())
        {
            return NodeStatus.Done;
        }

        m_CurrentChildHandle = ChildHandle.NotInitialized;
        m_LastChildStatus = NodeStatus.InProgress;

        for (;;)
        {
            m_CurrentChildHandle = GetNextChildHandle(m_CurrentChildHandle, m_LastChildStatus);
            if (m_CurrentChildHandle == ChildHandle.Done)
            {
                return NodeStatus.Done;
            }

            m_CurrentChild = m_Children[(int)m_CurrentChildHandle];
            Assert.IsNotNull(m_CurrentChild);

            BHFlowNode ChildAsFlowNode = m_CurrentChild as BHFlowNode;
            if (ChildAsFlowNode != null && !ChildAsFlowNode.CheckDecoratorsToStart())
            {
                m_LastChildStatus = NodeStatus.Failed;
                continue;
            }

            m_LastChildStatus = m_CurrentChild.Start();
            if (m_LastChildStatus == NodeStatus.InProgress)
            {
                m_CurrentChild.bActive = true;
                return NodeStatus.InProgress;
            }
        }
    }

    public override void Update()
    {
        Assert.IsNotNull(m_CurrentChild);

        // TODO: Update current child, get next
    }

    public override void Finish(NodeStatus FinishStatus)
    {
        base.Finish(FinishStatus);

        foreach (var Child in m_Children)
        {
            if (Child.bActive)
            {
                Child.Finish(FinishStatus);
            }
        }
    }

    public void OnChildFinished(BHActionNode Node, NodeStatus FinishStatus)
    {
        if (Node == null)
        {
            return;
        }

        Node.bActive = false;
        m_LastChildStatus = FinishStatus;
    }

    public virtual BHFlowNode AddNode(BHActionNode Node)
    {
        if (Node == null ||
            (!Node.GetType().IsSubclassOf(typeof(BHTaskNode)) &&
             !Node.GetType().IsSubclassOf(typeof(BHFlowNode))))
        {
            Assert.IsTrue(false, "Given node is null or not derived from Task or Flow!");
            return this;
        }

        m_Children.Add(Node);
        return this;
    }

    protected bool CheckDecoratorsToStart()
    {
        bool bResult = true;

        foreach (var Decorator in m_Decorators)
        {
            Decorator.ComputeStartCondition(ref bResult);
        }

        return bResult;
    }

    protected bool CheckDecoratorsToUpdate()
    {
        bool bResult = true;

        foreach (var Decorator in m_Decorators)
        {
            Decorator.ComputeUpdateCondition(ref bResult);
        }

        return bResult;
    }

    private void FindNextChild()
    {
        // TODO: Put finding code from Start() here
    }
}

