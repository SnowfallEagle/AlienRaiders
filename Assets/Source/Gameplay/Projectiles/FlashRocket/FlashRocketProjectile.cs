using UnityEngine;

public class FlashRocketProjectile : Projectile
{
    [SerializeField] protected float m_SlowFlyTime = 2.5f;
    [SerializeField] protected float m_FastSpeedModifier = 4f;

    protected override void Start()
    {
        base.Start();

        BehaviorComponent.StartBehavior(new BHFlow_Sequence()
            .AddNode(new BHFlow_Sequence()
                .AddNode(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed)))
                .AddDecorator(new BHDecorator_TimeLimit(m_SlowFlyTime, false))
            )

            .AddNode(new BHFlow_Sequence()
                .AddNode(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed * m_FastSpeedModifier)))
            )
        );
    }
}
