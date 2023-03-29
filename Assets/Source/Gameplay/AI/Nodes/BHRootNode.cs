using UnityEngine.Assertions;

public class BHRootNode : BHFlowNode
{
    private BHNode m_Child;

    // NOTE: Doesn't override Start(), because it must to has no children on start

    public override void Stop()
    {
        base.Stop();

        if (m_Child != null && m_Child.bActive)
        {
            m_Child?.Stop();
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_Child != null && m_Child.bActive)
        {
            m_Child?.Update();
        }
    }

    public override BHFlowNode AddNode(BHNode Node)
    {
        Assert.IsNull(m_Child, "Only one Node can be child of RootNode!");

        /* NOTE:
            Since RootNode starts and runs until BehaviorComponent is not destroyed,
            we need to call Start() on our node manually
        */
        m_Child = Node;
        m_Child.Start();

        return base.AddNode(Node);
    }

    public override BHNode AddTask(BHTaskNode Task)
    {
        base.AddTask(Task);

        /* NOTE:
            Since RootNode starts and runs until BehaviorComponent is not destroyed,
            we need to call Start() every new task manually
        */
        Task.Start();

        return this;
    }
}
