using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    [SerializeField] protected float CleanupTimeRate = 3f;
    [SerializeField] protected int InitialRefObjectsCapacity = 128;

    private List<GameObject> m_RefObjects;
    private TimerService.Handle m_hCleanupTimer = new TimerService.Handle();

    public virtual void Start()
    {
        m_RefObjects = new List<GameObject>(InitialRefObjectsCapacity);
        TimerService.Instance.AddTimer(m_hCleanupTimer, null, Cleanup, CleanupTimeRate, true, 0f);
    }

    public virtual void Update()
    { }

    public virtual void Exit()
    {
        m_hCleanupTimer.Invalidate();

        foreach (var RefObject in m_RefObjects)
        {
            if (RefObject)
            {
                CustomBehavior.Destroy(RefObject);
            }
        }

        m_RefObjects.Clear();
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

    private void Cleanup()
    {
        m_RefObjects.RemoveAll(Object => !Object);
    }
}
