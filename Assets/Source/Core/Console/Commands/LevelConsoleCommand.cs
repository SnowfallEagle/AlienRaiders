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

        // @TODO: Figure out how to deal with player
        PlayerState.Instance.PlayerShip.gameObject.SetActive(true);
        GameStateMachine.Instance.SwitchState(new FightGameState(LevelIdx, (int)Args[1], (int)Args[2]));
    }
}

