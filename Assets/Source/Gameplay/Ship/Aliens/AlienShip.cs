using System;
using UnityEngine;

public class AlienShip : Ship
{
    private enum Weapons
    {
        AnyWeapon,

        MaxWeapons
    }

    public static class Pattern
    {
        public const int MoveBottom    = 0;
        public const int MoveLeftRight = 1;
        public const int MoveZigZag    = 2; // @INCOMPLETE
    }

    [SerializeField] protected float m_DamageOnCollide = 10f;
    public float DamageOnCollide => m_DamageOnCollide;

    [NonSerialized] public int BehaviorPattern = Pattern.MoveBottom;

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        Vector3 Position = transform.position;
        Position.z = WorldZLayers.Alien;
        transform.position = Position;

        gameObject.layer = LayerMask.NameToLayer("Alien");
        m_Team = ShipTeam.Enemy;

        BoxCollider.isTrigger = true;

        HealthComponent.OnDied += () => { Destroy(gameObject); };

        BehaviorComponent.StartBehavior(GetBehaviorTree(BehaviorPattern));
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        var PlayerShip = Other.GetComponent<PlayerShip>();
        if (PlayerShip)
        {
            PlayerShip.HealthComponent.TakeDamage(m_DamageOnCollide);
        }
    }

    protected override Type[] OnPreInitializeWeapons()
    {
        Type[] WeaponTypes = new Type[(int)Weapons.MaxWeapons];
        WeaponTypes[(int)Weapons.AnyWeapon] = typeof(Weapon);
        return WeaponTypes;
    }

    public virtual BHActionNode GetBehaviorTree(int BehaviorPattern)
    {
        const float Fov = 135f;
        const float MoveLeftRightTimeLimit = 2.5f;

        switch (BehaviorPattern)
        {
            case Pattern.MoveBottom:
                return new BHFlow_Sequence()
                    .AddNode(new BHTask_LoopCommand(new BHCommand_MoveForward(Speed)))

                    .AddService(new BHShipService_DestroyWhenOutOfBottomBound())
                    .AddService(new BHShipService_FireWhenSeePlayer(Fov));

            /* @INCOMPLETE:
                - Remove previous code, put task execution stuff in special method Process
                - Check if we lose frames when done
            */
            case Pattern.MoveLeftRight:
                return new BHFlow_Selector()
                    .AddNode(new BHFlow_Sequence()
                        .AddNode(new BHTask_ProcessCommand(new BHCommand_DirectionalMove(transform.up, Speed)))
                        .AddNode(new BHTask_ProcessCommand(new BHCommand_DirectionalMove(-transform.right, Speed * 2f)))

                        // @INCOMPLETE AddDecorator(new BHDecorator_Loop())
                        .AddDecorator(new BHDecorator_TimeLimit(MoveLeftRightTimeLimit))
                    )

                    .AddNode(new BHFlow_Sequence()
                        .AddNode(new BHTask_ProcessCommand(new BHCommand_DirectionalMove(transform.up, Speed)))
                        .AddNode(new BHTask_ProcessCommand(new BHCommand_DirectionalMove(transform.right, Speed * 2f)))

                        // @INCOMPLETE AddDecorator(new BHDecorator_Loop())
                        .AddDecorator(new BHDecorator_TimeLimit(MoveLeftRightTimeLimit))
                    )

                    .AddService(new BHShipService_DestroyWhenOutOfBottomBound())
                    .AddService(new BHShipService_FireWhenSeePlayer(Fov));

            default:
                NoEntry.Assert($"No pattern { BehaviorPattern }");
                return null;
        }
    }
}
