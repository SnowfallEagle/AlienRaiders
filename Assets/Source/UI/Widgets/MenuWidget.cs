using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MenuWidget : UIWidget
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
        m_FadeImage.color = Color.black;
        m_FadeImage.raycastTarget = false;

        m_BehaviorComponent.AddAction(new BHUIAction_FadeImage(m_FadeImage, false, 0.5f));
    }

    public void OnPlayClicked()
    {
        GameStateMachine.Instance.SwitchState(new FightGameState());

        var PlayerShip = PlayerState.Instance.PlayerShip;
        PlayerShip.BehaviorComponent.AddAction(
            new BHPlayerAction_CinematicMove(new Vector3(0f, -2f), Acceleration: 0.25f, Deceleration: 0.075f, MaxAngle: 1f, FirstPart: 0.65f)
                .AddOnActionFinished((_) =>
                {
                    PlayerShip.bProcessInput = true;
                    PlayerShip.bCheckBounds = true;
                })
        );
    }

    public void OnMuteClicked()
    {
        AudioService.Instance.Mute(!AudioService.Instance.bMuted);
    }
}
