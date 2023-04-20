using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PauseWidget : UIWidget
{
    [SerializeField] private Image m_FadeImage;
    private BehaviorComponent m_BehaviorComponent;

    public override void Initialize()
    {
        base.Initialize();

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
    }

    public override void OnShow()
    {
        base.OnShow();

        Assert.IsNotNull(m_FadeImage);
        m_FadeImage.color = Color.clear;
        m_FadeImage.raycastTarget = false;

        Time.timeScale = 0f;
        PlayerState.Instance.PlayerShip.bProcessInput = false;
    }

    public override void OnHide()
    {
        base.OnHide();

        Time.timeScale = 1f;
        PlayerState.Instance.PlayerShip.bProcessInput = true;
    }

    public void OnContinueClicked()
    {
        UIService.Instance.HideWidget(this);
    }

    public void OnMenuClicked()
    {
        m_FadeImage.raycastTarget = true;
        m_BehaviorComponent.AddAction(new BHUIAction_FadeImage(m_FadeImage)
            .AddOnActionFinished((_) =>
            {
                GameStateMachine.Instance.SwitchState(new MenuGameState());
            })
        );
    }

    public void OnMuteClicked()
    {
        AudioService.Instance.Mute(!AudioService.Instance.bMuted);
    }
}
