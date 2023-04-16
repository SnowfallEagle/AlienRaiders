using UnityEngine;

public class ShipHealthComponent : CustomBehavior
{
    public delegate void OnHealthChangedSignature(float NewHealth, float DeltaHealth);

    public OnHealthChangedSignature OnHealthChanged;

    public delegate void OnDiedSignature();
    public OnDiedSignature OnDied;

    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    [SerializeField] protected float m_DefaultHealth = 100f;
    private float m_MaxHealth;
    private float m_Health;

    /** Must be called from Ship */
    public void Initialize(BuffMultipliers Buffs)
    {
        m_MaxHealth = m_DefaultHealth * Buffs.ShipHealth;
        SetMaxHealth();
    }

    private void SetHealth(float NewHealth)
    {
        float OldHealth = m_Health;
        m_Health = System.MathF.Min(NewHealth, m_MaxHealth);

        OnHealthChanged?.Invoke(NewHealth, m_Health - OldHealth);

        if (m_Health <= 0f)
        {
            OnDied?.Invoke();
        }
    }

    public void TakeDamage(float Damage)
    {
        if (Damage < 0f)
        {
            NoEntry.Assert("Damage can't be < 0f!");
            return;
        }

#if UNITY_EDITOR
        if (GameEnvironment.Instance.GetDebugOption<bool>("DebugPlayer.bGodMode") && GetComponent<PlayerShip>())
        {
            return;
        }
#endif

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
