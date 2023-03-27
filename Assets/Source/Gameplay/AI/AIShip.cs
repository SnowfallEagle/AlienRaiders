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
        m_ShipTeam = Team.Enemy;

        m_BoxCollider.isTrigger = true;

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

    protected override void OnDamageTaken(float NewHealth, float DeltaHealth)
    {
        base.OnDamageTaken(NewHealth, DeltaHealth);

        // TODO: Later we can make config for this animation
        m_BehaviorComponent.AddTask(new BHTaskAnimateSpriteColor(Color.red, Duration: 0.15f, bPulse: true));
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
