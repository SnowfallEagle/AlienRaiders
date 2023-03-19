using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CustomBehaviour
{
    [SerializeField] protected float m_FireRate = 0.1f;
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
        if (!m_hFireTimer.bValid)
        {
            ServiceLocator.Instance.Get<TimerService>().AddTimer(m_hFireTimer, Fire, m_FireRate, true);
        }
    }

    public void StopFire()
    {
        ServiceLocator.Instance.Get<TimerService>().RemoveTimer(m_hFireTimer);
    }

    // Overridable method for derived weapons
    protected virtual void Fire()
    { }
}
