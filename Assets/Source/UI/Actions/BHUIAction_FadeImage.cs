using UnityEngine;
using UnityEngine.UI;

public class BHUIAction_FadeImage : BHAction
{
    private Image m_Image;
    private Color m_Color;

    private bool m_bFadeOut;
    private float m_Speed;

    public BHUIAction_FadeImage(Image Image, bool bFadeOut = true, float Speed = 1f)
    {
        m_Image = Image;
        m_Color = m_Image.color;

        m_bFadeOut = bFadeOut;
        m_Speed = Speed;
    }

    public override bool Update()
    {
        bool bRes = true;

        if (m_bFadeOut)
        {
            m_Color.a += m_Speed * Time.unscaledDeltaTime;
            if (m_Color.a >= 1f)
            {
                m_Color.a = 1f;
                bRes = false;
            }
        }
        else
        {
            m_Color.a -= m_Speed * Time.unscaledDeltaTime;
            if (m_Color.a <= 0f)
            {
                m_Color.a = 0f;
                bRes = false;
            }
        }

        m_Image.color = m_Color;
        return bRes;
    }
}
