using System.Collections.Generic;
using UnityEngine.Assertions;

public class BHFlow_Sequence : BHFlowNode
{
    protected override ChildHandle GetNextChildHandle(ChildHandle CurrentChild, NodeStatus LastChildStatus)
    {
        ChildHandle Result = ChildHandle.Done;

        if (CurrentChild == ChildHandle.NotInitialized)
        {
            return (ChildHandle)0;
        }
        else if (LastChildStatus == NodeStatus.Done && ((int)CurrentChild + 1) % m_NumChildren > 0)
        {
            Result = CurrentChild + 1;
        }

        return Result;
    }
}