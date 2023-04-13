using UnityEngine;

public class LevelConsoleCommand : ConsoleCommand
{
    public LevelConsoleCommand()
    {
        ArgumentsInfo = new ArgumentInfo[]
        {
            new ArgumentInfo { Name = "name",    Type = typeof(string), Default = "IntroLevel" },
            new ArgumentInfo { Name = "stage",   Type = typeof(int),    Default = 0 },
            new ArgumentInfo { Name = "spawner", Type = typeof(int),    Default = 0 },
        };
    }

    public override void Execute(object[] Args)
    {
        var Object = new GameObject();
        var FightState = Object.AddComponent<FightGameState>();

        string LevelName = Args[0].ToString();
        int LevelIdx = FightGameState.AnyIdx;

        for (int i = 0; i < FightGameState.s_Levels.Length; ++i)
        {
            if (LevelName == FightGameState.s_Levels[i].Name)
            {
                LevelIdx = i;
                break;
            }
        }

        FightState.Initialize(LevelIdx, (int)Args[1], (int)Args[2]);
        GameStateMachine.Instance.SwitchState(FightState);
    }
}

