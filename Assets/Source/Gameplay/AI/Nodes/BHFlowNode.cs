using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class BHFlowNode : BHNode
{
    // TODO: Maybe separate PrevChildResult and NextChildResult?
    protected enum ChildResult
    {
        Initialization, // Base sends it to initialize derived children
        InProgress,     // Base sends if previous result is InProgress, derived returns if we can use returned Child
        ReturnToParent, // Derived returns if it's InProgress and iterated all children
        Done,           // Base sends if previous result is Done, derived returns if it's Done
        Failed          // For base internal usage, indicates that child finished with Failed status
    }

    protected List<BHNode> m_Children = new List<BHNode>();
    private BHNode m_CurrentChild;
    private ChildResult m_PrevChildResult;

    protected abstract ChildResult GetNextChild(out BHNode Child, ChildResult PrevResult);

    public override void Initialize(Temp.BehaviorComponent Owner, BHFlowNode Parent)
    {
        base.Initialize(Owner, Parent);

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

        m_PrevChildResult = ChildResult.Initialization;

        for (;;)
        {
            m_PrevChildResult = GetNextChild(out m_CurrentChild, m_PrevChildResult);

            switch (m_PrevChildResult)
            {
                case ChildResult.InProgress:
                    Assert.IsNotNull(m_CurrentChild);

                    NodeStatus StartStatus = m_CurrentChild.Start();
                    switch (StartStatus)
                    {
                        case NodeStatus.InProgress:
                            m_PrevChildResult = ChildResult.InProgress;
                            m_CurrentChild.bActive = true;
                            break;

                        case NodeStatus.Done:
                            m_PrevChildResult = ChildResult.Done;
                            break;

                        case NodeStatus.Failed:
                            return NodeStatus.Failed;
                    }

                    break;

                case ChildResult.ReturnToParent:
                    return NodeStatus.InProgress;

                case ChildResult.Done:
                    return NodeStatus.Done;

                default:
                    Assert.IsTrue(false, $"Derived Flow node can't return ChildResult: { (int)m_PrevChildResult}");
                    return NodeStatus.Failed;
            }
        }
    }

    public override void Update()
    {
        Assert.IsNotNull(m_CurrentChild);

        /*
            On enter we have previous result = ReturnToParent and if m_CurrentChild.Update() doesn't call Finish(),
            then we need to set previous result = InProgress
        */
        m_PrevChildResult = ChildResult.InProgress;

        for (;;)
        {
            m_CurrentChild.Update();

            /*
                If child called Finish() then in OnChildFinished()
                we set its bActive = false and m_PrevChildResult = Done or Failed
            */
            if (m_PrevChildResult == ChildResult.Failed)
            {
                Finish(NodeStatus.Failed);
                return;
            }

            m_PrevChildResult = GetNextChild(out m_CurrentChild, m_PrevChildResult);

            switch (m_PrevChildResult)
            {
                case ChildResult.InProgress:
                    Assert.IsNotNull(m_CurrentChild);

                    if (!m_CurrentChild.bActive)
                    {
                        NodeStatus StartStatus = m_CurrentChild.Start();
                        switch (StartStatus)
                        {
                            case NodeStatus.InProgress:
                                m_PrevChildResult = ChildResult.InProgress;
                                m_CurrentChild.bActive = true;
                                break;

                            case NodeStatus.Done:
                                m_PrevChildResult = ChildResult.Done;
                                continue;

                            case NodeStatus.Failed:
                                Finish(NodeStatus.Failed);
                                return;
                        }
                    }

                    break;

                case ChildResult.ReturnToParent:
                    return;

                case ChildResult.Done:
                    Finish(NodeStatus.Done);
                    return;

                default:
                    Assert.IsTrue(false, $"Derived Flow node can't return ChildResult: { (int)m_PrevChildResult }");
                    return;
            }
        }
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

    public void OnChildFinished(BHNode Node, NodeStatus FinishStatus)
    {
        if (Node == null)
        {
            return;
        }

        Node.bActive = false;

        switch (FinishStatus)
        {
            case NodeStatus.Done:   m_PrevChildResult = ChildResult.Done; break;
            case NodeStatus.Failed: m_PrevChildResult = ChildResult.Failed; break;
        }
    }

    public virtual BHFlowNode AddNode(BHNode Node)
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
}

