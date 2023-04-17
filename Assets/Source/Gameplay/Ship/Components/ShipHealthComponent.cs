using UnityEngine;
using UnityEngine.Assertions;

public class ShipHealthComponent : CustomBehavior
{
    public delegate void OnHealthChangedSignature(float NewHealth, float DeltaHealth);

    public OnHealthChangedSignature OnHealthChanged;

    public delegate void OnDiedSignature();
    public OnDiedSignature OnDied;

    public delegate void OnShieldToggledSignature(bool bToggle);
    public OnShieldToggledSignature OnShieldToggled;

    public bool bDead => m_Health <= 0f;
    public bool bAlive => m_Health > 0f;

    private bool m_bShield = false;

    [Header("Health")]
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
        Assert.IsTrue(Damage >= 0f);

        if (m_bShield)
        {
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
        Assert.IsTrue(AdditionalHealth >= 0f);
        SetHealth(m_Health + AdditionalHealth);
    }

    public void SetMaxHealth()
    {
        SetHealth(m_MaxHealth);
    }

    public void ToggleShield(bool bToggle)
    {
        m_bShield = bToggle;
        OnShieldToggled?.Invoke(bToggle);
    }

    public void Kill()
    {
        SetHealth(-1f);
    }
}
