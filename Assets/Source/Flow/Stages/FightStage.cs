using System;
using UnityEngine;
using UnityEngine.Assertions;

/** Fight stages should set m_Spawners in their constructor */
public abstract class FightStage
{
    public const int AnyIdx = -1;

    protected class SpawnerInfo
    {
        /** Just wait timer if null */
        public Type Spawner;
        public SpawnerConfig Config = new SpawnerConfig();

        /** Resource path of pickup
            Only for first iteration
        */
        public string Pickup;

        /** If bWaitToEnd = true then this shows delay after destroying ships */
        public float TimeToNext = 0f;
        public bool bWaitToEnd = false;
        public int Iterations = 1;
    }

    protected SpawnerInfo[] m_Spawners;
    private SpawnerInfo m_CurrentSpawnerInfo;

    private int m_CurrentSpawnerIdx;
    private Spawner m_CurrentSpawner;
    private GameObject[] m_CurrentShips;

    private bool m_bWaitingEnded;
    private TimerService.Handle m_hIterationTimer = new TimerService.Handle();

    public void Start(int SpecificSpawnerIdx = AnyIdx)
    {
        Assert.IsNotNull(m_Spawners);

        // Force wait to end last spawner
        int SpawnersLength = m_Spawners.Length;
        if (SpawnersLength > 0)
        {
            m_Spawners[SpawnersLength - 1].bWaitToEnd = true;
        }

        m_CurrentSpawnerIdx = SpecificSpawnerIdx == AnyIdx ? -1 : SpecificSpawnerIdx - 1;
        NextSpawner();
    }

    public void Update()
    {
        if (m_CurrentSpawnerInfo == null || !m_CurrentSpawnerInfo.bWaitToEnd || m_bWaitingEnded)
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

        TimerService.Instance.AddTimer(m_hIterationTimer, null, NextIteration, m_CurrentSpawnerInfo.TimeToNext);
        m_bWaitingEnded = true;
    }

    public void Exit()
    {
        m_hIterationTimer.Invalidate();
    }

    private void NextIteration()
    {
        if (--m_CurrentSpawnerInfo.Iterations < 0)
        {
            if (m_CurrentSpawner)
            {
                CustomBehavior.Destroy(m_CurrentSpawner.gameObject);
            }
            NextSpawner();
            return;
        }

        m_CurrentShips = m_CurrentSpawner ? m_CurrentSpawner.Spawn(m_CurrentSpawnerInfo.Config) : null;
        m_bWaitingEnded = false;

        if (!m_CurrentSpawnerInfo.bWaitToEnd)
        {
            if (m_CurrentSpawnerInfo.TimeToNext > 0f)
            {
                TimerService.Instance.AddTimer(m_hIterationTimer, null, NextIteration, m_CurrentSpawnerInfo.TimeToNext);
            }
            else
            {
                NextIteration();
            }
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
        if (m_CurrentSpawnerInfo.Spawner != null)
        {
            m_CurrentSpawner = CustomBehavior.SpawnInState<Spawner>(m_CurrentSpawnerInfo.Spawner);
            m_CurrentSpawner.name = m_CurrentSpawner.GetType().Name;
        }
        else
        {
            m_CurrentSpawner = null;
        }

        if (m_CurrentSpawnerInfo.Pickup != null)
        {
            var Pickup = CustomBehavior.SpawnInState(Resources.Load<GameObject>(m_CurrentSpawnerInfo.Pickup));

            var SpriteRenderer = Pickup.GetComponent<SpriteRenderer>();
            float PickupHalfSizeX = SpriteRenderer.bounds.size.x * 0.5f;
            Vector3 TargetSize = RenderingService.Instance.TargetSize;

            Vector3 PickupSpawnPosition = RenderingService.Instance.TargetCenter;
            PickupSpawnPosition.y += TargetSize.y;
            PickupSpawnPosition.x += UnityEngine.Random.Range(PickupHalfSizeX, TargetSize.x - PickupHalfSizeX) - (TargetSize.x * 0.5f);

            Pickup.transform.position = PickupSpawnPosition;
        }

        NextIteration();
    }
}
