using UnityEngine.Assertions;

public class BHRootNode : BHFlowNode
{
    private BHNode m_Child;

    public override void Start(Temp.BehaviorComponent Owner, BHNode Parent)
    {
        base.Start(Owner, Parent);

        m_Child?.Start(Owner, this);
    }

    public override void Stop()
    {
        base.Stop();

        if (m_Child != null && m_Child.bActive)
        {
            m_Child.Stop();
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_Child == null)
        {
            return;
        }

        if (m_Child.bActive)
        {
            m_Child.Update();
        }
        else
        {
            Restart();
        }
    }

    public override BHFlowNode AddNode(BHNode Node)
    {
        Assert.IsNull(m_Child, "Only one Node can be child of RootNode!");

        /* NOTE:
            Since RootNode starts and runs until BehaviorComponent is not destroyed,
            we need to call Start() on our node manually
        */
        Node.Start(m_Owner, this);
        m_Child = Node;

        return base.AddNode(Node);
    }

    public override BHNode AddTask(BHNode Task)
    {
        base.AddTask(Task);

        /* NOTE:
            Since RootNode starts and runs until BehaviorComponent is not destroyed,
            we need to call Start() every new task manually
        */
        Task.Start(m_Owner, this);

        return this;
    }
}
