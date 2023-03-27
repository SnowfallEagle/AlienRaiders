using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviorComponent : CustomBehavior
{
    private Ship m_Owner;

    private List<BHTask> m_TaskList = new List<BHTask>();

    private void Start()
    {
        m_Owner = GetComponent<Ship>();
    }

    private void LateUpdate()
    {
        UpdateTasks();
    }

    public void AddTask(BHTask Task)
    {
        Task.Start(m_Owner);
        if (!Task.bEnded)
        {
            m_TaskList.Add(Task);
        }
    }

    private void UpdateTasks()
    {
        for (int i = m_TaskList.Count - 1; i >= 0; --i)
        {
            m_TaskList[i].Update(m_Owner);
        }

        m_TaskList.RemoveAll(Task => Task.bEnded);
    }
}
