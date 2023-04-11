public class IntroLevel : Level
{
    public IntroLevel()
    {
        Stages = new StageInfo[]
        {
            new StageInfo
            {
                Stage = typeof(IntroStage),
                EnemyBuffs = new BuffMultipliers
                {
                    ShipHealth = 0.1f,

                    ProjectileDamage = 10f
                }
            }
        };
    }
}

