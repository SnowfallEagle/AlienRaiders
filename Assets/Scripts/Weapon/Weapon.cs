using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CustomBehaviour
{
    [SerializeField] protected float m_FireRate = 0.1f;
    private float m_TimeLeftToFire = 0f;
    private bool m_bFiring = false;

    protected virtual void Start()
    { }

    private void Update()
    {
        m_TimeLeftToFire -= Time.deltaTime;

        if (m_TimeLeftToFire <= 0f)
        {
            if (m_bFiring)
            {
                m_TimeLeftToFire = m_FireRate;
                Fire();
            }
            else
            {
                m_TimeLeftToFire = 0f;
            }
        }
    }

    public void StartFire()
    {
        m_bFiring = true;
    }

    public void StopFire()
    {
        m_bFiring = false;
    }

    // Overridable method for derived weapons
    protected virtual void Fire()
    { }
}
