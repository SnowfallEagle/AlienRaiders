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
        PlayerState.Instance.PlayerShip.StartRevive();
    }

    public void OnWatchAdClicked()
    {
        UIService.Instance.Hide(this);
        NotImplemented.Assert();
    }

    public void OnBuyLifeClicked()
    {
        UIService.Instance.Hide(this);
        NotImplemented.Assert();
    }

    public void OnMenuClicked()
    {
        GameStateMachine.Instance.SwitchState(new MenuGameState());
    }
}
