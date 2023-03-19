using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Ship : CustomBehaviour
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

    // TODO: Maybe it has more meaning to be only in PlayerShip class...
    [SerializeField] protected bool m_bCheckBounds = false;

    protected BoxCollider2D m_BoxCollider;

    protected ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent => m_HealthComponent;

    protected ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent => m_WeaponComponent;

    private Type[] m_WeaponTypes = new Type[] { };

    private List<BHTask> m_TaskList = new List<BHTask>();

    protected virtual void Start()
    {
        m_BoxCollider = InitializeComponent<BoxCollider2D>();
        m_HealthComponent = InitializeComponent<ShipHealthComponent>();
        m_WeaponComponent = InitializeComponent<ShipWeaponComponent>();

        OnPreInitializeWeapons();
        m_WeaponComponent.InitializeWeapons(m_WeaponTypes);
    }

    private void Update()
    {
        ProcessInput();
    }

    private void LateUpdate()
    {
        UpdateTasks(); // TODO: Move tasks in ShipBehaviorComponent
        CheckBounds();
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

    public void AddTask(BHTask Task)
    {
        Task.Start(this);
        if (!Task.bEnded)
        {
            m_TaskList.Add(Task);
        }
    }

    private void UpdateTasks()
    {
        foreach (var Task in m_TaskList)
        {
            Task.Update(this);
        }

        m_TaskList.RemoveAll(Task => Task.bEnded);
    }

    private void CheckBounds()
    {
        if (!m_bCheckBounds)
        {
            return;
        }

        var RenderingService = ServiceLocator.Instance.Get<RenderingService>();

        Vector3 BoundsCenter = RenderingService.TargetCenter;
        Vector3 BoundsSizeDiv2 = RenderingService.TargetSize / 2;

        Vector3 BoundsBottomLeft = BoundsCenter - BoundsSizeDiv2;
        Vector3 BoundsTopRight = BoundsCenter + BoundsSizeDiv2;

        Vector3 CurrentPosition = transform.position;

        if (CurrentPosition.x < BoundsBottomLeft.x) CurrentPosition.x = BoundsBottomLeft.x;
        if (CurrentPosition.y < BoundsBottomLeft.y) CurrentPosition.y = BoundsBottomLeft.y;

        if (CurrentPosition.y > BoundsTopRight.y) CurrentPosition.y = BoundsTopRight.y;
        if (CurrentPosition.x > BoundsTopRight.x) CurrentPosition.x = BoundsTopRight.x;

        transform.position = CurrentPosition;
    }

}
