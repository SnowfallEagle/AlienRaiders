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
            TimeToNext = 1f,

            Config = new Spawner.Config
            {
                SpawnPattern = (int)AlienSpawner.Pattern.Single
            }
        },

        new SpawnerInfo
        {
            Type = typeof(AlienSpawner),
            TimeToNext = 1f,

            Config = new Spawner.Config
            {
                SpawnPattern = (int)AlienSpawner.Pattern.Triple
            }
        },
    };

    private int m_CurrentSpawner = -1;

    private void Start()
    {
        NextSpawner();
    }

    private void Update()
    {
        // TODO: Check if ships are destroyed
    }

    private void NextSpawner()
    {
        if (++m_CurrentSpawner >= s_SpawnersInfo.Length)
        {
            GameStateMachine.Instance.GetCurrentState<FightGameState>().NextStage();
            return;
        }

        SpawnerInfo Info = s_SpawnersInfo[m_CurrentSpawner];
        var Spawner = SpawnInState<Spawner>(Info.Type);
        Spawner.Spawn(Info.Config);
        // TODO: Spawner can return its Ships and we can track them instead of using timer

        if (Info.bWaitToEnd)
        {
            return;
        }

        TimerService.Instance.AddTimer(NextSpawner, Info.TimeToNext);
    }
}
