using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealthComponent : MonoBehaviour
{
    private float m_Health;
    [SerializeField] protected float m_MaxHealth = 100f;

    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    private void Start()
    {
        SetHealth(m_MaxHealth);
    }

    private void SetHealth(float NewHealth)
    {
        m_Health = NewHealth;
        if (m_Health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float Damage)
    {
        Debug.Log(gameObject.name + " took " + Damage.ToString() + " damage");

        SetHealth(m_Health - Damage);
    }
}
