using UnityEngine;

public class BHPlayerAction_MoveFromReviveToReady : BHAction
{
    private Vector3 m_ReadyPosition;
    private float m_Speed;

    private float m_MaxAngle = 30f;
    private float m_Angle = 0f;
    private float m_RotationSpeed = 5f * Time.fixedDeltaTime;
    private bool m_bStartedFromRight;

    private float m_FirstPartDistance;
    private bool m_bMovingFirstPart = true;

    public BHPlayerAction_MoveFromReviveToReady()
    {
        m_bFixedUpdate = true;
    }

    public override bool Start()
    {
        var PlayerShip = m_Owner.GetComponent<PlayerShip>();

        m_ReadyPosition = PlayerShip.ReadyPosition;
        m_Speed = PlayerShip.Speed * Time.fixedDeltaTime;

        Vector3 CurrentPosition = PlayerShip.RevivePosition;
        if (Random.Range(0, 2) == 1)
        {
            CurrentPosition.x = -CurrentPosition.x;
            m_MaxAngle        = -m_MaxAngle;
            m_RotationSpeed   = -m_RotationSpeed;

            m_bStartedFromRight = false;
        }
        else
        {
            m_bStartedFromRight = true;
        }

        PlayerShip.transform.position = CurrentPosition;
        m_FirstPartDistance = (m_ReadyPosition - CurrentPosition).sqrMagnitude * 0.25f;
        return true;
    }

    public override bool Update()
    {
        Vector3 CurrentPosition = m_Owner.transform.position;

        Vector3 RemainingPath = m_ReadyPosition - CurrentPosition;
        float Distance = RemainingPath.sqrMagnitude;

        Vector3 Step = RemainingPath.normalized * m_Speed; // m_Speed already multiplied by fixedDeltaTime
        float StepLength = Step.sqrMagnitude;

        if (StepLength < Distance)
        {
            m_Owner.transform.position += Step;

            if (m_bMovingFirstPart && Distance <= m_FirstPartDistance)
            {
                m_RotationSpeed = -m_RotationSpeed;
                m_bMovingFirstPart = false;
            }

            m_Angle += m_RotationSpeed; // m_RotationSpeed already multiplied by fixedDeltaTime
            if (m_bMovingFirstPart)
            {
                if (System.MathF.Abs(m_Angle) > System.MathF.Abs(m_MaxAngle))
                {
                    m_Angle = m_MaxAngle;
                }
            }
            else
            {
                if (m_bStartedFromRight)
                {
                    // Angle < 0f -> ship moves right
                    if (m_Angle < 0f)
                    {
                        m_Angle = 0f;
                    }
                }
                else
                {
                    // Angle > 0f -> ship moves left
                    if (m_Angle > 0f)
                    {
                        m_Angle = 0f;
                    }
                }
            }

            m_Owner.transform.rotation = Quaternion.Euler(0f, 0f, m_Angle);
            return true;
        }

        m_Owner.transform.position = m_ReadyPosition;
        m_Owner.transform.rotation = Quaternion.identity;
        return false;
    }
}
