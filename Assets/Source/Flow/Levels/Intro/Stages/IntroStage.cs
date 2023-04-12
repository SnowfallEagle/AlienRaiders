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
                },

                Pickup = "Pickups/Pickup",

                bWaitToEnd = true,
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

                    Align = SpawnerConfig.AlignType.Left
                },
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

                    Align = SpawnerConfig.AlignType.Right
                },

                bWaitToEnd = true
            }
        };
    }
}
