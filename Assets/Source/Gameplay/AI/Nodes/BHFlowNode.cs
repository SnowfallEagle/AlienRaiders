using UnityEngine.Assertions;

public class BHFlowNode : BHNode
{
    /** NOTE:
        Derived flow nodes must call Start(), Stop(), Update() on their children and
        override AddNode() to add child
    */

    public virtual BHFlowNode AddNode(BHNode Node)
    {
        Assert.IsNotNull(Node);
        return this;
    }
}

