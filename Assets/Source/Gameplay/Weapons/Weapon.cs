using UnityEngine;
using UnityEngine.Assertions;

public class Weapon : CustomBehavior
{
    protected Ship m_Owner;
    protected BuffMultipliers m_Buffs;

    [SerializeField] protected float m_FireRate = 0.5f;
    private TimerService.Handle m_hFireTimer       = new TimerService.Handle();
    private TimerService.Handle m_hShootDelayTimer = new TimerService.Handle();

    public virtual void Initialize(BuffMultipliers Buffs)
    {
        m_Buffs = Buffs;

        m_Owner = transform.parent.GetComponent<Ship>();
        Assert.IsNotNull(m_Owner);
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        {
            m_hFireTimer.Invalidate();
            m_hShootDelayTimer.Invalidate();
        }
    }

    public void StartFire()
    {
        if (!m_hFireTimer.bValid && !m_hShootDelayTimer.bValid)
        {
            TimerService.Instance.AddTimer(m_hFireTimer, this, Fire, m_FireRate, true);
        }
    }

    public void StopFire()
    {
        if (m_hFireTimer.bValid)
        {
            TimerService.Instance.AddTimer(m_hShootDelayTimer, this, () => { }, m_hFireTimer.Timer.TimeLeftToFire);
            m_hFireTimer.Invalidate();
        }
    }

    /** Overridable method for derived weapons */
    protected virtual void Fire()
    { }
}
