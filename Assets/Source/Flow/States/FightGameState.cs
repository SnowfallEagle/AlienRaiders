using System;
using UnityEngine;
using UnityEngine.Assertions;

public class FightGameState : GameState
{
    public const int AnyIdx = -1;

    public static Level[] s_Levels = new Level[]
    {
        new IntroLevel(),
        // new DevLevel()
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

        UIService.Instance.ShowWidget<FightWidget>();
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

        UIService.Instance.HideWidget<FightWidget>();
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
        const float DelayInTheMiddle = 1f;
        const float DelayAfterFlewAway = 2.5f;
        const float DelayBeforeFinishing = 1f;

        m_CurrentStage?.Exit();

        if (++m_CurrentStageIdx >= m_CurrentLevel.Stages.Length)
        {
            if (++PlayerState.Instance.Level >= s_Levels.Length)
            {
                PlayerState.Instance.Level = 0;
            }

            var PlayerShip = PlayerState.Instance.PlayerShip;
            { // @TODO: Put this stuff in method for player ship?
                PlayerShip.BehaviorComponent.ClearActions();
                TimerService.Instance.RemoveOwnerTimers(PlayerShip);

                PlayerShip.bProcessInput = false;
                PlayerShip.bCheckBounds  = false;
                PlayerShip.WeaponComponent.StopFire();
            }

            PlayerShip.HealthComponent.bGodMode = true;

            TimerService.Instance.AddTimer(null, PlayerShip, () =>
                {
                    // @TODO: Put in method for player ship
                    TimerService.Instance.RemoveOwnerTimers(PlayerShip);
                    PlayerShip.BehaviorComponent.ClearActions();

                    PlayerShip.BehaviorComponent.AddAction(new BHPlayerAction_CinematicMove(Vector3.zero, MaxAngle: 1f)
                        .AddOnActionFinished((_) =>
                        {
                            TimerService.Instance.AddTimer(null, PlayerShip, () =>
                                {
                                    PlayerShip.BehaviorComponent.AddAction(new BHPlayerAction_CinematicMove(new Vector3(0f, 50f), MaxAngle: 0f, Acceleration: 1f, MaxSpeed: 25f)
                                        .AddOnActionFinished((_) =>
                                        {
                                            TimerService.Instance.AddTimer(null, PlayerShip, () =>
                                                {
                                                    PlayerShip.HealthComponent.bGodMode = false;
                                                    UIService.Instance.ShowWidget<LevelEndedWidget>();
                                                },
                                                DelayAfterFlewAway
                                            );
                                        })
                                    );
                                },
                                DelayInTheMiddle
                            );
                        })
                    );
                },
                DelayBeforeFinishing
            );

            return;
        }

        m_EnemyBuffs = m_CurrentLevel.EnemyBuffs * m_CurrentLevel.Stages[m_CurrentStageIdx].EnemyBuffs;

        m_CurrentStage = (FightStage)Activator.CreateInstance(m_CurrentLevel.Stages[m_CurrentStageIdx].Stage);
        Assert.IsNotNull(m_CurrentStage);
        m_CurrentStage.Start(m_SpecificSpawnerIdx);
    }
}
