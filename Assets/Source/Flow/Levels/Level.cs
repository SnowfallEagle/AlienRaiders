using System;
using UnityEngine;

public class LevelAppearance
{
    public string BackgroundOver = "Backgrounds/1";
    public string BackgroundUnder = "Backgrounds/5";

    public Color CloudsColor = Color.black;

    // @TODO: Star colors, PCBorder sprites and colors
}

/** Levels should set fields in their constructor */
public abstract class Level
{
    public class StageInfo
    {
        public Type Stage = typeof(FightStage);
        public BuffMultipliers EnemyBuffs = new BuffMultipliers();
    }

    public StageInfo[] Stages;
    public BuffMultipliers EnemyBuffs = new BuffMultipliers();
    public LevelAppearance Appearance = new LevelAppearance();
}
