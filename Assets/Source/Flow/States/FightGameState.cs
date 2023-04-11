using System;
using UnityEngine.Assertions;

public class FightGameState : GameState
{
    private static Type[] s_Levels = new Type[]
    {
        typeof(IntroLevel)
    };
    private Level m_CurrentLevel;

    private FightStage m_CurrentStage;
    private int m_CurrentStageIdx;

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

        int Level = PlayerState.Instance.Level;
        if (Level >= s_Levels.Length)
        {
            // @INCOMPLETE: What we'll do when player completes game?

            // @DEBUG
            PlayerState.Instance.Level = 0;
            NextLevel();
            return;
        }

        m_CurrentLevel = (Level)Activator.CreateInstance(s_Levels[Level]);
        Assert.IsNotNull(m_CurrentLevel.Stages);

        m_CurrentStageIdx = -1;
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
            // @INCOMPLETE: Call GameStateMachine to switch state; Show ad;

            // @DEBUG
            NextLevel();
            return;
        }

        m_EnemyBuffs = m_CurrentLevel.EnemyBuffs * m_CurrentLevel.Stages[m_CurrentStageIdx].EnemyBuffs;

        m_CurrentStage = SpawnInState<FightStage>(m_CurrentLevel.Stages[m_CurrentStageIdx].Stage);
        m_CurrentStage.name = m_CurrentStage.GetType().Name;
    }
}
