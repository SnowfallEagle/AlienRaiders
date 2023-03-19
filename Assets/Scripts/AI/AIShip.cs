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

        BehaviorComponent.AddTask(new BHTaskMoveBottom());
        BehaviorComponent.AddTask(new BHTaskDestroyWhenOutOfBottomBound());
        BehaviorComponent.AddTask(new BHTaskFireWhenSeePlayer(180f));
    }

    protected override void OnPreInitializeWeapons()
    {
        PreSetNumWeapons((int)Weapons.MaxWeapons);
        PreAddWeapon<LauncherWeapon>((int)Weapons.Launcher);
    }
}
