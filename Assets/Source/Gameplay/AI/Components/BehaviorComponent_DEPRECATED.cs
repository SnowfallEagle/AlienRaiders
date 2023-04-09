using System.Collections.Generic;
using UnityEngine;

public class BehaviorComponent : CustomBehavior
{
    private List<BHTask> m_TaskList = new List<BHTask>();
    private BHNode m_RootNode = new BHFlow_Root();

    private void Start()
    {
    }

    private void LateUpdate()
    {
        UpdateTasks();
    }

    /** Add task that'll start immediately and update every frame
        Callback'll be called after task ended
    */
    public void AddTask(BHTask Task, BHTask.OnEndedSignature Callback = null)
    {
        Task.Initialize(this, m_RootNode);
        if (Callback != null)
        {
            Task.OnEnded += Callback;
        }

        Task.Start();
        if (Task.bEnded)
        {
            Task.OnEnded?.Invoke(Task);
        }
        else
        {
            m_TaskList.Add(Task);
        }
    }

    public void AddTask(BHTask Task, BHTask NextTask)
    {
        AddTask(Task, _ => AddTask(NextTask));
    }

    private void UpdateTasks()
    {
        for (int i = m_TaskList.Count - 1; i >= 0; --i)
        {
            m_TaskList[i].Update();
        }

        m_TaskList.RemoveAll(Task =>
        {
            if (Task.bEnded)
            {
                Task.OnEnded?.Invoke(Task);
                return true;
            }
            return false;
        });
    }
}
