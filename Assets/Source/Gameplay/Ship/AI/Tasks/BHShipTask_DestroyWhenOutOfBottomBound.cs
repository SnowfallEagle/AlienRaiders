// @INCOMPLETE: It's a service
public class BHShipTask_DestroyWhenOutOfBottomBound : BHTaskNode
{
    private float YBound;

    public override NodeStatus Start()
    {
        var Renderer = RenderingService.Instance;
        YBound = Renderer.TargetCenter.y - (Renderer.TargetSize.y * 0.6f);
        return NodeStatus.InProgress;
    }

    public override void Update()
    {
        if (m_Owner.transform.position.y < YBound)
        {
            m_Owner.GetComponent<ShipHealthComponent>().Kill();
            Finish(NodeStatus.Done);
        }
    }
}
