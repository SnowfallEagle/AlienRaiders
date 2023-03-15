using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public class Ship : CustomBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    public virtual Team ShipTeam { get => Team.Player; }

    protected BoxCollider2D m_BoxCollider;

    protected ShipHealthComponent m_HealthComponent;
    public ShipHealthComponent HealthComponent { get => m_HealthComponent; }

    protected ShipWeaponComponent m_WeaponComponent;
    public ShipWeaponComponent WeaponComponent { get => m_WeaponComponent; }

    private List<BHTask> m_TaskList = new List<BHTask>();

    protected virtual void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_HealthComponent = GetComponent<ShipHealthComponent>();
        m_WeaponComponent = GetComponent<ShipWeaponComponent>();

        Assert.IsNotNull(m_HealthComponent);
        Assert.IsNotNull(m_WeaponComponent);
    }

    private void LateUpdate()
    {
        UpdateTasks();
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
}
