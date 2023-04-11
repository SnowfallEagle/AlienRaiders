using System;

/** Levels should set Stages and EnemyBuffs in their constructor */
public abstract class Level
{
    public class StageInfo
    {
        public Type Stage = typeof(FightStage);
        public BuffMultipliers EnemyBuffs = new BuffMultipliers();
    }

    public StageInfo[] Stages;
    public BuffMultipliers EnemyBuffs = new BuffMultipliers();
}
