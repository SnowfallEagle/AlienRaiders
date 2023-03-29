using UnityEngine;

// TODO: Player support
public class AutoRocketProjectile : Projectile
{
    private Temp.BehaviorComponent m_TempBehaviorComponent;

    protected override void Start()
    {
        base.Start();

        m_TempBehaviorComponent = InitializeComponent<Temp.BehaviorComponent>();
        m_TempBehaviorComponent.Initialize();

        // TODO: Add check for nodes like AnyActiveNodes() to Restart() them
        // TODO: Remove manual restart from Root and Sequence
        m_TempBehaviorComponent.RootNode.AddNode(new BHSequenceNode()
            .AddNode(new BHNode()
                .AddTask(new BHTask_LoopCommand(new BHCommand_MoveForward(m_Speed * 2f))
                    .AddOnNodeEnded((_) => { })
                )
                .AddTask(new BHTask_SetNodeLifeTime(1f))
            )

            .AddNode(new BHNode()
                .AddTask(new BHTask_LoopCommand(new BHCommand_MoveForward(-m_Speed)))
                .AddTask(new BHTask_SetNodeLifeTime(0.5f))
            )
        );

        /*
        m_TempBehaviorComponent.RootNode.AddNode(new BHSequenceNode()
            .AddNode(new BHNode()
                .AddTask(new BHTask_MoveVertical(-5f)
                    .AddOnEndedCallback(Task => { Debug.Log("Ended"); })
                )
                .AddTask(new BHTask_LimitNodeTime(1f))
            )

            .AddNode(new BHNode()
                .AddTask(new BHTask_FollowTarget(PlayerState.Instance.PlayerShip))
            )
        );
        */
    }
}
