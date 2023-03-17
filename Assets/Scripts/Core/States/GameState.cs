using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : CustomBehaviour
{
    [SerializeField] protected float CleanupTimeRate = 3f;

    private static readonly int InitialRefObjectsCapacity = 128;
    private List<MonoBehaviour> m_RefObjects = new List<MonoBehaviour>(InitialRefObjectsCapacity);

    private void Start()
    {
        ServiceLocator.Instance.Get<TimerService>().AddTimer(Cleanup, CleanupTimeRate, true);
    }

    private void OnDestroy()
    {
        foreach (var Object in m_RefObjects)
        {
            if (Object)
            {
                Destroy(Object.gameObject);
            }
        }

        m_RefObjects.Clear();
    }

    private void Cleanup()
    {
        for (int i = m_RefObjects.Count - 1; i >= 0; --i)
        {
            if (!m_RefObjects[i])
            {
                m_RefObjects.RemoveAt(i);
            }
        }
    }

    public void ReferenceObject(MonoBehaviour Object)
    {
        if (Object)
        {
            m_RefObjects.Add(Object);
        }
    }
}
