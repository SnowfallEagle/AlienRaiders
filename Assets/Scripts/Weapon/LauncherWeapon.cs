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
        if (!m_Projectile)
        {
            return;
        }

        Projectile Projectile = SpawnInState(m_Projectile, transform.position, transform.rotation);
        Projectile.Initialize(m_Owner.ShipTeam);
    }
}
