using UnityEngine;

// @TODO: Make base HealthComponent
public class ShipHealthComponent : CustomBehavior
{
    public delegate void OnDamageTakenSignature(float NewHealth, float DeltaHealth);

    public OnDamageTakenSignature OnDamageTaken;

    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    [SerializeField] protected float m_DefaultHealth = 100f;
    private float m_MaxHealth;
    private float m_Health;

    private bool m_bNeedToDestroy = false;

    /** Must be called from Ship */
    public void Initialize(BuffMultipliers Buffs)
    {
        m_MaxHealth = m_DefaultHealth * Buffs.ShipHealth;
        SetMaxHealth();
    }

    private void LateUpdate()
    {
        if (m_bNeedToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void SetHealth(float NewHealth)
    {
        float OldHealth = m_Health;
        m_Health = System.MathF.Min(NewHealth, m_MaxHealth);

        if (NewHealth < OldHealth)
        {
            OnDamageTaken?.Invoke(NewHealth, NewHealth - OldHealth);
        }

        if (NewHealth <= 0f)
        {
            // @TODO: Separate PlayerHealthComponent
            if (GetComponent<PlayerShip>())
            {
                gameObject.SetActive(false);
            }
            else
            {
                m_bNeedToDestroy = true;
            }
        }

        // @DEBUG
        if (GetComponent<PlayerShip>())
        {
            Debug.Log($"New Player Health: { m_Health }");
        }
    }

    public void TakeDamage(float Damage)
    {
        if (Damage < 0f)
        {
            NoEntry.Assert("Damage can't be < 0f!");
            return;
        }

        // @TODO: Move this logic in PlayerHealthComponent.CanBeDamaged()
        if (GameEnvironment.Instance.GetDebugOption<bool>("DebugPlayer.bGodMode") && GetComponent<PlayerShip>())
        {
            return;
        }

        SetHealth(m_Health - Damage);
    }

    public void AddHealth(float AdditionalHealth)
    {
        if (AdditionalHealth < 0f)        
        {
            NoEntry.Assert("AdditionalHealth can't be < 0f!");
            return;
        }

        SetHealth(m_Health + AdditionalHealth);
    }

    public void SetMaxHealth()
    {
        SetHealth(m_MaxHealth);
    }

    public void Kill()
    {
        SetHealth(-1f);
    }
}
