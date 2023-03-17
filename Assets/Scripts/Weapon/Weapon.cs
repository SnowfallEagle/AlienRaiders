using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CustomBehaviour
{
    [SerializeField] protected float m_FireRate = 0.1f;
    private TimerService.Handle m_hFireTimer;

    private bool m_bFiring = false;

    protected virtual void Start()
    { }

    private void Update()
    { }

    public void StartFire()
    {
        m_bFiring = true;

        m_hFireTimer = ServiceLocator.Instance.Get<TimerService>().AddTimer(Fire, m_FireRate, true);
    }

    public void StopFire()
    {
        m_bFiring = false;

        ServiceLocator.Instance.Get<TimerService>().RemoveTimer(m_hFireTimer);
    }

    // Overridable method for derived weapons
    protected virtual void Fire()
    { }
}
