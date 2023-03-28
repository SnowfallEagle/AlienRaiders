using UnityEngine;

public class BHAutoRocketTask_MoveVertical : BHTask
{
    public override void Start()
    {
        float Speed = m_Owner.GetComponent<AutoRocketProjectile>().Speed;
        if (Vector3.Dot(Vector3.up, m_Owner.transform.up) < 0f)
        {
            Speed = -Speed;
        }

        BHTask InternalMoveTask = new BHTask_MoveVertical(Speed);
        m_Owner.AddTask(InternalMoveTask);

        TimerService.Instance.AddTimer(null, m_Owner, () =>
            {
                State = TaskState.Done;
                InternalMoveTask.State = TaskState.Done;
                // TODO: m_Owner.AddTask(new BHAutoRocketTask_FollowPlayer());
            },
            1f
        );
    }
}
