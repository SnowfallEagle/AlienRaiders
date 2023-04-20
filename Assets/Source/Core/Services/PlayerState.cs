using System;
using UnityEngine;

public class PlayerState : Service<PlayerState>
{
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

    [Header("Position")]
    /** Should be right relative to ReadyPosition, so we can revive from random position */
    [SerializeField] private Vector3 m_RevivePosition            = new Vector3(5f, -12.5f);
    public Vector3 RevivePosition => m_RevivePosition;

    [SerializeField] private Vector3 m_ReadyPosition             = new Vector3(0f, -7.5f);
    public Vector3 ReadyPosition => m_ReadyPosition;

    [SerializeField] private Vector3 m_LeftCruisePosition        = new Vector3(-2.5f, -7.5f);
    public Vector3 LeftCruisePosition => m_LeftCruisePosition;

    [SerializeField] private Vector3 m_RightCruisePosition       = new Vector3(2.5f, -7.5f);
    public Vector3 RightCruisePosition => m_RightCruisePosition;

    [SerializeField] private Vector3 m_FlyAroundDiff             = new Vector3(2.5f, 0f);
    public Vector3 FlyAroundDiff => m_FlyAroundDiff;

    [SerializeField] private Vector3 m_FlewAwayPosition          = new Vector3(0f, 50f);
    public Vector3 FlewAwayPosition => m_FlewAwayPosition;

    [SerializeField] private Vector3 m_PrepareToFlewAwayPosition = Vector3.zero;
    public Vector3 PrepareToFlewAwayPosition => m_PrepareToFlewAwayPosition;

    // @TODO: We need to load and save data somewhere using sdk...
    [NonSerialized] public int Level = 0;

    public PlayerState()
    {
        m_RevivePosition.z            = WorldZLayers.Player;
        m_ReadyPosition.z             = WorldZLayers.Player;
        m_LeftCruisePosition.z        = WorldZLayers.Player;
        m_RightCruisePosition.z       = WorldZLayers.Player;
        m_FlyAroundDiff.z             = WorldZLayers.Player;
        m_FlewAwayPosition.z          = WorldZLayers.Player;
        m_PrepareToFlewAwayPosition.z = WorldZLayers.Player;
    }

    public void SpawnShip()
    {
        if (m_PlayerShip)
        {
            Destroy(m_PlayerShip.gameObject);
        }

        m_PlayerShip = Instantiate(Resources.Load<GameObject>("Player/Player")).GetComponent<PlayerShip>();
        m_PlayerShip.Initialize(new BuffMultipliers());
    }
}
