using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelEndedWidget : UIWidget
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

        m_FadeImage.color = Color.clear;
        m_FadeImage.raycastTarget = false;
    }

    public void OnNextLevelClicked()
    {
        m_FadeImage.raycastTarget = true;

        m_BehaviorComponent.AddExclusiveAction(new BHUIAction_FadeImage(m_FadeImage)
            .AddOnActionFinished((_) =>
            {
                GameStateMachine.Instance.SwitchState(new FightGameState(bFadeIn: true));
                PlayerState.Instance.PlayerShip.Revive(false);
            })
        );
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
