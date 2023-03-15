using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : CustomBehaviour
{
    [SerializeField] protected float m_Damage = 5f;
    [SerializeField] protected float m_LifeTime = 5f;
    [SerializeField] protected float m_Speed = 5f;

    Ship.Team m_OwnerTeam;

    private void Start()
    {
        var Rigidbody = InitializeComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0f;

        Destroy(gameObject, m_LifeTime);
    }

    private void Update()
    {
        transform.Translate(0f, m_Speed * Time.deltaTime, 0f);
    }

    public void Initialize(Ship.Team OwnerTeam)
    {
        m_OwnerTeam = OwnerTeam;
    }

    private void OnDestroy()
    {
        Debug.Log("Projectile: " + name + " is destroyed...");
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        Ship Ship = Other.GetComponent<Ship>();
        if (Ship && Ship.ShipTeam != m_OwnerTeam)
        {
            // TODO: Maybe spawn effect?
            Ship.GetComponent<ShipHealthComponent>().TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
