using System;
using UnityEngine;
using UnityEngine.Assertions;

public class FightGameState : GameState
{
    public const int AnyIdx = -1;

    private static Type[] s_Levels = new Type[]
    {
        typeof(IntroLevel)
    };
    private Level m_CurrentLevel;
    private int m_SpecificLevelIdx;

    private FightStage m_CurrentStage;
    private int m_CurrentStageIdx;

    private int m_SpecificStageIdx;
    private int m_SpecificSpawnerIdx;

    private BuffMultipliers m_EnemyBuffs;
    public BuffMultipliers EnemyBuffs => m_EnemyBuffs;

    public void Initialize(int LevelIdx = AnyIdx, int StageIdx = AnyIdx, int SpawnerIdx = AnyIdx)
    {
        m_SpecificLevelIdx   = LevelIdx   == AnyIdx ? AnyIdx : LevelIdx;
        m_SpecificStageIdx   = StageIdx   == AnyIdx ? AnyIdx : StageIdx;
        m_SpecificSpawnerIdx = SpawnerIdx == AnyIdx ? AnyIdx : SpawnerIdx;
    }

    protected override void Start()
    {
        base.Start();

        // @TODO: Figure out how to do it better
        SpawnInState(Resources.Load<GameObject>("States/FightScene"));

        PlayerState.Instance.SpawnShip();

        NextLevel();
    }

    private void NextLevel()
    {
        int Level;
        if (m_SpecificLevelIdx == AnyIdx)
        {
            Level = PlayerState.Instance.Level;
        }
        else
        {
            Level = m_SpecificLevelIdx;
            m_SpecificLevelIdx = AnyIdx;
        }

        if (Level >= s_Levels.Length)
        {
            // @TODO: What we'll do when player completes game?

            // @DEBUG
            PlayerState.Instance.Level = 0;
            NextLevel();
            return;
        }

        m_CurrentLevel = (Level)Activator.CreateInstance(s_Levels[Level]);
        Assert.IsNotNull(m_CurrentLevel.Stages);

        if (m_SpecificStageIdx == AnyIdx)
        {
            m_CurrentStageIdx = -1;
        }
        else
        {
            m_CurrentStageIdx = m_SpecificStageIdx - 1;
            m_SpecificStageIdx = AnyIdx;
        }
        NextStage();
    }

    public void NextStage()
    {
        if (m_CurrentStage)
        {
            Destroy(m_CurrentStage.gameObject);
        }

        if (++m_CurrentStageIdx >= m_CurrentLevel.Stages.Length)
        {
            ++PlayerState.Instance.Level;
            // @TODO: Call GameStateMachine to switch state; Show ad;

            // @DEBUG
            NextLevel();
            return;
        }

        m_EnemyBuffs = m_CurrentLevel.EnemyBuffs * m_CurrentLevel.Stages[m_CurrentStageIdx].EnemyBuffs;

        m_CurrentStage = SpawnInState<FightStage>(m_CurrentLevel.Stages[m_CurrentStageIdx].Stage);
        m_CurrentStage.Initialize(m_SpecificSpawnerIdx);
        m_CurrentStage.name = m_CurrentStage.GetType().Name;
    }
}
