using UnityEngine;

public class BHShipTask_AnimateSpriteColor : BHTask
{
    private Color m_DesiredColor;
    private Color m_SavedColor;

    private float m_Duration;
    private float m_Elapsed;

    private bool m_bPulse;
    private bool m_bLoop;

    private SpriteRenderer m_SpriteRenderer;
    private BehaviorComponent m_BehaviorComponent;

    /** bLoop worls only with bPulse = true
    */
    public BHShipTask_AnimateSpriteColor(Color Desired, float Duration = 1f, float StartTime = 0f, bool bPulse = false, bool bLoop = false)
    {
        m_DesiredColor = Desired;

        m_Duration = Duration;
        m_Elapsed = StartTime;

        m_bPulse = bPulse;
        m_bLoop = bLoop;
    }

    public override void Start()
    {
        m_SpriteRenderer = m_Owner.GetComponent<SpriteRenderer>();
        m_BehaviorComponent = m_Owner.GetComponent<BehaviorComponent>();
        m_SavedColor = m_SpriteRenderer.color;
    }

    public override void Update()
    {
        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > m_Duration)
        {
            m_SpriteRenderer.color = m_DesiredColor;
            State = TaskState.Done;

            if (m_bPulse)
            {
                m_BehaviorComponent.AddTask(new BHShipTask_AnimateSpriteColor(m_SavedColor, m_Duration, bPulse: m_bLoop, bLoop: m_bLoop));
            }
            return;
        }

        m_SpriteRenderer.color = Color.Lerp(m_SavedColor, m_DesiredColor, m_Elapsed / m_Duration);
    }
}
