using UnityEngine;

// @NOTE: No player support yet
public class AutoRocketProjectile : Projectile
{
    [SerializeField] protected float m_UntargetedFlyTime = 1f;
    [SerializeField] protected float m_TargetDirectionModifier = 0.0075f;

    protected override void Start()
    {
        base.Start();

        m_BehaviorComponent.StartBehavior(new BHFlow_Sequence()
            .AddNode(new BHFlow_Sequence()
                .AddNode(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed)))
                .AddDecorator(new BHDecorator_TimeLimit(m_UntargetedFlyTime, false))
            )

            .AddNode(new BHFlow_Sequence()
                .AddNode(new BHTask_FollowTarget(PlayerState.Instance.PlayerShip, Speed, m_TargetDirectionModifier))
            )
        );
    }
}
