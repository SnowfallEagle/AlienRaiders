using UnityEngine;
using UnityEngine.Assertions;

public class TripleLauncherWeapon : Weapon
{
    private const int NumSockets = 3;

    [SerializeField] protected Projectile m_Projectile;
    private Transform[] m_Sockets = new Transform[NumSockets];

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        Assert.IsTrue(transform.childCount == NumSockets);
        for (int i = 0; i < NumSockets; ++i)
        {
            m_Sockets[i] = transform.GetChild(i);
        }
    }

    protected override void Fire()
    {
        for (int i = 0; i < NumSockets; ++i)
        {
            Projectile Projectile = SpawnInState(m_Projectile);
            Projectile.transform.position = m_Sockets[i].position;
            Projectile.transform.rotation = m_Sockets[i].rotation;

            Projectile.Initialize(m_Owner, m_Buffs);
        }
    }
}
