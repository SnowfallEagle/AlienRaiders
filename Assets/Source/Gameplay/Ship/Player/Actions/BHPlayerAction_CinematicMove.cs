using UnityEngine;

public class BHPlayerAction_CinematicMove : BHAction
{
    private Vector3 m_Destination;

    private float m_Speed = 0f;
    private float m_MaxAcceleratedSpeed;
    private float m_MaxDeceleratedSpeed;
    private float m_Acceleration;

    private float m_MaxAngle;
    private float m_Angle = 0f;
    private float m_AngleOnStartingSecondPart;

    private float m_MaxRotationSpeed;
    private float m_RotationSign = 1f;

    private float m_FirstPart;
    private float m_SecondPartDistance;
    private bool m_bMovingFirstPart = true;

    /** Params:
            FirstPart = (0f;1f)
    */
    public BHPlayerAction_CinematicMove(Vector3 Destination, float MaxSpeed = 5f, float Acceleration = 0.10f, float MaxAngle = 45f, float MaxRotationSpeed = 10f, float FirstPart = 0.75f)
    {
        m_bFixedUpdate = true;

        m_Destination = Destination;

        m_MaxAcceleratedSpeed = MaxSpeed;
        m_MaxDeceleratedSpeed = MaxSpeed * 0.5f;
        m_Acceleration = Acceleration;

        m_MaxRotationSpeed = MaxRotationSpeed;
        m_MaxAngle = MaxAngle;

        m_FirstPart = FirstPart;
    }

    public override bool Start()
    {
        Vector3 CurrentPosition = m_Owner.transform.position;
        m_Destination.z = CurrentPosition.z;

        if (CurrentPosition.x < m_Destination.x)
        {
            CurrentPosition.x = -CurrentPosition.x;
            m_MaxAngle        = -m_MaxAngle;
            m_RotationSign    = -m_RotationSign;
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

        if (StepLength < Distance && StepLength > 0.00000001f)
        {
            m_Owner.transform.position += Step;

            if (m_bMovingFirstPart && Distance <= m_SecondPartDistance)
            {
                m_RotationSign = -m_RotationSign;
                m_AngleOnStartingSecondPart = m_Angle;
                m_bMovingFirstPart = false;
            }

            if (m_bMovingFirstPart)
            {
                m_Angle += Mathf.Lerp(0f, m_MaxRotationSpeed, m_Speed / m_MaxAcceleratedSpeed) * m_RotationSign * Time.fixedDeltaTime;

                if (System.MathF.Abs(m_Angle) > System.MathF.Abs(m_MaxAngle))
                {
                    m_Angle = m_MaxAngle;
                }
            }
            else
            {
                // Less distance -> angle closer to 0
                // @TODO: Debug it
                m_Angle = Mathf.Lerp(0f, m_AngleOnStartingSecondPart, Distance / m_SecondPartDistance);
            }

            m_Owner.transform.rotation = Quaternion.Euler(0f, 0f, m_Angle);
            return true;
        }

        m_Owner.transform.position = m_Destination;
        m_Owner.transform.rotation = Quaternion.identity;
        return false;
    }
}
