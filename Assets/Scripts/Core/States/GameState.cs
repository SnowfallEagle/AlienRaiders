using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : CustomBehavior
{
    [SerializeField] protected float CleanupTimeRate = 3f;
    [SerializeField] protected int InitialRefObjectsCapacity = 128;

    private List<GameObject> m_RefObjects;

    protected virtual void Start()
    {
        m_RefObjects = new List<GameObject>(InitialRefObjectsCapacity);
        ServiceLocator.Instance.Get<TimerService>().AddTimer(Cleanup, CleanupTimeRate, true, 0f);
    }

    protected virtual void Update()
    { }

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
