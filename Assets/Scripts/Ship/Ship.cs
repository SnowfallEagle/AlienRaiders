using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player,
    Enemy
}

[RequireComponent(typeof(BoxCollider2D))]
public class Ship : MonoBehaviour
{
    public virtual Team Team { get => Team.Player; }

    private float m_Health;
    [SerializeField] protected float m_MaxHealth = 100f;

    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    protected BoxCollider2D m_BoxCollider;

    protected virtual void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();

        SetHealth(m_MaxHealth);
    }

    public virtual void Fire() { }

    private void SetHealth(float NewHealth)
    {
        m_Health = NewHealth;
        if (m_Health <= 0f)
        {
            OnDeath();
        }
    }

    public virtual void TakeDamage(float Damage)
    {
        Debug.Log(gameObject.name + " took " + Damage.ToString() + " damage");

        SetHealth(m_Health - Damage);
    }

    protected virtual void OnDeath()
    {
        Debug.Log(gameObject.name + " died");

        Destroy(gameObject);
    }
}
