using UnityEngine;

public class PauseWidget : UIWidget
{
    public override void OnShow()
    {
        base.OnShow();

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
        GameStateMachine.Instance.SwitchState(new MenuGameState());
    }

    public void OnMuteClicked()
    {
        AudioService.Instance.Mute(!AudioService.Instance.bMuted);
    }
}
