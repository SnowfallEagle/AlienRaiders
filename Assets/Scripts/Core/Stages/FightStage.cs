using System;
using UnityEngine;

public class FightStage : CustomBehavior
{
    private class SpawnerInfo
    {
        public Type Type = typeof(AlienSpawner);
        public float TimeToNext = 5f;
        public bool bWaitToEnd = false;

        public Spawner.Config Config = new Spawner.Config();

        // TODO: Iterations count?
    }

    private SpawnerInfo[] s_SpawnersInfo = new SpawnerInfo[]
    {
        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            bWaitToEnd = true,

            Config = new Spawner.Config
            {
                SpawnPattern = (int)AlienSpawner.Pattern.Single,
                ShipColor = Color.red
            }
        },

        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            TimeToNext = 1f,

            Config = new Spawner.Config
            {
                SpawnPattern = (int)AlienSpawner.Pattern.Triple,
                ShipColor = Color.blue
            }
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

        NextSpawner();
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
