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
        int LevelIdx = FightGameState.FindLevelIdxByName(Args[0].ToString());

        // @TODO: Figure out how to deal with player
        PlayerState.Instance.PlayerShip.gameObject.SetActive(true);
        GameStateMachine.Instance.SwitchState(new FightGameState(LevelIdx, (int)Args[1], (int)Args[2]));
    }
}

