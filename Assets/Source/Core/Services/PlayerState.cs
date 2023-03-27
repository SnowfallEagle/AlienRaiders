using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Service<PlayerState>
{
    // NOTE: Can be null!
    private PlayerShip m_PlayerShip;
    public PlayerShip PlayerShip
    {
        get
        {
            if (!m_PlayerShip)
            {
                m_PlayerShip = FindObjectOfType<PlayerShip>();
            }
            return m_PlayerShip;
        }
    }
}
