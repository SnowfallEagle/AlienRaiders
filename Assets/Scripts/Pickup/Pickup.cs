using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : CustomBehaviour
{
    [SerializeField] protected float m_Speed = 2.5f;

    private void Start()
    {
        var Rigidbody = InitializeComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0f;

        InitializeComponent<BoxCollider2D>();
        
        // We should check for level boundaries
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.Translate(0f, -m_Speed * Time.deltaTime, 0f);
    }

    private void OnTriggerStay2D(Collider2D Other)
    {
        Ship Ship = Other.GetComponent<Ship>();
        if (Ship && Ship.ShipTeam == Ship.Team.Player)
        {
            if (GivePickup(Ship))
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual bool GivePickup(Ship Ship)
    {
        return false;
    }
}
