using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DeathWidget : UIWidget
{
    [SerializeField] private Image m_FadeImage;
    private BehaviorComponent m_BehaviorComponent;

    public override void Initialize()
    {
        base.Initialize();

        Assert.IsNotNull(m_FadeImage);
        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
    }

    public override void OnShow()
    {
        base.OnShow();

        Time.timeScale = 0f;
        PlayerState.Instance.PlayerShip.bProcessInput = false;

        m_FadeImage.color = Color.clear;
        m_FadeImage.raycastTarget = false;
    }

    public override void OnHide()
    {
        base.OnHide();

        Time.timeScale = 1f;
    }

    public void OnWatchAdClicked()
    {
        PlayerState.Instance.PlayerShip.Revive();
        UIService.Instance.HideWidget(this);
        NotImplemented.Assert();
    }

    public void OnBuyLifeClicked()
    {
        PlayerState.Instance.PlayerShip.Revive();
        UIService.Instance.HideWidget(this);
        NotImplemented.Assert();
    }

    public void OnMenuClicked()
    {
        m_FadeImage.raycastTarget = true;

        m_BehaviorComponent.AddExclusiveAction(new BHUIAction_FadeImage(m_FadeImage)
            .AddOnActionFinished((_) =>
            {
                GameStateMachine.Instance.SwitchState(new MenuGameState());
            })
        );
    }
}
