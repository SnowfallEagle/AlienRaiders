using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CustomBehavior
{
    [SerializeField] protected float m_FireRate = 0.5f;
    private TimerService.Handle m_hFireTimer = new TimerService.Handle();

    protected BuffMultipliers m_Buffs;

    public void Initialize(BuffMultipliers Buffs)
    {
        m_Buffs = Buffs;
    }

    protected virtual void Start()
    { }

    private void Update()
    { }

    public void StartFire()
    {
        // FIXME: We should set timers that'll fire when we can shoot again, because we can spam taps
        if (!m_hFireTimer.bValid)
        {
            TimerService.Instance.AddTimer(m_hFireTimer, this, Fire, m_FireRate, true);
        }
    }

    public void StopFire()
    {
        m_hFireTimer.Invalidate();
    }

    /** Overridable method for derived weapons
    */
    protected virtual void Fire()
    { }
}
