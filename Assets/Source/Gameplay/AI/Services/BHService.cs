using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BHService : BHActionNode
{
    /** Dervived services shouldn't call Finish() in Update() as Tasks,
        but they can override this method to hook on this event.
    */
    public override void Finish(NodeStatus FinishStatus)
    {
        if (!bActive && FinishStatus == NodeStatus.InProgress)
        {
            Assert.IsTrue(false, "Child wanted to finish with being not activated or InProgress status!");
            return;
        }

        // @NOTE: We don't bother parent Flow Node

        m_OnNodeFinished?.Invoke(this, FinishStatus);
    }
}
