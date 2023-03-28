public class BHShipTask_DestroyWhenOutOfBottomBound : BHTask
{
    private float YBound;

    public override void Start()
    {
        var Renderer = RenderingService.Instance;
        YBound = Renderer.TargetCenter.y - (Renderer.TargetSize.y * 0.6f);
    }

    public override void Update()
    {
        if (m_Owner.transform.position.y < YBound)
        {
            m_Owner.GetComponent<ShipHealthComponent>().Kill();
            State = TaskState.Done;
        }
    }
}
