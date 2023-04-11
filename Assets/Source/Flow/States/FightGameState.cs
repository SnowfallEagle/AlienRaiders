using System;

public class FightGameState : GameState
{
    private class StageInfo
    {
        public Type StageType = typeof(FightStage);
        public BuffMultipliers EnemyBuffs = new BuffMultipliers();
    }

    private class LevelInfo
    {
        public StageInfo[] Stages = new StageInfo[] { };
        public BuffMultipliers EnemyBuffs = new BuffMultipliers();
    }

    private static LevelInfo[] s_Levels = new LevelInfo[]
    {
        new LevelInfo
        {
            Stages = new StageInfo[]
            {
                new StageInfo
                {
                    StageType = typeof(FightStage),
                    EnemyBuffs = new BuffMultipliers
                    {
                        ShipHealth = 0.1f,

                        ProjectileDamage = 10f
                    }
                }
            }
        }
    };
    private int m_CurrentLevelIdx;

    private int m_CurrentStageIdx;
    private FightStage m_CurrentStage;

    private BuffMultipliers m_EnemyBuffs;
    public BuffMultipliers EnemyBuffs => m_EnemyBuffs;

    protected override void Start()
    {
        base.Start();

        NextLevel();
    }

    private void NextLevel()
    {
        // @TODO: Implement DebugLevel
        m_CurrentLevelIdx = PlayerState.Instance.Level;
        if (m_CurrentLevelIdx >= s_Levels.Length)
        {
            // @INCOMPLETE: What we'll do when player completes game?

            // @DEBUG
            PlayerState.Instance.Level = 0;
            NextLevel();
            return;
        }

        m_CurrentStageIdx = -1;
        NextStage();
    }

    public void NextStage()
    {
        if (m_CurrentStage)
        {
            Destroy(m_CurrentStage.gameObject);
        }

        if (++m_CurrentStageIdx >= s_Levels[m_CurrentLevelIdx].Stages.Length)
        {
            ++PlayerState.Instance.Level;
            // @INCOMPLETE: Call GameStateMachine to switch state; Show ad;

            // @DEBUG
            NextLevel();
            return;
        }

        m_EnemyBuffs =
            s_Levels[m_CurrentLevelIdx].EnemyBuffs *
            s_Levels[m_CurrentLevelIdx].Stages[m_CurrentStageIdx].EnemyBuffs;

        m_CurrentStage = SpawnInState<FightStage>(s_Levels[m_CurrentLevelIdx].Stages[m_CurrentStageIdx].StageType);
        m_CurrentStage.name = m_CurrentStage.GetType().Name;
    }
}
