using System;

public class FightGameState : GameState
{
    private class LevelInfo
    {
        public Type StageType = typeof(FightStage);
    }

    private static LevelInfo[] s_LevelsInfo = new LevelInfo[]
    {
        new LevelInfo
        {
            StageType = typeof(FightStage)
        }
    };

    protected override void Start()
    {
        base.Start();

        var State = SpawnInState(s_LevelsInfo[0].StageType);
        State.name = State.GetType().Name;
    }

    protected override void Update()
    {
        base.Update();

        /* TODO:
            Track when we're done with this lvl
            Set new lvl to PlayerState
            Call GameStateMachine to switch state
        */
    }
}
