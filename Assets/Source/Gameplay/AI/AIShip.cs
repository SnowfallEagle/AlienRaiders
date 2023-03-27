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

    [SerializeField] protected float m_DamageOnCollide = 10f;
    public float DamageOnCollide => m_DamageOnCollide;

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        gameObject.layer = LayerMask.NameToLayer("Enemy");

        m_BoxCollider.isTrigger = true;
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

    private void OnTriggerEnter2D(Collider2D Other)
    {
        var PlayerShip = Other.GetComponent<PlayerShip>();
        if (PlayerShip)
        {
            PlayerShip.GetComponent<ShipHealthComponent>().TakeDamage(m_DamageOnCollide);
        }
    }
}
