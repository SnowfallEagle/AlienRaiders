using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Spawner : CustomBehavior
{
    public class Config
    {
        public const int AnyParam = -1;

        public int SpawnPattern = AnyParam;
        public int SpawnSubpattern = AnyParam; // Only when SpawnPattern specified

        public int ShipPattern = AnyParam; // TODO: Zig-Zag, Just Bottom, etc...
        public Color ShipColor = Color.white;

        public BuffMultipliers Buffs = new BuffMultipliers();
    }

    protected struct PrecomputedStuff
    {
        public bool bPrecomputed;

        public Vector3 TargetSize;
        public Vector3 TargetCenter;
        public Vector3 LeftTop;

        public void Precompute()
        {
            TargetSize = RenderingService.Instance.TargetSize;
            TargetCenter = RenderingService.Instance.TargetCenter;
            LeftTop = TargetCenter - TargetSize * 0.5f;
        }
    }

    protected Config m_Config;
    static protected PrecomputedStuff s_Precomputed = new PrecomputedStuff();
    static private bool s_bPrecomputed = false;

    public GameObject[] Spawn(Config Config)
    {
        if (!s_bPrecomputed)
        {
            s_Precomputed.Precompute();
            s_bPrecomputed = true;
        }

        Assert.IsNotNull(Config);

        m_Config = Config;

        GameObject[] Ships = OnSpawn();
        Assert.IsNotNull(Ships);
        InitializeShips(Ships);

        Destroy(gameObject);

        return Ships;
    }

    // Derived classes should put spawn logic here
    protected virtual GameObject[] OnSpawn()
    {
        return new GameObject[] { };
    }

    private void InitializeShips(GameObject[] Ships)
    {
        BuffMultipliers Buffs =
            m_Config.Buffs *
            GameStateMachine.Instance.GetCurrentState<FightGameState>().EnemyBuffs;

        foreach (var Ship in Ships)
        {
            Ship.GetComponent<Ship>().Initialize(Buffs);
            Ship.GetComponent<SpriteRenderer>().color = m_Config.ShipColor;
        }
    }
}
