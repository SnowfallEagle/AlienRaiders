using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class FightWidget : UIWidget
{
    [SerializeField] private Image m_FadeImage;
    private bool m_bFadingIn = false;

    [SerializeField] private GameObject m_PauseButton;

    private BehaviorComponent m_BehaviorComponent;

    protected override void Start()
    {
        base.Start();

        Assert.IsNotNull(m_FadeImage);
        Assert.IsNotNull(m_PauseButton);

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
    }

    public override void OnShow()
    {
        base.OnShow();

        if (m_bFadingIn)
        {
            // Reset for the next time
            m_bFadingIn = false;
        }
        else
        {
            m_FadeImage.raycastTarget = false;
            m_FadeImage.color = Color.clear;
        }

        ShowPauseButton();
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

    private void ShowPauseButton()
    {
        m_PauseButton.SetActive(true);
    }

    public void HidePauseButton()
    {
        m_PauseButton.SetActive(false);
    }
}
