using System;

public class ShipWeaponComponent : CustomBehavior
{
    private Weapon[] m_Weapons;
    private Weapon CurrentWeapon;

    public void Initialize(BuffMultipliers Buffs, Type[] WeaponTypes)
    {
        InitializeWeapons(WeaponTypes);
        UseBuffs(Buffs);
    }

    public void StartFire()
    {
        CurrentWeapon?.StartFire();
    }

    public void StopFire()
    {
        CurrentWeapon?.StopFire();
    }

    public void SwitchWeapon(int Idx)
    {
        if (Idx >= 0 && Idx < m_Weapons.Length)
        {
            CurrentWeapon = m_Weapons[Idx];
        }
    }

    private void InitializeWeapons(Type[] WeaponTypes)
    {
        Array.Resize(ref m_Weapons, WeaponTypes.Length);

        Weapon[] ChildrenWeapons = GetComponentsInChildren<Weapon>();
        foreach (var ChildWeapon in ChildrenWeapons)
        {
            for (int i = 0; i < WeaponTypes.Length; ++i)
            {
                System.Type ChildType = ChildWeapon.GetType();
                if (ChildType == WeaponTypes[i] || ChildType.IsSubclassOf(WeaponTypes[i]))
                {
                    m_Weapons[i] = ChildWeapon;
                    break;
                }
            }
        }

        SwitchWeapon(0);
    }

    private void UseBuffs(BuffMultipliers Buffs)
    {
        foreach (var Weapon in m_Weapons)
        {
            Weapon.Initialize(Buffs);
        }
    }
}

