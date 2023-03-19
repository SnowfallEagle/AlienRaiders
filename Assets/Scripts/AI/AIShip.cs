using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShip : Ship
{
    private enum Weapons
    {
        Launcher,
        MaxWeapons
    }

    protected override void Start()
    {
        base.Start();

        m_ShipTeam = Team.Enemy;

        AddTask(new BHTaskMoveBottom());
        AddTask(new BHTaskDestroyWhenOutOfBottomBound());
        AddTask(new BHTaskFireWhenSeePlayer(360f));
    }
    protected override void OnPreInitializeWeapons()
    {
        PreSetNumWeapons((int)Weapons.MaxWeapons);
        PreAddWeapon<LauncherWeapon>((int)Weapons.Launcher);
    }
}
