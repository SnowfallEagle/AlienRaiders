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

        /** Node must be BHTaskNode or BHFlowNode
        */
        public void StartBehavior(BHActionNode Node = null)
        {
            StopBehavior();

            if (Node != null)
            {
                Root.AddNode(Node);
            }

            Root.Initialize(this, null);
            Root.Start();
            Root.bActive = true;
        }

        public void StopBehavior()
        {
            // TODO: We need to find way to finish without restarting, maybe make bool bRestart in RootNode
            if (Root.bActive)
            {
                Root.Finish(BHNode.NodeStatus.Done);
                Root.bActive = false;
            }
        }
    }

}
