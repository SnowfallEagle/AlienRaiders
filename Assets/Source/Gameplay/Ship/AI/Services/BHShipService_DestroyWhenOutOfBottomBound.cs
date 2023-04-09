public class BHShipService_DestroyWhenOutOfBottomBound : BHService
{
    private float m_YBound;

    public override NodeStatus Start()
    {
        var Renderer = RenderingService.Instance;
        m_YBound = Renderer.TargetCenter.y - (Renderer.TargetSize.y * 0.6f);
        return NodeStatus.InProgress;
    }

    public override void Update()
    {
        if (m_Owner.transform.position.y < m_YBound)
        {
            m_Owner.GetComponent<ShipHealthComponent>().Kill();
        }
    }
}
