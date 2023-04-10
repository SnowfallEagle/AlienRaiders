using UnityEngine;
using UnityEngine.Assertions;

public class Spawner : CustomBehavior
{
    public class Config
    {
        public const int AnyParam = -1;

        /** Have more priority than From->To */
        public int SpecificSpawnPattern = AnyParam;
        public int SpecificSpawnSubpattern = AnyParam;
        public int SpecificShipPattern = AnyParam;

        public int FromSpawnPattern = AnyParam;
        public int ToSpawnPattern = AnyParam;

        /** Only when SpawnPattern specified */
        public int FromSpawnSubpattern = AnyParam;
        public int ToSpawnSubpattern = AnyParam;

        public int FromShipPattern = AnyParam;
        public int ToShipPattern = AnyParam;

        /** Black = default */
        public Color ShipColor = Color.black;
        public BuffMultipliers Buffs = new BuffMultipliers();

        public string ResourcePath = null;
    }

    protected struct PrecomputedStuff
    {
        public Vector3 TargetSize;
        public Vector3 TargetCenter;
        public Vector3 LeftTop;
        public Vector3 CenterTop;

        public void Precompute()
        {
            TargetSize = RenderingService.Instance.TargetSize;
            TargetCenter = RenderingService.Instance.TargetCenter;

            LeftTop = TargetCenter;
            LeftTop.x -= TargetSize.x * 0.5f;
            LeftTop.y += TargetSize.y * 0.5f;

            CenterTop = TargetCenter;
            CenterTop.y += TargetSize.y * 0.5f;
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

        return Ships;
    }

    /** Derived classes should put spawn logic here */
    protected virtual GameObject[] OnSpawn()
    {
        return new GameObject[] { };
    }

    protected int GetPattern(int MaxPatterns)
    {
        if (m_Config.SpecificSpawnPattern != Config.AnyParam)
        {
            return m_Config.SpecificSpawnPattern;
        }

        int From = m_Config.FromSpawnPattern == Config.AnyParam ? 0           : m_Config.FromSpawnPattern;
        int To   = m_Config.ToSpawnPattern   == Config.AnyParam ? MaxPatterns : m_Config.ToSpawnPattern + 1;
        return UnityEngine.Random.Range(From, To);
    }

    protected int GetSubpattern(int MaxPatterns)
    {
        if (m_Config.SpecificSpawnSubpattern != Config.AnyParam && m_Config.SpecificSpawnPattern != Config.AnyParam)
        {
            return m_Config.SpecificSpawnSubpattern;
        }

        int From = m_Config.FromSpawnSubpattern == Config.AnyParam ? 0           : m_Config.FromSpawnSubpattern;
        int To   = m_Config.ToSpawnSubpattern   == Config.AnyParam ? MaxPatterns : m_Config.ToSpawnSubpattern + 1;
        return UnityEngine.Random.Range(From, To);
    }

    protected int GetShipPattern(int MaxPatterns)
    {
        if (m_Config.SpecificShipPattern == Config.AnyParam)
        {
            int From = m_Config.FromShipPattern == Config.AnyParam ? 0           : m_Config.FromShipPattern;
            int To   = m_Config.ToShipPattern   == Config.AnyParam ? MaxPatterns : m_Config.ToShipPattern + 1;
            return UnityEngine.Random.Range(From, To);
        }

        return m_Config.SpecificShipPattern;
    }

    private void InitializeShips(GameObject[] Ships)
    {
        BuffMultipliers Buffs =
            m_Config.Buffs *
            GameStateMachine.Instance.GetCurrentState<FightGameState>().EnemyBuffs;

        foreach (var Ship in Ships)
        {
            Ship.GetComponent<Ship>().Initialize(Buffs);
            if (m_Config.ShipColor != Color.black)
            {
                Ship.GetComponent<SpriteRenderer>().color = m_Config.ShipColor;
            }
        }
    }
}
