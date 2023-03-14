using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public class ShipWeaponComponent : MonoBehaviour
{
    [SerializeField] protected Projectile m_Projectile;

    protected BoxCollider2D m_Collider;
    protected Ship m_Owner;

    public virtual void Start()
    {
        m_Collider = GetComponent<BoxCollider2D>();        
        m_Owner = GetComponent<Ship>();

        Assert.IsNotNull(m_Projectile);
        Assert.IsNotNull(m_Owner);
    }

    public virtual void Fire()
    {
        Vector3 Position = transform.position;
        Position.y += gameObject.GetComponent<BoxCollider2D>().bounds.size.y * 0.5f;

        Projectile Projectile = Instantiate(m_Projectile, Position, Quaternion.identity);
        Projectile.Initialize(gameObject.GetComponent<Ship>().Team);
    }
}
