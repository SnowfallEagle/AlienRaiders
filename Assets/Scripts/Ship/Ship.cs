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

    [SerializeField] protected float m_Speed = 2.5f;
    public float Speed => m_Speed;

    protected BoxCollider2D m_BoxCollider;

    protected ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent => m_HealthComponent;

    protected ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent => m_WeaponComponent;

    private Type[] m_WeaponTypes = new Type[] { };

    protected ShipBehaviorComponent m_BehaviorComponent;
    public ShipBehaviorComponent BehaviorComponent => m_BehaviorComponent;

    protected virtual void Start()
    {
        m_BoxCollider = InitializeComponent<BoxCollider2D>();
        m_HealthComponent = InitializeComponent<ShipHealthComponent>();
        m_WeaponComponent = InitializeComponent<ShipWeaponComponent>();
        m_BehaviorComponent = InitializeComponent<ShipBehaviorComponent>();

        OnPreInitializeWeapons();
        m_WeaponComponent.InitializeWeapons(m_WeaponTypes);
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
}
