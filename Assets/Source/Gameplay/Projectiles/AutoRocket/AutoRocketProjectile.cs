using UnityEngine;

public class AutoRocketProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();

        // TODO: We need to have stuff like AddTask(new Task(), OnTaskDoneAction)
        m_BehaviorComponent.AddTask(new BHAutoRocketTask_MoveVertical());
    }
}
