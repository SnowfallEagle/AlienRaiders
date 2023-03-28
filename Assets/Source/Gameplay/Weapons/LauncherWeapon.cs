using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LauncherWeapon : Weapon
{
    [SerializeField] protected Projectile m_Projectile;
    private Ship m_Owner;

    protected override void Start()
    {
        base.Start();

        m_Owner = transform.parent.GetComponent<Ship>();

        Assert.IsNotNull(m_Owner);
    }

    protected override void Fire()
    {
        Projectile Projectile = SpawnInState(m_Projectile);
        Projectile.transform.position = transform.position;
        Projectile.transform.rotation = transform.rotation;

        Projectile.Initialize(m_Owner.ShipTeam, m_Buffs);
    }
}
