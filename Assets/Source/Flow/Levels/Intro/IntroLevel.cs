public class IntroLevel : Level
{
    public IntroLevel()
    {
        Stages = new StageInfo[]
        {
            new StageInfo { Stage = typeof(IntroStage) }
        };
    }
}

