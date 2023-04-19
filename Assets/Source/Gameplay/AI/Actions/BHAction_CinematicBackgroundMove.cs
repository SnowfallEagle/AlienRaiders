using UnityEngine;

// @TODO: We need to affect star speed...

public class BHAction_CinematicBackgroundMove : BHAction
{
    enum MoveState
    {
        Accelerate,
        Delay,
        Decelerate
    }

    private MoveState m_State = MoveState.Accelerate;

    private float m_SavedBackgroundSpeed;
    private float m_SavedMovingEffectSpeed;
    private float m_SavedFarStarSpeed;
    private float m_SavedNearStarSpeed;

    private float m_MaxBackgroundSpeed;
    private float m_Acceleration;
    private float m_Deceleration;

    private float m_DelayBetweenAccelerations;
    private float m_Elapsed = 0f;

    public BHAction_CinematicBackgroundMove(float Acceleration = 1f, float Deceleration = 0.1f, float MaxSpeed = 25f, float DelayBetweenAccelerations = 5f)
    {
        m_MaxBackgroundSpeed = MaxSpeed;

        m_Acceleration = Acceleration;
        m_Deceleration = Deceleration;

        m_DelayBetweenAccelerations = DelayBetweenAccelerations;
    }

    public override bool Start()
    {
        m_SavedBackgroundSpeed   = RenderingService.Instance.BackgroundVelocityY;
        m_SavedMovingEffectSpeed = RenderingService.Instance.MovingEffectVelocityY;
        m_SavedFarStarSpeed      = RenderingService.Instance.FarStarVelocityY;
        m_SavedNearStarSpeed     = RenderingService.Instance.NearStarVelocityY;
        return true;
    }

    public override bool Update()
    {
        switch (m_State)
        {
            case MoveState.Accelerate:
            {
                float NewBackgroundSpeed = RenderingService.Instance.BackgroundVelocityY += m_Acceleration;
                RenderingService.Instance.MovingEffectVelocityY += m_Acceleration;
                RenderingService.Instance.FarStarVelocityY      += m_Acceleration;
                RenderingService.Instance.NearStarVelocityY     += m_Acceleration;

                if (NewBackgroundSpeed >= m_MaxBackgroundSpeed)
                {
                    m_State = MoveState.Delay;
                }
            } break;

            case MoveState.Delay:
            {
                m_Elapsed += Time.deltaTime;

                if (m_Elapsed >= m_DelayBetweenAccelerations)
                {
                    m_State = MoveState.Decelerate;
                }
            } break;

            case MoveState.Decelerate:
            {
                float NewBackgroundSpeed = RenderingService.Instance.BackgroundVelocityY -= m_Deceleration;
                RenderingService.Instance.MovingEffectVelocityY -= m_Deceleration;
                RenderingService.Instance.FarStarVelocityY      -= m_Deceleration;
                RenderingService.Instance.NearStarVelocityY     -= m_Deceleration;

                if (NewBackgroundSpeed <= m_SavedBackgroundSpeed)
                {
                    RestoreSpeeds();
                    return false;
                }
            } break;
        }

        return true;
    }

    public override void OnAbort()
    {
        base.OnAbort();

        RestoreSpeeds();
    }

    private void RestoreSpeeds()
    {
        RenderingService.Instance.BackgroundVelocityY   = m_SavedBackgroundSpeed;
        RenderingService.Instance.MovingEffectVelocityY = m_SavedMovingEffectSpeed;
        RenderingService.Instance.FarStarVelocityY      = m_SavedFarStarSpeed;
        RenderingService.Instance.NearStarVelocityY     = m_SavedNearStarSpeed;
    }
}
