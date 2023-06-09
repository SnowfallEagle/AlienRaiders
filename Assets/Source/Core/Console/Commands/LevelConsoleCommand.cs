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

        GameStateMachine.Instance.SwitchState(new FightGameState(LevelIdx, (int)Args[1], (int)Args[2], true));
        RenderingService.Instance.UpdateAppearance(LevelIdx);
        PlayerState.Instance.PlayerShip.Revive();
    }
}

