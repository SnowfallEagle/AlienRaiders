using UnityEngine;

// @TODO: I think back rotation must be interpolated between start position of second part and destination

public class BHPlayerAction_CinematicMoveWithRotation : BHAction
{
    private Vector3 m_Destination;
    private float m_Speed = 0f;
    private float m_MaxAcceleratedSpeed;
    private float m_MaxDeceleratedSpeed;
    private float m_Acceleration;

    private float m_MaxAngle = 30f;
    private float m_Angle = 0f;
    private float m_RotationSpeed = 5f * Time.fixedDeltaTime;
    private bool m_bStartedFromRight;

    private float m_FirstPart;
    private float m_SecondPartDistance;
    private bool m_bMovingFirstPart = true;

    /** Params:
            FirstPart = (0f;1f)
    */
    public BHPlayerAction_CinematicMoveWithRotation(Vector3 Destination, float MaxSpeed = 5f, float Acceleration = 0.05f, float FirstPart = 0.8f)
    {
        m_bFixedUpdate = true;

        m_Destination = Destination;
        m_MaxAcceleratedSpeed = MaxSpeed;
        m_MaxDeceleratedSpeed = MaxSpeed * 0.5f;
        m_Acceleration = Acceleration;

        m_FirstPart = FirstPart;
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

        m_SecondPartDistance = (m_Destination - CurrentPosition).sqrMagnitude * (1f - m_FirstPart);
        return true;
    }

    public override bool Update()
    {
        Vector3 CurrentPosition = m_Owner.transform.position;

        Vector3 RemainingPath = m_Destination - CurrentPosition;
        float Distance = RemainingPath.sqrMagnitude;

        if (m_bMovingFirstPart)
        {
            m_Speed = System.MathF.Min(m_Speed + m_Acceleration, m_MaxAcceleratedSpeed);
        }
        else
        {
            m_Speed = System.MathF.Max(m_Speed - m_Acceleration, m_MaxDeceleratedSpeed);
        }

        Vector3 Step = RemainingPath.normalized * (m_Speed * Time.fixedDeltaTime);
        float StepLength = Step.sqrMagnitude;

        if (StepLength < Distance)
        {
            m_Owner.transform.position += Step;

            if (m_bMovingFirstPart && Distance <= m_SecondPartDistance)
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
                    if (m_Angle < 0f) // Angle < 0f -> ship moves right
                    {
                        m_Angle = 0f;
                    }
                }
                else
                {
                    if (m_Angle > 0f) // Angle > 0f -> ship moves left
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
