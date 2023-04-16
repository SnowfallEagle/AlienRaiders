using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Ship : CustomBehavior
{
    public enum ShipTeam
    {
        Player,
        Enemy
    }

    [SerializeField] protected ShipTeam m_Team = ShipTeam.Player;
    public ShipTeam Team => m_Team;

    [SerializeField] protected float m_DefaultSpeed = 2.5f;
    private float m_Speed;
    public float Speed => m_Speed;

    public bool bProcessInput = true;

    private BoxCollider2D m_BoxCollider;
    public BoxCollider2D BoxCollider => m_BoxCollider;

    private Rigidbody2D m_RigidBody;
    public Rigidbody2D RigidBody => m_RigidBody;

    private SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;

    private ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent => m_HealthComponent;

    private ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent => m_WeaponComponent;

    private BehaviorComponent m_BehaviorComponent;
    public BehaviorComponent BehaviorComponent => m_BehaviorComponent;

    public virtual void Initialize(BuffMultipliers Buffs)
    {
        Assert.IsNotNull(Buffs);

        m_BoxCollider = InitializeComponent<BoxCollider2D>();
        m_SpriteRenderer = InitializeComponent<SpriteRenderer>();
        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();

        m_RigidBody = InitializeComponent<Rigidbody2D>();
        m_RigidBody.gravityScale = 0f;

        m_HealthComponent = InitializeComponent<ShipHealthComponent>();
        m_HealthComponent.Initialize(Buffs);
        HealthComponent.OnHealthChanged += (_, DeltaHealth) =>
        {
            if (DeltaHealth < 0f)
            {
                BehaviorComponent.AddExclusiveAction(new BHShipAction_AnimateSpriteColor(Color.red, 0.1f, bPulse: true));
            }
        };

        m_WeaponComponent = InitializeComponent<ShipWeaponComponent>();
        Type[] WeaponTypes = OnPreInitializeWeapons();
        m_WeaponComponent.Initialize(Buffs, WeaponTypes);

        UseBuffs(Buffs);
    }

    private void Update()
    {
        if (bProcessInput)
        {
            ProcessInput();
        }
    }

    /** Overridable ProcessInput() method
        Should be overrided by derived classes.
        Example: Player processes touch input, AI handle its behaviour.
    */
    protected virtual void ProcessInput()
    { }

    /** Overridable OnPreInitializeWeapons() method
        Should be overrided by derived classes to set up weapons.
    */
    protected virtual Type[] OnPreInitializeWeapons()
    {
        return new Type[] { };
    }

    private void UseBuffs(BuffMultipliers Buffs)
    {
        m_Speed = m_DefaultSpeed * Buffs.ShipSpeed;
    }
}
