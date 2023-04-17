using UnityEngine;

// @TODO: Acceleration, Deceleration

public class BHPlayerAction_CinematicMoveWithRotation : BHAction
{
    private Vector3 m_Destination;
    private float m_Speed;

    private float m_MaxAngle = 30f;
    private float m_Angle = 0f;
    private float m_RotationSpeed = 5f * Time.fixedDeltaTime;
    private bool m_bStartedFromRight;

    private float m_FirstPartDistance;
    private bool m_bMovingFirstPart = true;

    public BHPlayerAction_CinematicMoveWithRotation(Vector3 Destination, float Speed = 5f)
    {
        m_bFixedUpdate = true;

        m_Destination = Destination;
        m_Speed = Speed * Time.fixedDeltaTime;
    }

    public override bool Start()
    {
        Vector3 CurrentPosition = m_Owner.transform.position;
        if (CurrentPosition.x > m_Destination.x)
        {
            m_bStartedFromRight = true;
        }
        else
        {
            CurrentPosition.x = -CurrentPosition.x;
            m_MaxAngle        = -m_MaxAngle;
            m_RotationSpeed   = -m_RotationSpeed;

            m_bStartedFromRight = false;
        }

        m_FirstPartDistance = (m_Destination - CurrentPosition).sqrMagnitude * 0.25f;
        return true;
    }

    public override bool Update()
    {
        Vector3 CurrentPosition = m_Owner.transform.position;

        Vector3 RemainingPath = m_Destination - CurrentPosition;
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

        m_Owner.transform.position = m_Destination;
        m_Owner.transform.rotation = Quaternion.identity;
        return false;
    }
}
