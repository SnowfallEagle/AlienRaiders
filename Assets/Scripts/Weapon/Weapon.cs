using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CustomBehavior
{
    [SerializeField] protected float m_FireRate = 0.5f;
    private TimerService.Handle m_hFireTimer = new TimerService.Handle();

    protected virtual void Start()
    { }

    private void Update()
    { }

    private void OnDestroy()
    {
        StopFire();
    }

    public void StartFire()
    {
        // FIXME: We should set timers that'll fire when we can shoot again, because we can spam taps
        if (!m_hFireTimer.bValid)
        {
            TimerService.Instance.AddTimer(m_hFireTimer, Fire, m_FireRate, true);
        }
    }

    public void StopFire()
    {
        TimerService.Instance.RemoveTimer(m_hFireTimer);
    }

    // Overridable method for derived weapons
    protected virtual void Fire()
    { }
}
