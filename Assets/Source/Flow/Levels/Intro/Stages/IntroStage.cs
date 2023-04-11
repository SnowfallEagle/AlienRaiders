using System.Collections.Generic;

public class IntroStage : FightStage
{
    public IntroStage()
    {
        m_Spawners = new SpawnerInfo[]
        {
            new SpawnerInfo
            {
                Type = typeof(OneBigTwoNearSpawner),
                Config = new Spawner.Config()
                {
                    StringValues = new Dictionary<string, string>()
                    {
                        { "NearResourcePath", "Ships/FlashRocketer" },
                        { "BigResourcePath", "Ships/BigAlien" },
                    }
                },
                bWaitToEnd = true,
            }
        };
    }
}
