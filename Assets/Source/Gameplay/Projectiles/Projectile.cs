using UnityEngine;

public class Projectile : CustomBehavior
{
    [SerializeField] protected float m_DefaultSpeed = 5f;
    [SerializeField] protected float m_DefaultDamage = 5f;
    [SerializeField] protected float m_LifeTime = 5f;

    protected BehaviorComponent m_BehaviorComponent;
    public BehaviorComponent BehaviorComponent => m_BehaviorComponent;

    protected float m_Damage = 5f;
    protected float m_Speed = 5f;

    protected Ship m_Owner;

    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");

        var Rigidbody = InitializeComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0f;

        var BoxCollider = InitializeComponent<BoxCollider2D>();
        BoxCollider.isTrigger = true;

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
        m_BehaviorComponent.Initialize(this);

        Destroy(gameObject, m_LifeTime);
    }

    public void Initialize(Ship Owner, BuffMultipliers Buffs)
    {
        m_Owner = Owner;

        m_Speed = m_DefaultSpeed * Buffs.ProjectileSpeed;
        m_Damage = m_DefaultDamage * Buffs.ProjectileDamage;
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        Ship Ship = Other.GetComponent<Ship>();
        if (Ship && Ship.ShipTeam != m_Owner.ShipTeam)
        {
            // TODO: Maybe spawn effect?
            Ship.HealthComponent.TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
