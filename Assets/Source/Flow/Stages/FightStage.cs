using System;
using UnityEngine;
using UnityEngine.Assertions;

public class FightStage : CustomBehavior
{
    private class SpawnerInfo
    {
        public Type Type = typeof(AlienSpawner);
        public Spawner.Config Config = new Spawner.Config();

        /** If bWaitToEnd = true then this shows delay after destroying ships */
        public float TimeToNext = 0f;
        public bool bWaitToEnd = false;
        public int Iterations = 1;
    }

    private SpawnerInfo[] s_SpawnersInfo = new SpawnerInfo[]
    {
        new SpawnerInfo
        {
            Type = typeof(OneBigTwoNearSpawner),
            bWaitToEnd = true,
        },

        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            Config = new Spawner.Config
            {
                SpecificSpawnPattern = (int)AlienSpawner.Pattern.Triple,
                SpecificSpawnSubpattern = (int)AlienSpawner.TripleSubpattern.Column,
                ShipColor = Color.magenta,
            },

            bWaitToEnd = true,
        },

        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            Config = new Spawner.Config
            {
                FromSpawnPattern = (int)AlienSpawner.Pattern.Single,
                ToSpawnPattern = (int)AlienSpawner.Pattern.Triple,
                ShipColor = Color.red,
            },

            bWaitToEnd = true,

            Iterations = 2
        },

        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            Config = new Spawner.Config
            {
                SpecificSpawnPattern = (int)AlienSpawner.Pattern.Triple,
                ShipColor = Color.blue
            },

            TimeToNext = 1f,
        },
    };

    private int m_CurrentSpawnerIdx = -1;
    private SpawnerInfo m_CurrentSpawnerInfo;

    private GameObject[] m_CurrentShips;
    private Spawner m_CurrentSpawner;

    private TimerService.Handle m_hIterationTimer = new TimerService.Handle();

    private void Start()
    {
        // @TODO: Implement DebugLevel

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
        if (++m_CurrentSpawnerIdx >= s_SpawnersInfo.Length)
        {
            GameStateMachine.Instance.GetCurrentState<FightGameState>().NextStage();
            return;
        }

        m_CurrentSpawnerInfo = s_SpawnersInfo[m_CurrentSpawnerIdx];
        m_CurrentSpawner = SpawnInState<Spawner>(m_CurrentSpawnerInfo.Type);

        NextIteration();
    }
}
