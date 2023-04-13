using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : CustomBehavior
{
    [SerializeField] protected float CleanupTimeRate = 3f;
    [SerializeField] protected int InitialRefObjectsCapacity = 128;

    private List<GameObject> m_RefObjects;
    private TimerService.Handle m_hCleanupTimer = new TimerService.Handle();

    protected virtual void Start()
    {
        m_RefObjects = new List<GameObject>(InitialRefObjectsCapacity);
        TimerService.Instance.AddTimer(m_hCleanupTimer, this, Cleanup, CleanupTimeRate, true, 0f);
    }

    private void OnDestroy()
    {
        foreach (var Object in m_RefObjects)
        {
            if (Object)
            {
                Destroy(Object);
            }
        }

        m_RefObjects.Clear();
    }

    private void Cleanup()
    {
        m_RefObjects.RemoveAll(Object => !Object);
    }

    public void ReferenceObject(MonoBehaviour Object)
    {
        if (Object)
        {
            m_RefObjects.Add(Object.gameObject);
        }
    }

    public void ReferenceObject(GameObject Object)
    {
        if (Object)
        {
            m_RefObjects.Add(Object);
        }
    }
}
