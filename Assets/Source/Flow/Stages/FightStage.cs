using System;
using UnityEngine;
using UnityEngine.Assertions;

/** Fight stages should set m_Spawners in their constructor */
public abstract class FightStage : CustomBehavior
{
    protected class SpawnerInfo
    {
        public Type Type = typeof(AlienSpawner);
        public Spawner.Config Config = new Spawner.Config();

        /** If bWaitToEnd = true then this shows delay after destroying ships */
        public float TimeToNext = 0f;
        public bool bWaitToEnd = false;
        public int Iterations = 1;
    }

    protected SpawnerInfo[] m_Spawners;
    private SpawnerInfo m_CurrentSpawnerInfo;
    private int m_CurrentSpawnerIdx = -1;

    private Spawner m_CurrentSpawner;
    private GameObject[] m_CurrentShips;

    private TimerService.Handle m_hIterationTimer = new TimerService.Handle();

    private void Start()
    {
        // @TODO: Implement DebugLevel

        Assert.IsNotNull(m_Spawners);
        NextSpawner();
    }

    private void Update()
    {
        if (m_CurrentSpawnerInfo == null || !m_CurrentSpawnerInfo.bWaitToEnd)
        {
            return;
        }

        foreach (var Ship in m_CurrentShips)
        {
            if (Ship)
            {
                return;
            }
        }

        TimerService.Instance.AddTimer(m_hIterationTimer, this, NextIteration, m_CurrentSpawnerInfo.TimeToNext);
    }

    private void NextIteration()
    {
        if (--m_CurrentSpawnerInfo.Iterations < 0)
        {
            Destroy(m_CurrentSpawner.gameObject);
            NextSpawner();
            return;
        }

        m_CurrentShips = m_CurrentSpawner.Spawn(m_CurrentSpawnerInfo.Config);

        if (!m_CurrentSpawnerInfo.bWaitToEnd)
        {
            TimerService.Instance.AddTimer(m_hIterationTimer, this, NextIteration, m_CurrentSpawnerInfo.TimeToNext);
        }
    }

    private void NextSpawner()
    {
        if (++m_CurrentSpawnerIdx >= m_Spawners.Length)
        {
            GameStateMachine.Instance.GetCurrentState<FightGameState>().NextStage();
            return;
        }

        m_CurrentSpawnerInfo = m_Spawners[m_CurrentSpawnerIdx];
        m_CurrentSpawner = SpawnInState<Spawner>(m_CurrentSpawnerInfo.Type);
        m_CurrentSpawner.name = m_CurrentSpawner.GetType().Name;

        NextIteration();
    }
}
