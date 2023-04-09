using UnityEngine.Assertions;

namespace Temp
{

    public class BehaviorComponent : CustomBehavior
    {
        BHRootNode Root = new BHRootNode();

        private void LateUpdate()
        {
            if (Root.bActive)
            {
                Root.Update();
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

            Root.AddNode(Node);

            Root.Initialize(this, null);
            Root.Start();
            Root.bActive = true;
        }

        public void StopBehavior()
        {
            if (Root.bActive)
            {
                Root.Finish(BHNode.NodeStatus.Done);
                Root.bActive = false;
            }
        }
    }

}
