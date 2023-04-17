using UnityEngine;

public class BHAction_AnimateSpriteColor : BHAction
{
    private SpriteRenderer m_SpriteRenderer;

    private Color m_SavedColor;
    private Color m_DesiredColor;
    private Color m_StartColor;

    private float m_Duration;
    private float m_Elapsed;

    private bool m_bPulse;
    private bool m_bLoop;

    /** bLoop worls only with bPulse = true */
    public BHAction_AnimateSpriteColor(SpriteRenderer SpriteRenderer, Color Desired, float Duration = 1f, bool bPulse = false, bool bLoop = false)
    {
        m_SpriteRenderer = SpriteRenderer;

        m_DesiredColor = Desired;
        m_Duration = Duration;

        m_bPulse = bPulse;
        m_bLoop = bLoop;
    }

    public override bool Start()
    {
        m_SavedColor = m_SpriteRenderer.color;
        m_StartColor = m_SavedColor;

        m_Elapsed = 0.0f;
        return true;
    }

    private void StartAnimation()
    {
    }

    public override bool Update()
    {
        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > m_Duration)
        {
            if (!m_bPulse)
            {
                m_SpriteRenderer.color = m_DesiredColor;
                return false;
            }

            // Restart animation
            Color Temp     = m_StartColor;
            m_StartColor   = m_DesiredColor;
            m_DesiredColor = Temp;

            m_Elapsed = 0f;
            m_bPulse = m_bLoop; // Play animation last time if m_bLoop = false
        }

        m_SpriteRenderer.color = Color.Lerp(m_StartColor, m_DesiredColor, m_Elapsed / m_Duration);
        return true;
    }

    public override void OnAbort()
    {
        base.OnAbort();

        m_SpriteRenderer.color = m_SavedColor;
    }
}
