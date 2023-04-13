public class IntroStage : FightStage
{
    public IntroStage()
    {
        m_Spawners = new SpawnerInfo[]
        {
            new SpawnerInfo
            {
                TimeToNext = 5f
            },

            new SpawnerInfo
            {
                Spawner = typeof(AlienSpawner),
                Config = new AlienSpawner.Config
                {
                    Pattern = new PatternSpawnerConfig { SpecificSpawnPattern = AlienSpawner.Pattern.Single },

                    NumGridCells = 6,
                    GridPosition = 2
                }
            },

            new SpawnerInfo
            {
                Spawner = typeof(AlienSpawner),
                Config = new AlienSpawner.Config
                {
                    Pattern = new PatternSpawnerConfig { SpecificSpawnPattern = AlienSpawner.Pattern.Single },

                    NumGridCells = 6,
                    GridPosition = 5
                },

                bWaitToEnd = true,
                TimeToNext = 2f
            },

            new SpawnerInfo
            {
                Spawner = typeof(AlienSpawner),
                Config = new AlienSpawner.Config
                {
                    Pattern = new PatternSpawnerConfig { SpecificSpawnPattern = AlienSpawner.Pattern.Single },

                    ResourcePath = "Aliens/FlashRocketer",
                    Align = SpawnerConfig.AlignType.Center
                },

                TimeToNext = 4f
            },

            new SpawnerInfo
            {
                Spawner = typeof(AlienSpawner),
                Config = new AlienSpawner.Config
                {
                    Pattern = new PatternSpawnerConfig
                    {
                        SpecificSpawnPattern = AlienSpawner.Pattern.Triple,
                        SpecificSpawnSubpattern = AlienSpawner.TripleSubpattern.Row
                    },

                    SpaceBetweenAliens = 3f,

                    NumGridCells = 6,
                    GridPosition = 2.5f
                }
            },

            new SpawnerInfo
            {
                Spawner = typeof(AlienSpawner),
                Config = new AlienSpawner.Config
                {
                    Pattern = new PatternSpawnerConfig
                    {
                        SpecificSpawnPattern = AlienSpawner.Pattern.Triple,
                        SpecificSpawnSubpattern = AlienSpawner.TripleSubpattern.Row
                    },

                    SpaceBetweenAliens = 3f,

                    NumGridCells = 6,
                    GridPosition = 4.5f
                },

                bWaitToEnd = true,
                TimeToNext = 1f
            },

            new SpawnerInfo
            {
                Spawner = typeof(OneBigTwoNearSpawner),
                Config = new OneBigTwoNearSpawner.Config
                {
                    Align = SpawnerConfig.AlignType.Center
                }
            }
        };
    }
}
