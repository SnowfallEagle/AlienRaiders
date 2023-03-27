using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealthComponent : CustomBehavior
{
    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    [SerializeField] protected float m_DefaultHealth = 100f;
    private float m_Health;

    private bool bNeedToDestroy = false;

    // Must be called from Ship
    public void Initialize(BuffMultipliers Buffs)
    {
        SetHealth(m_DefaultHealth * Buffs.ShipHealth);
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

        // TODO: Move this logic in PlayerHealthComponent
        if (GameEnvironment.Instance.GetDebugOption<bool>("DebugPlayer.bGodMode") && GetComponent<PlayerShip>())
        {
            return;
        }

        // TODO: Move this logic in AIShip
        GetComponent<ShipBehaviorComponent>().AddTask(new BHTaskAnimateSpriteColor(Color.red, bPulse: true));

        SetHealth(m_Health - Damage);
    }

    public void Kill()
    {
        SetHealth(-1f);
    }
}
