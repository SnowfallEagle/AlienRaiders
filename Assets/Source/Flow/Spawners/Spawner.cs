using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Spawner : CustomBehavior
{
    public class Config
    {
        public Dictionary<string, int> IntValues = new Dictionary<string, int>();
        public Dictionary<string, float> FloatValues = new Dictionary<string, float>();
        public Dictionary<string, string> StringValues = new Dictionary<string, string>();

        /** Black = default */
        public Color ShipColor = Color.black;
        public BuffMultipliers Buffs = new BuffMultipliers();
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
        int SpecificSpawnPattern;
        if (m_Config.IntValues.TryGetValue("SpecificSpawnPattern", out SpecificSpawnPattern))
        {
            return SpecificSpawnPattern;
        }

        int From;
        if (!m_Config.IntValues.TryGetValue("FromSpawnPattern", out From))
        {
            From = 0;
        }

        int To;
        if (!m_Config.IntValues.TryGetValue("ToSpawnPattern", out To))
        {
            To = MaxPatterns;
        }
        else
        {
            ++To; // Exclusive range
        }

        return Random.Range(From, To);
    }

    protected int GetSubpattern(int MaxPatterns)
    {
        int SpecificSpawnSubpattern;
        if (m_Config.IntValues.TryGetValue("SpecificSpawnSubpattern", out SpecificSpawnSubpattern) &&
            m_Config.IntValues.ContainsKey("SpecificSpawnPattern"))
        {
            return SpecificSpawnSubpattern;
        }

        int From;
        if (!m_Config.IntValues.TryGetValue("FromSpawnSubpattern", out From))
        {
            From = 0;
        }

        int To;
        if (!m_Config.IntValues.TryGetValue("ToSpawnSubpattern", out To))
        {
            To = MaxPatterns;
        }
        else
        {
            ++To; // Exclusive range
        }

        return Random.Range(From, To);
    }

    protected int GetShipPattern(int MaxPatterns)
    {
        // @INCOMPLETE
        Assert.IsTrue(false, "Not implemented");
        return 0;
    }

    protected bool GetSpawnPosition(out Vector3 Position)
    {
        float GridPosition;
        if (m_Config.FloatValues.TryGetValue("GridPosition", out GridPosition))
        {
            float NumGridCells;
            if (!m_Config.FloatValues.TryGetValue("NumGridCells", out NumGridCells))
            {
                NumGridCells = 1;
            }
            Assert.IsTrue(NumGridCells > 0);

            float CellWidth = RenderingService.Instance.TargetSize.x / NumGridCells;
            Position = s_Precomputed.LeftTop;
            Position.x += CellWidth * (GridPosition - 1) + (CellWidth * 0.5f);

            return true;
        }

        Position = Vector3.zero;
        return false;
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
