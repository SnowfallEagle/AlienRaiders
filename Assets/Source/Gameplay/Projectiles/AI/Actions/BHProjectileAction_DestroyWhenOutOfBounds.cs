using UnityEngine;

public class BHProjectileAction_DestroyWhenOutOfBounds : BHAction
{
    private Vector3 m_TargetSize;

    public override bool Start()
    {
        m_TargetSize = RenderingService.Instance.TargetSize * 0.75f;        
        return true;
    }

    public override bool Update()
    {
        if (!IsInOfBounds())
        {
            CustomBehavior.Destroy(m_Owner.gameObject);
        }
        return true;
    }

    private bool IsInOfBounds()
    {
        Vector3 Position = m_Owner.transform.position;
        return
            Position.x > -m_TargetSize.x && Position.x < m_TargetSize.x &&
            Position.y > -m_TargetSize.y && Position.y < m_TargetSize.y;
    }
}
