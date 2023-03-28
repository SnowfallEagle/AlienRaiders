using UnityEngine;

// TODO: Implement auto rocket support for player
public class AutoRocketProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();

        m_BehaviorComponent.AddTask(new BHAutoRocketTask_MoveVertical());
    }
}
