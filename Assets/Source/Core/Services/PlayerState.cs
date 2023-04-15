using UnityEngine;

public class PlayerState : Service<PlayerState>
{
    // @NOTE: Can be null!
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

    // @TODO: We need to load and save data somewhere using sdk...
    public int Level = 0;

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
