using UnityEngine;
using UnityEngine.Assertions;

public class LauncherWeapon : Weapon
{
    [SerializeField] protected Projectile m_Projectile;

    protected override void Fire()
    {
        Projectile Projectile = SpawnInState(m_Projectile);
        Projectile.transform.position = transform.position;
        Projectile.transform.rotation = transform.rotation;

        Projectile.Initialize(m_Owner, m_Buffs);
    }
}
