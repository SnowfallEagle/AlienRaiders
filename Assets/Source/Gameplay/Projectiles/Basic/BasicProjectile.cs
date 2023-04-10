using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();

        m_BehaviorComponent.StartBehavior(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed)));
    }
}
