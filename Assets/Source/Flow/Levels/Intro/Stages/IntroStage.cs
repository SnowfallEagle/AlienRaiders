using System.Collections.Generic;

public class IntroStage : FightStage
{
    public IntroStage()
    {
        m_Spawners = new SpawnerInfo[]
        {
            new SpawnerInfo
            {
                Spawner = typeof(OneBigTwoNearSpawner),
                Config = new Spawner.Config
                {
                    StringValues = new Dictionary<string, string>()
                    {
                        { "NearResourcePath", "Ships/FlashRocketer" },
                        { "BigResourcePath", "Ships/BigAlien" },
                    },

                    FloatValues = new Dictionary<string, float>
                    {
                        { "NumGridCells", 10 },
                        { "GridPosition", 3 },
                    }
                },

                Pickup = "Pickups/Pickup",

                bWaitToEnd = true,
            }
        };
    }
}
