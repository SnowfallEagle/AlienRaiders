using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Pickup : MonoBehaviour
{
    [SerializeField] protected float m_Speed = 2.5f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0f;

        // TODO: Later we need smth like services for level stuff like level bounds and ad service, but for now just destroy by time
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
