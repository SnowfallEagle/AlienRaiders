using System.Collections.Generic;
using UnityEngine.Assertions;

public class BehaviorComponent : CustomBehavior
{
    private BHFlow_Root m_Root = new BHFlow_Root();

    private List<BHAction> m_Actions = new List<BHAction>();
    private List<BHAction> m_FixedActions = new List<BHAction>();

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
                FinishAction(m_Actions[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = m_FixedActions.Count - 1; i >= 0; --i)
        {
            if (!m_FixedActions[i].Update())
            {
                FinishAction(m_FixedActions[i]);
            }
        }
    }

    private void OnDestroy()
    {
        FinishBehavior();
    }

    /** Node must be BHTaskNode or BHFlowNode */
    public void StartBehavior(BHActionNode Node = null)
    {
        FinishBehavior();

        if (Node == null)
        {
            NoEntry.Assert("Behavior started with null Node!");
            return;
        }

        m_Root.AddNode(Node);

        m_Root.Initialize(this, null);
        m_Root.Start();
        m_Root.bActive = true;
    }

    public void FinishBehavior()
    {
        if (m_Root.bActive)
        {
            m_Root.Finish(BHNode.NodeStatus.Done);
            m_Root.bActive = false;
        }
    }

    public void AddAction(BHAction Action)
    {
        if (Action == null)
        {
            return;
        }

        Action.Initialize(this);
        if (Action.Start())
        {
            if (Action.bFixedUpdate)
            {
                m_FixedActions.Add(Action);
            }
            else
            {
                m_Actions.Add(Action);
            }
        }
    }

    public void AddExclusiveAction(BHAction ExclusiveAction)
    {
        if (ExclusiveAction == null)
        {
            return;
        }

        System.Type ExclusiveType = ExclusiveAction.GetType();

        List<BHAction> ActionList = ExclusiveAction.bFixedUpdate ? m_FixedActions : m_Actions;
        for (int i = ActionList.Count - 1; i >= 0; --i)
        {
            if (ActionList[i].GetType() == ExclusiveType)
            {
                AbortAction(ActionList[i]);
            }
        }

        AddAction(ExclusiveAction);
    }

    public void AbortAction(BHAction Action)
    {
        if (Action != null && !Action.bDone)
        {
            RemoveAction(Action);
            Action.OnAbort();
            Action.OnActionFinished?.Invoke(Action);
        }
    }

    public void FinishAction(BHAction Action)
    {
        if (Action != null && !Action.bDone)
        {
            RemoveAction(Action);
            Action.OnFinish();
            Action.OnActionFinished?.Invoke(Action);
        }
    }

    private void RemoveAction(BHAction Action)
    {
        if (Action != null)
        {
            List<BHAction> ActionList = Action.bFixedUpdate ? m_FixedActions : m_Actions;
            ActionList.Remove(Action);
        }
    }

    /** Clears all actions, but doesn't remove timers! */
    public void ClearActions()
    {
        m_Actions.Clear();
        m_FixedActions.Clear();
    }

    public void AbortActions()
    {
        for (int i = m_Actions.Count - 1; i >= 0; --i)
        {
            AbortAction(m_Actions[i]);
        }

        for (int i = m_FixedActions.Count - 1; i >= 0; --i)
        {
            AbortAction(m_FixedActions[i]);
        }

        ClearActions();
    }
}
