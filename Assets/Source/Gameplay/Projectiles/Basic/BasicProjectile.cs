using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();

        float Dot = Vector3.Dot(Vector3.up, m_Owner.transform.up);
        float Modifier = Dot < 0f ? -1f : 1f;

        m_BehaviorComponent.StartBehavior(new BHTask_LoopCommand(
            new BHCommand_RelativeMove(m_Owner.transform.up * (m_Speed * Modifier * Time.deltaTime))
        ));
    }
}
