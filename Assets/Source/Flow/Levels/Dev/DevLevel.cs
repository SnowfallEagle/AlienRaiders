using UnityEngine;

public class DevLevel : Level
{
    public DevLevel()
    {
        Stages = new StageInfo[]
        {
            new StageInfo { Stage = typeof(IntroStage) }
        };

        Appearance = new LevelAppearance
        {
            BackgroundOver = "Backgrounds/2",
            BackgroundUnder = "Backgrounds/3",
            CloudsColor = Color.red
        };
    }
}

