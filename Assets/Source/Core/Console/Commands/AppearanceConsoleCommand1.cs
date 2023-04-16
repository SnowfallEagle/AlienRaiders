using UnityEngine;

public class AppearanceConsoleCommand : ConsoleCommand
{
    public AppearanceConsoleCommand()
    {
        ArgumentsInfo = new ArgumentInfo[]
        {
            new ArgumentInfo { Name = "name", Type = typeof(string), Default = "IntroLevel" },
        };
    }

    public override void Execute(object[] Args)
    {
        int LevelIdx = FightGameState.FindLevelIdxByName(Args[0].ToString());
        RenderingService.Instance.UpdateAppearance(LevelIdx);
    }
}

