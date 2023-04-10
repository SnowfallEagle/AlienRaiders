using UnityEngine;
using UnityEngine.Assertions;

public class Weapon : CustomBehavior
{
    protected Ship m_Owner;
    protected BuffMultipliers m_Buffs;

    [SerializeField] protected float m_FireRate = 0.5f;
    private TimerService.Handle m_hFireTimer = new TimerService.Handle();

    public virtual void Initialize(BuffMultipliers Buffs)
    {
        m_Buffs = Buffs;

        m_Owner = transform.parent.GetComponent<Ship>();
        Assert.IsNotNull(m_Owner);
    }

    public void StartFire()
    {
        // @FIXME: We should set timers that'll fire when we can shoot again, because we can spam taps
        if (!m_hFireTimer.bValid)
        {
            TimerService.Instance.AddTimer(m_hFireTimer, this, Fire, m_FireRate, true);
        }
    }

    public void StopFire()
    {
        m_hFireTimer.Invalidate();
    }

    /** Overridable method for derived weapons */
    protected virtual void Fire()
    { }
}
