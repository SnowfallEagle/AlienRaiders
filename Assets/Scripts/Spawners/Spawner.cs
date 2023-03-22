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
    }

    protected class Context
    {
        public Config Config;

        // Some precomputed stuff
        public Vector3 TargetSize;
        public Vector3 TargetCenter;
        public Vector3 LeftTop;

        public Context()
        {
            TargetSize = RenderingService.Instance.TargetSize;
            TargetCenter = RenderingService.Instance.TargetCenter;
            LeftTop = TargetCenter - TargetSize * 0.5f;
        }
    }

    protected Context m_Context;

    public GameObject[] Spawn(Config Config)
    {
        Assert.IsNotNull(Config);

        m_Context = new Context();
        m_Context.Config = Config;

        GameObject[] Ships = OnSpawn();
        Assert.IsNotNull(Ships);

        SetColors(Ships);
        Destroy(gameObject);

        return Ships;
    }

    // Derived classes should put spawn logic here
    protected virtual GameObject[] OnSpawn()
    {
        return new GameObject[] { };
    }

    private void SetColors(GameObject[] Ships)
    {
        foreach (var Ship in Ships)
        {
            Ship.GetComponent<SpriteRenderer>().color = m_Context.Config.ShipColor;
        }
    }
}
