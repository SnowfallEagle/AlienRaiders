using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_Damage = 5f;
    [SerializeField] protected float m_LifeTime = 5f;
    [SerializeField] protected float m_Speed = 0.1f;

    Team m_OwnerTeam;

    private void Start()
    {
        Destroy(gameObject, m_LifeTime);
    }

    private void Update()
    {
        transform.Translate(0f, m_Speed * Time.deltaTime, 0f);
        Debug.Log(transform.position.y);
    }

    public void Initialize(Team OwnerTeam)
    {
        m_OwnerTeam = OwnerTeam;
    }

    private void OnDestroy()
    {
        Debug.Log("Projectile: " + name + " is destroyed...");
    }

    protected virtual void OnTriggerEnter2D(Collider2D Other)
    {
        Ship Ship = Other.GetComponent<Ship>();
        if (Ship && Ship.Team != m_OwnerTeam)
        {
            // TODO: Maybe spawn effect?
            Ship.TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
