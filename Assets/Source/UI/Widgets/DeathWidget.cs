using UnityEngine;

public class DeathWidget : UIWidget
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
        GameStateMachine.Instance.SwitchState(new MenuGameState());
    }
}
