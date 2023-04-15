using UnityEngine;

public class PauseWidget : UIWidget
{
    public override void Show()
    {
        base.Show();

        Time.timeScale = 0f;
        PlayerState.Instance.PlayerShip.bProcessInput = false;
    }

    public override void Hide()
    {
        base.Hide();

        Time.timeScale = 1f;
        PlayerState.Instance.PlayerShip.bProcessInput = true;
    }

    public void OnContinueClicked()
    {
        Hide();
    }

    public void OnMenuClicked()
    {
        GameStateMachine.Instance.SwitchState(new MenuGameState());
    }

    public void OnMuteClicked()
    {
        NotImplemented.Assert();
    }
}
