using System;
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

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        m_ShipTeam = Team.Enemy;

        BehaviorComponent.AddTask(new BHTaskMoveBottom());
        BehaviorComponent.AddTask(new BHTaskDestroyWhenOutOfBottomBound());
        BehaviorComponent.AddTask(new BHTaskFireWhenSeePlayer(90f));
    }

    protected override Type[] OnPreInitializeWeapons()
    {
        Type[] WeaponTypes = new Type[(int)Weapons.MaxWeapons];
        WeaponTypes[(int)Weapons.Launcher] = typeof(LauncherWeapon);
        return WeaponTypes;
    }
}
