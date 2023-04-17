using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : CustomBehavior
{
    [SerializeField] protected float m_Speed = 5f;

    private BehaviorComponent m_BehaviorComponent;
    public BehaviorComponent BehaviorComponent => m_BehaviorComponent;

    private void Start()
    {
        Vector3 Position = transform.position;
        Position.z = WorldZLayers.Pickup;
        transform.position = Position;

        var Rigidbody = InitializeComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0f;

        InitializeComponent<BoxCollider2D>();
        InitializeComponent<SpriteRenderer>();

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
        m_BehaviorComponent.StartBehavior(new BHTask_LoopCommand(new BHCommand_MoveForward(-m_Speed)));

        // @TODO: We should check for level boundaries instead
        Destroy(gameObject, 10f);
    }

    private void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.GetComponent<PlayerShip>() is var Ship)
        {
            if (GivePickup(Ship))
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual bool GivePickup(PlayerShip Ship)
    {
        return true;
    }
}
