using System;
using UnityEngine;

public class ShipWeaponComponent : CustomBehavior
{
    private Weapon[] m_Weapons;
    private Weapon CurrentWeapon;

    public void InitializeWeapons(Type[] WeaponTypes)
    {
        Array.Resize(ref m_Weapons, WeaponTypes.Length);

        Weapon[] ChildrenWeapons = GetComponentsInChildren<Weapon>();
        foreach (var ChildWeapon in ChildrenWeapons)
        {
            for (int i = 0; i < WeaponTypes.Length; ++i)
            {
                if (ChildWeapon.GetType() == WeaponTypes[i])
                {
                    m_Weapons[i] = ChildWeapon;
                    Debug.Log(ChildWeapon + " is " + i);
                    break;
                }
            }
        }

        SwitchWeapon(0);
    }

    public void StartFire()
    {
        CurrentWeapon?.StartFire();
    }

    public void StopFire()
    {
        CurrentWeapon?.StopFire();
    }

    public void SwitchWeapon(int Index)
    {
        if (Index >= 0 && Index < m_Weapons.Length)
        {
            CurrentWeapon = m_Weapons[Index];
        }
    }
}
