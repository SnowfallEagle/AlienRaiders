using UnityEngine.Assertions;

public class BHRootNode : BHFlowNode
{
    private BHNode m_Child;

    public override void Start()
    {
        base.Start();

        m_Child?.Start();
    }

    public override void Update()
    {
        base.Update();

        m_Child?.Update();
    }

    public override BHFlowNode AddNode(BHNode Node)
    {
        Assert.IsNull(m_Child);
        m_Child = Node;

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
