using UnityEngine;

// @NOTE: No player support yet
public class AutoRocketProjectile : Projectile
{
    private BehaviorComponent m_TempBehaviorComponent;

    protected override void Start()
    {
        base.Start();

        m_TempBehaviorComponent = InitializeComponent<BehaviorComponent>();

        m_TempBehaviorComponent.StartBehavior(new BHFlow_Sequence()
            .AddNode(new BHFlow_Sequence()
                .AddNode(new BHTask_LoopCommand(new BHCommand_MoveForward(m_Speed))
                    .AddOnNodeFinished((Task, Status) => { Debug.Log($"{ Task.GetType().Name } ended with status { Status }"); })
                )
                .AddDecorator(new BHDecorator_TimeLimit(1f, false))
            )

            .AddNode(new BHFlow_Sequence()
                .AddNode(new BHTask_FollowTarget(PlayerState.Instance.PlayerShip))
            )
        );
    }
}
