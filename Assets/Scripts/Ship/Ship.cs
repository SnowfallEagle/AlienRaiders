using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum Team
{
    Player,
    Enemy
}

[RequireComponent(typeof(BoxCollider2D))]
public class Ship : MonoBehaviour
{
    public virtual Team Team { get => Team.Player; }

    protected ShipHealthComponent m_HealthComponent;
    protected ShipWeaponComponent m_WeaponComponent;

    protected BoxCollider2D m_BoxCollider;

    protected virtual void Start()
    {
        m_HealthComponent = GetComponent<ShipHealthComponent>();
        m_WeaponComponent = GetComponent<ShipWeaponComponent>();

        Assert.IsNotNull(m_HealthComponent);
        Assert.IsNotNull(m_WeaponComponent);

        m_BoxCollider = GetComponent<BoxCollider2D>();
    }
}
