using UnityEngine;

// NOTE: No player support yet
public class AutoRocketProjectile : Projectile
{
    private Temp.BehaviorComponent m_TempBehaviorComponent;

    protected override void Start()
    {
        base.Start();

        m_TempBehaviorComponent = InitializeComponent<Temp.BehaviorComponent>();

        m_TempBehaviorComponent.StartBehavior(new BHTask_LoopCommand(new BHCommand_MoveForward(m_Speed))
            .AddOnNodeEnded((Task, Status) => { Debug.Log($"{ Task.GetType().Name } ended with status { Status }"); })
        );
    }
}
