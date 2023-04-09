using UnityEngine;

public class BHShipAction_AnimateSpriteColor : BHAction
{
    private Color m_DesiredColor;
    private Color m_SavedColor;

    private float m_Duration;
    private float m_Elapsed;

    private bool m_bPulse;
    private bool m_bLoop;

    private SpriteRenderer m_SpriteRenderer;

    /** bLoop worls only with bPulse = true */
    public BHShipAction_AnimateSpriteColor(Color Desired, float Duration = 1f, float StartTime = 0f, bool bPulse = false, bool bLoop = false)
    {
        m_DesiredColor = Desired;

        m_Duration = Duration;
        m_Elapsed = StartTime;

        m_bPulse = bPulse;
        m_bLoop = bLoop;
    }

    public override bool Start()
    {
        m_SpriteRenderer = m_Owner.GetComponent<SpriteRenderer>();
        m_SavedColor = m_SpriteRenderer.color;
        return true;
    }

    public override bool Update()
    {
        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > m_Duration)
        {
            m_SpriteRenderer.color = m_DesiredColor;

            if (m_bPulse)
            {
                m_Owner.AddAction(new BHShipAction_AnimateSpriteColor(m_SavedColor, m_Duration, bPulse: m_bLoop, bLoop: m_bLoop));
            }
            return false;
        }

        m_SpriteRenderer.color = Color.Lerp(m_SavedColor, m_DesiredColor, m_Elapsed / m_Duration);
        return true;
    }
}
