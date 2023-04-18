using UnityEngine;

public class DevStage : FightStage
{
    public DevStage()
    {
        m_Spawners = new SpawnerInfo[]
        {
            new SpawnerInfo
            {
                TimeToNext = 1f
            }
        };
    }
}

public class DevLevel : Level
{
    public DevLevel()
    {
        Stages = new StageInfo[]
        {
            new StageInfo { Stage = typeof(DevStage) }
        };

        Appearance = new LevelAppearance
        {
            BackgroundOver = "Backgrounds/2",
            BackgroundUnder = "Backgrounds/3",
            CloudsColor = Color.red
        };
    }
}

