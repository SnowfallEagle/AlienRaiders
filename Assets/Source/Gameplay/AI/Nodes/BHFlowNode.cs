using UnityEngine.Assertions;

public class BHFlowNode : BHNode
{
    public virtual BHFlowNode AddNode(BHNode Node)
    {
        Assert.IsNotNull(Node);
        return this;
    }
}

