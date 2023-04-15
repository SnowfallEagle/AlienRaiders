using UnityEngine;

public class BHShipAction_AnimateSpriteColor : BHAction
{
    private Color m_SavedColor;
    private Color m_DesiredColor;
    private Color m_CurrentColor;

    private float m_Duration;
    private float m_Elapsed;

    private bool m_bPulse;
    private bool m_bLoop;

    private SpriteRenderer m_SpriteRenderer;

    /** bLoop worls only with bPulse = true */
    public BHShipAction_AnimateSpriteColor(Color Desired, float Duration = 1f, bool bPulse = false, bool bLoop = false)
    {
        m_DesiredColor = Desired;
        m_Duration = Duration;

        m_bPulse = bPulse;
        m_bLoop = bLoop;
    }

    public override bool Start()
    {
        m_SpriteRenderer = m_Owner.GetComponent<SpriteRenderer>();
        m_SavedColor = m_SpriteRenderer.color;

        StartAnimation(m_SavedColor, m_DesiredColor);
        return true;
    }

    private void StartAnimation(Color From, Color To)
    {
        m_Elapsed = 0.0f;
        m_CurrentColor = From;
        m_DesiredColor = To;

        m_SpriteRenderer.color = From;
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

            StartAnimation(m_DesiredColor, m_SavedColor);
            m_bPulse = m_bLoop; // Play animation last time if m_bLoop = false
        }

        m_SpriteRenderer.color = Color.Lerp(m_CurrentColor, m_DesiredColor, m_Elapsed / m_Duration);
        return true;
    }

    public override void OnAbort()
    {
        base.OnAbort();

        m_SpriteRenderer.color = m_SavedColor;
    }
}
