using System;
using UnityEngine;

// @TODO: We need to make base alien class from it
public class AlienShip : Ship
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

        gameObject.layer = LayerMask.NameToLayer("Alien");
        m_Team = ShipTeam.Enemy;

        m_BoxCollider.isTrigger = true;

        m_BehaviorComponent.StartBehavior(new BHFlow_Sequence()
            .AddNode(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed)))

            .AddService(new BHShipService_DestroyWhenOutOfBottomBound())
            .AddService(new BHShipService_FireWhenSeePlayer(90f))
        );
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

        // @TODO: Later we can make config for this animation
        m_BehaviorComponent.AddAction(new BHShipAction_AnimateSpriteColor(Color.red, Duration: 0.15f, bPulse: true));
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        var PlayerShip = Other.GetComponent<PlayerShip>();
        if (PlayerShip)
        {
            PlayerShip.HealthComponent.TakeDamage(m_DamageOnCollide);
        }
    }
}
