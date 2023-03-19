using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealthComponent : CustomBehaviour
{
    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    private float m_Health;
    [SerializeField] protected float m_MaxHealth = 100f;

    private bool bNeedToDestroy = false;

    private void Start()
    {
        SetHealth(m_MaxHealth);
    }

    private void LateUpdate()
    {
        if (bNeedToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void SetHealth(float NewHealth)
    {
        m_Health = NewHealth;
        if (m_Health <= 0f)
        {
            bNeedToDestroy = true;
        }
    }

    public void TakeDamage(float Damage)
    {
        Debug.Log(gameObject.name + " took " + Damage.ToString() + " damage");

        SetHealth(m_Health - Damage);
    }

    public void Kill()
    {
        SetHealth(-1f);
    }
}
