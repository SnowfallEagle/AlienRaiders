using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : Ship
{
    protected override void Start()
    {
        base.Start();

        m_ShipTeam = Team.Enemy;
    }
}
