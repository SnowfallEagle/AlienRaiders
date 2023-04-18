using System;
using UnityEngine.Assertions;

public class FightGameState : GameState
{
    public const int AnyIdx = -1;

    public static Level[] s_Levels = new Level[]
    {
        new IntroLevel(),
        new DevLevel()
    };
    private Level m_CurrentLevel;
    private int m_SpecificLevelIdx;

    private FightStage m_CurrentStage;
    private int m_CurrentStageIdx;

    private int m_SpecificStageIdx;
    private int m_SpecificSpawnerIdx;

    private BuffMultipliers m_EnemyBuffs;
    public BuffMultipliers EnemyBuffs => m_EnemyBuffs;

    public FightGameState(int LevelIdx = AnyIdx, int StageIdx = AnyIdx, int SpawnerIdx = AnyIdx)
    {
        m_SpecificLevelIdx   = LevelIdx;
        m_SpecificStageIdx   = StageIdx;
        m_SpecificSpawnerIdx = SpawnerIdx;
    }

    public override void Start()
    {
        base.Start();

        UIService.Instance.Show<FightWidget>();

        // @FIXME: We should process input only after player arrived at destination...
        PlayerState.Instance.PlayerShip.bProcessInput = true;
        NextLevel();
    }

    public override void Update()
    {
        base.Update();

        m_CurrentStage?.Update();
    }

    public override void Exit()
    {
        base.Exit();

        UIService.Instance.Hide<FightWidget>();
        m_CurrentStage?.Exit();
    }

    public static int FindLevelIdxByName(string Name)
    {
        for (int i = 0; i < s_Levels.Length; ++i)
        {
            if (s_Levels[i].GetType().Name == Name)
            {
                return i;
            }
        }

        return AnyIdx;
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

        m_CurrentLevel = s_Levels[Level];
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
        m_CurrentStage?.Exit();

        if (++m_CurrentStageIdx >= m_CurrentLevel.Stages.Length)
        {
            ++PlayerState.Instance.Level;
            // @TODO: Call GameStateMachine to switch state; Show ad;

            // @DEBUG
            NextLevel();
            return;
        }

        m_EnemyBuffs = m_CurrentLevel.EnemyBuffs * m_CurrentLevel.Stages[m_CurrentStageIdx].EnemyBuffs;

        m_CurrentStage = (FightStage)Activator.CreateInstance(m_CurrentLevel.Stages[m_CurrentStageIdx].Stage);
        Assert.IsNotNull(m_CurrentStage);
        m_CurrentStage.Start(m_SpecificSpawnerIdx);
    }
}
