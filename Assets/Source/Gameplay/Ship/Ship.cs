using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Ship : CustomBehavior
{
    public enum Team
    {
        Player,
        Enemy
    }

    [SerializeField] protected Team m_ShipTeam = Team.Player;
    public Team ShipTeam => m_ShipTeam;

    [SerializeField] protected float m_DefaultSpeed = 2.5f;
    private float m_Speed;
    public float Speed => m_Speed;

    protected BoxCollider2D m_BoxCollider;
    public BoxCollider2D BoxCollider => m_BoxCollider;

    protected Rigidbody2D m_RigidBody;
    public Rigidbody2D RigidBody => m_RigidBody;

    protected SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;

    protected ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent => m_HealthComponent;

    protected ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent => m_WeaponComponent;

    protected BehaviorComponent m_BehaviorComponent;
    public BehaviorComponent BehaviorComponent => m_BehaviorComponent;

    public virtual void Initialize(BuffMultipliers Buffs)
    {
        Assert.IsNotNull(Buffs);

        m_BoxCollider = InitializeComponent<BoxCollider2D>();
        m_SpriteRenderer = InitializeComponent<SpriteRenderer>();

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
        m_BehaviorComponent.Initialize(this);

        m_RigidBody = InitializeComponent<Rigidbody2D>();
        m_RigidBody.gravityScale = 0f;

        m_HealthComponent = InitializeComponent<ShipHealthComponent>();
        m_HealthComponent.Initialize(Buffs);
        m_HealthComponent.OnDamageTaken += OnDamageTaken;

        m_WeaponComponent = InitializeComponent<ShipWeaponComponent>();
        Type[] WeaponTypes = OnPreInitializeWeapons();
        m_WeaponComponent.Initialize(Buffs, WeaponTypes);

        UseBuffs(Buffs);
    }

    private void Update()
    {
        ProcessInput();
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

    /** Overridable method
    */
    protected virtual void OnDamageTaken(float NewHealth, float DeltaHealth)
    { }

    private void UseBuffs(BuffMultipliers Buffs)
    {
        m_Speed = m_DefaultSpeed * Buffs.ShipSpeed;
    }
}
