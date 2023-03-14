using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ShipWeaponComponent : MonoBehaviour
{
    [SerializeField] protected Projectile m_Projectile;

    protected BoxCollider2D m_Collider;
    protected Ship m_Owner;

    [SerializeField] protected float m_FireRate = 0.1f;
    private float m_TimeLeftToFire = 0f;
    private bool m_bFiring = false;

    public void Start()
    {
        m_Collider = GetComponent<BoxCollider2D>();        
        m_Owner = GetComponent<Ship>();

        Assert.IsNotNull(m_Projectile);
        Assert.IsNotNull(m_Owner);
    }

    public void Update()
    {
        m_TimeLeftToFire -= Time.deltaTime;

        if (m_TimeLeftToFire <= 0f)
        {
            if (m_bFiring)
            {
                m_TimeLeftToFire = m_FireRate;
                Fire();
            }
            else
            {
                m_TimeLeftToFire = 0f;
            }
        }
    }

    protected void Fire()
    {
        Vector3 Position = transform.position;
        Position.y += gameObject.GetComponent<BoxCollider2D>().bounds.size.y * 0.5f;

        Projectile Projectile = Instantiate(m_Projectile, Position, Quaternion.identity);
        Projectile.Initialize(gameObject.GetComponent<Ship>().Team);
    }

    public void StartFire()
    {
        m_bFiring = true;
    }

    public void StopFire()
    {
        m_bFiring = false;
    }
}
