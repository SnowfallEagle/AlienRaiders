using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHFlow_Selector : BHFlowNode
{
    protected override ChildHandle GetNextChildHandle(ChildHandle CurrentChild, NodeStatus LastChildStatus)
    {
        if (CurrentChild == ChildHandle.NotInitialized)
        {
            return (ChildHandle)0;
        }
        else if (LastChildStatus == NodeStatus.Failed && ((int)CurrentChild + 1) % m_NumChildren > 0)
        {
            return CurrentChild + 1;
        }

        return ChildHandle.Done;
    }
}
