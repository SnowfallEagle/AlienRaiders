using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();

        float Dot = Vector3.Dot(Vector3.up, m_Owner.transform.up);
        float Modifier = Dot < 0f ? -1f : 1f;

        m_BehaviorComponent.AddTask(new BHTask_MoveVertical(m_Speed * Modifier));
    }
}
