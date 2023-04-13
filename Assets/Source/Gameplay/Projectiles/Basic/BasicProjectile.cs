using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();

        BehaviorComponent.StartBehavior(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed)));
    }
}
