using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Spawner : CustomBehavior
{
    public GameObject[] Spawn(SpawnerConfig Config)
    {
        Assert.IsNotNull(Config);

        GameObject[] Ships = OnSpawn(Config);
        Assert.IsNotNull(Ships);
        InitializeShips(Ships, Config);

        return Ships;
    }

    /** Derived classes should put spawn logic here */
    protected virtual GameObject[] OnSpawn(SpawnerConfig BaseConfig)
    {
        return new GameObject[] { };
    }

    private void InitializeShips(GameObject[] Ships, SpawnerConfig Config)
    {
        BuffMultipliers Buffs =
            Config.Buffs *
            GameStateMachine.Instance.GetCurrentState<FightGameState>().EnemyBuffs;

        foreach (var Ship in Ships)
        {
            Ship.GetComponent<Ship>().Initialize(Buffs);
            if (Config.ShipColor != Color.black)
            {
                Ship.GetComponent<SpriteRenderer>().color = Config.ShipColor;
            }
        }
    }
}
