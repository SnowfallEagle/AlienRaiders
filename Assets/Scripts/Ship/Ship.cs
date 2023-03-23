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

    private Type[] m_WeaponTypes = new Type[] { };

    protected BoxCollider2D m_BoxCollider;
    public BoxCollider2D BoxCollider => m_BoxCollider;

    protected SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer SpriteRenderer => m_SpriteRenderer;

    protected ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent => m_HealthComponent;

    protected ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent => m_WeaponComponent;

    protected ShipBehaviorComponent m_BehaviorComponent;
    public ShipBehaviorComponent BehaviorComponent => m_BehaviorComponent;

    public virtual void Initialize(BuffMultipliers Buffs)
    {
        Assert.IsNotNull(Buffs);

        m_BoxCollider = InitializeComponent<BoxCollider2D>();
        m_SpriteRenderer = InitializeComponent<SpriteRenderer>();
        m_BehaviorComponent = InitializeComponent<ShipBehaviorComponent>();

        m_HealthComponent = InitializeComponent<ShipHealthComponent>();
        m_HealthComponent.Initialize(Buffs);

        m_WeaponComponent = InitializeComponent<ShipWeaponComponent>();
        OnPreInitializeWeapons();
        m_WeaponComponent.Initialize(Buffs, m_WeaponTypes);

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
        Derived class must use PreSetNumWeapons() and PreAddWeapon().
    */
    protected virtual void OnPreInitializeWeapons()
    { }

    protected void PreSetNumWeapons(int NumWeapons)
    {
        if (NumWeapons > 0)
        {
            Array.Resize(ref m_WeaponTypes, NumWeapons);
        }
    }

    protected void PreAddWeapon<T>(int Index) where T : Weapon
    {
        if (Index >= 0 && Index < m_WeaponTypes.Length)
        {
            m_WeaponTypes[Index] = typeof(T);
        }
    }

    private void UseBuffs(BuffMultipliers Buffs)
    {
        m_Speed = m_DefaultSpeed * Buffs.ShipSpeed;
    }
}
