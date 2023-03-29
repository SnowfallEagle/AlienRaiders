namespace Temp
{

public class BehaviorComponent : CustomBehavior
{
    public BHRootNode RootNode = new BHRootNode();

    /** Must be called by owner
    */
    public void Initialize()
    {
        RootNode.Start(this, null);
    }

    private void LateUpdate()
    {
        if (RootNode.bActive)
        {
            RootNode.Update();
        }
    }
}

}