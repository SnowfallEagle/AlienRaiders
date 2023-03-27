using System;
using UnityEngine;

public class FightStage : CustomBehavior
{
    private class SpawnerInfo
    {
        public Type Type = typeof(AlienSpawner);
        public Spawner.Config Config = new Spawner.Config();

        public float TimeToNext = 5f; // If bWaitToEnd = true then this shows delay after destroying ships
        public bool bWaitToEnd = false;

        // TODO: Iterations count?
    }

    private SpawnerInfo[] s_SpawnersInfo = new SpawnerInfo[]
    {
        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            Config = new Spawner.Config
            {
                SpawnPattern = (int)AlienSpawner.Pattern.Single,
                ShipColor = Color.red,
            },

            bWaitToEnd = true,
            TimeToNext = 2.5f
        },

        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            Config = new Spawner.Config
            {
                SpawnPattern = (int)AlienSpawner.Pattern.Triple,
                ShipColor = Color.blue
            },

            TimeToNext = 1f,
        },
    };

    private int m_CurrentSpawnerIndex = -1;
    private SpawnerInfo m_CurrentSpawnerInfo;

    private GameObject[] m_CurrentShips;

    private void Start()
    {
        NextSpawner();
    }

    private void Update()
    {
        if (!m_CurrentSpawnerInfo.bWaitToEnd)
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

        TimerService.Instance.AddTimer(NextSpawner, m_CurrentSpawnerInfo.TimeToNext);
    }

    private void NextSpawner()
    {
        if (++m_CurrentSpawnerIndex >= s_SpawnersInfo.Length)
        {
            GameStateMachine.Instance.GetCurrentState<FightGameState>().NextStage();
            return;
        }

        m_CurrentSpawnerInfo = s_SpawnersInfo[m_CurrentSpawnerIndex];
        var Spawner = SpawnInState<Spawner>(m_CurrentSpawnerInfo.Type);
        m_CurrentShips = Spawner.Spawn(m_CurrentSpawnerInfo.Config);

        if (m_CurrentSpawnerInfo.bWaitToEnd)
        {
            return;
        }

        TimerService.Instance.AddTimer(NextSpawner, m_CurrentSpawnerInfo.TimeToNext);
    }
}
