using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class FightWidget : UIWidget
{
    [SerializeField] private Image m_FadeImage;
    private bool m_bFadingIn = false;

    private BehaviorComponent m_BehaviorComponent;

    protected override void Start()
    {
        base.Start();

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
    }

    public override void OnShow()
    {
        base.OnShow();

        Assert.IsNotNull(m_FadeImage);

        if (m_bFadingIn)
        {
            // Reset for the next time
            m_bFadingIn = false;
            return;
        }

        m_FadeImage.raycastTarget = false;
        m_FadeImage.color = Color.clear;
    }

    public void OnPauseClicked()
    {
        UIService.Instance.ShowWidget<PauseWidget>();
    }

    public void FadeIn()
    {
        m_FadeImage.color = Color.black;
        m_BehaviorComponent.AddExclusiveAction(new BHUIAction_FadeImage(m_FadeImage, false, 0.5f));
        m_bFadingIn = true;
    }
}
