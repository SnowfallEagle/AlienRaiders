using UnityEngine;

public class MenuWidget : UIWidget
{
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
