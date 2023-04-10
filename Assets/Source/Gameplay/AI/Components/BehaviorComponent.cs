using System.Collections.Generic;
using UnityEngine.Assertions;

public class BehaviorComponent : CustomBehavior
{
    private BHFlow_Root m_Root = new BHFlow_Root();
    private List<BHAction> m_Actions = new List<BHAction>();

    private void LateUpdate()
    {
        // Behavior tree
        if (m_Root.bActive)
        {
            m_Root.Update();
        }

        // Actions
        for (int i = m_Actions.Count - 1; i >= 0; --i)
        {
            if (!m_Actions[i].Update())
            {
                m_Actions.RemoveAt(i);
            }
        }
    }

    private void OnDestroy()
    {
        StopBehavior();
    }

    /** Node must be BHTaskNode or BHFlowNode */
    public void StartBehavior(BHActionNode Node = null)
    {
        StopBehavior();

        if (Node == null)
        {
            Assert.IsTrue(false, "Behavior started with null Node!");
            return;
        }

        m_Root.AddNode(Node);

        m_Root.Initialize(this, null);
        m_Root.Start();
        m_Root.bActive = true;
    }

    public void StopBehavior()
    {
        if (m_Root.bActive)
        {
            m_Root.Finish(BHNode.NodeStatus.Done);
            m_Root.bActive = false;
        }
    }

    public void AddAction(BHAction Action)
    {
        Action.Initialize(this);
        if (Action.Start())
        {
            m_Actions.Add(Action);
        }
    }

    public void AddExclusiveAction(BHAction ExclusiveAction)
    {
        System.Type ExclusiveType = ExclusiveAction.GetType();
        m_Actions.RemoveAll((Action) =>
        {
            if (Action.GetType() == ExclusiveType)
            {
                Action.Abort();
                return true;
            }
            return false;
        });

        AddAction(ExclusiveAction);
    }
}
