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
                Config = new OneBigTwoNearSpawner.Config
                {
                    NearResourcePath = "Ships/FlashRocketer",
                    BigResourcePath = "Ships/BigAlien",

                    NumGridCells = 10,
                    GridPosition = 3,
                },

                Pickup = "Pickups/Pickup",

                bWaitToEnd = true,
            },

            new SpawnerInfo
            {
                Spawner = typeof(AlienSpawner),
                Config = new AlienSpawner.Config
                {
                    Pattern = new MPatternSpawnerConfig
                    {
                        SpecificSpawnPattern = AlienSpawner.Pattern.Triple,
                        SpecificSpawnSubpattern = AlienSpawner.TripleSubpattern.Row
                    }
                }
            }
        };
    }
}
