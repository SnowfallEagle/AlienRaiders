using UnityEngine;

public class Projectile : CustomBehavior
{
    [SerializeField] protected float m_DefaultSpeed = 5f;
    [SerializeField] protected float m_DefaultDamage = 5f;
    [SerializeField] protected float m_LifeTime = 15f;

    private BehaviorComponent m_BehaviorComponent;
    public BehaviorComponent BehaviorComponent => m_BehaviorComponent;

    private float m_Damage = 5f;
    public float Damage => m_Damage;

    private float m_Speed = 5f;
    public float Speed => m_Speed;

    protected Ship m_Owner;

    protected virtual void Start()
    {
        Vector3 Position = transform.position;
        Position.z = m_Owner.Team == Ship.ShipTeam.Player ? WorldZLayers.ProjectilePlayer : WorldZLayers.ProjectileAlien;
        transform.position = Position;

        gameObject.layer = LayerMask.NameToLayer("Projectile");

        var Rigidbody = InitializeComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0f;

        var BoxCollider = InitializeComponent<BoxCollider2D>();
        BoxCollider.isTrigger = true;

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
        // @INCOMPLETE: We need to check bounds

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
        if (Ship && Ship.Team != m_Owner.Team)
        {
            Ship.HealthComponent.TakeDamage(m_Damage);
            // @TODO: Maybe spawn effect
            Destroy(gameObject);
        }
    }
}
