using System;
using UnityEngine;

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

    private static LevelInfo[] s_LevelsInfo = new LevelInfo[]
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

                        ProjectileSpeed = 10f,
                        ProjectileDamage = 10f
                    }
                }
            }
        }
    };

    private int m_CurrentLevel = 0;
    private int m_CurrentStage = 0;

    // I don't think that PlayerBuffs is good idea, because it's better to make a new ship
    private BuffMultipliers m_EnemyBuffs;
    public BuffMultipliers EnemyBuffs => m_EnemyBuffs;

    protected override void Start()
    {
        base.Start();

        m_EnemyBuffs =
            s_LevelsInfo[m_CurrentLevel].EnemyBuffs *
            s_LevelsInfo[m_CurrentLevel].Stages[m_CurrentStage].EnemyBuffs;

        var State = SpawnInState(s_LevelsInfo[m_CurrentLevel].Stages[m_CurrentStage].StageType);
        State.name = State.GetType().Name;
    }

    public void NextStage()
    {
        Debug.Log("Next Stage");
        // TODO: Start next stage

        /* TODO:
            Set new lvl to PlayerState
            Call GameStateMachine to switch state, show ad
        */
    }
}
