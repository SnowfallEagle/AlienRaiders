using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : CustomBehavior
{
    [SerializeField] protected float m_DefaultSpeed = 5f;
    [SerializeField] protected float m_DefaultDamage = 5f;
    [SerializeField] protected float m_LifeTime = 5f;

    private float m_Damage = 5f;
    private float m_Speed = 5f;

    private Ship.Team m_OwnerTeam;

    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Bullet");

        var Rigidbody = InitializeComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0f;

        var BoxCollider = InitializeComponent<BoxCollider2D>();
        BoxCollider.isTrigger = true;

        // TODO: Behavior component

        Destroy(gameObject, m_LifeTime);
    }

    private void Update()
    {
        // TODO: Put in BasicProjectile's task
        transform.Translate(0f, m_Speed * Time.deltaTime, 0f);
    }

    public void Initialize(Ship.Team OwnerTeam, BuffMultipliers Buffs)
    {
        m_OwnerTeam = OwnerTeam;

        m_Speed = m_DefaultSpeed * Buffs.ProjectileSpeed;
        m_Damage = m_DefaultDamage * Buffs.ProjectileDamage;
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        Ship Ship = Other.GetComponent<Ship>();
        if (Ship && Ship.ShipTeam != m_OwnerTeam)
        {
            // TODO: Maybe spawn effect?
            Ship.HealthComponent.TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
