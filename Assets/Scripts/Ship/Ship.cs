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
    public Team ShipTeam { get => m_ShipTeam; }

    [SerializeField] protected bool m_bCheckBounds = false;

    private List<BHTask> m_TaskList = new List<BHTask>();

    protected BoxCollider2D m_BoxCollider;

    protected ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent { get => m_HealthComponent; }

    protected ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent { get => m_WeaponComponent; }

    protected virtual void Start()
    {
        m_BoxCollider = InitializeComponent<BoxCollider2D>();
        m_HealthComponent = InitializeComponent<ShipHealthComponent>();
        m_WeaponComponent = InitializeComponent<ShipWeaponComponent>();
    }

    private void Update()
    {
        ProcessInput();
    }

    private void LateUpdate()
    {
        UpdateTasks();
        CheckBounds();
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

    /** Overridable ProcessInput() method
        Should be overrided by derived classes.
        Example: Player processes touch input, AI handle its behaviour.
    */
    protected virtual void ProcessInput()
    { }

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
}
