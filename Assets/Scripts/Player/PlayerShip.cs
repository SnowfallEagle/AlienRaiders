using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShip : Ship
{
    [SerializeField] protected Projectile m_Projectile;

    public override Team Team { get => Team.Player; }

    protected override void Start()
    {
        base.Start();

        Assert.IsNotNull(m_Projectile);
    }

    public override void Fire()
    {
        Vector3 Position = transform.position;
        Position.y += m_BoxCollider.bounds.size.y * 0.5f;

        Projectile Projectile = Instantiate(m_Projectile, Position, Quaternion.identity);
        Projectile.Initialize(Team);
    }
}
