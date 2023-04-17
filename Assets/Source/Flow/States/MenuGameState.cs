public class MenuGameState : GameState
{
    public override void Start()
    {
        base.Start();

        UIService.Instance.Show<MenuWidget>();

        var PlayerShip = PlayerState.Instance.PlayerShip;
        PlayerShip.bProcessInput = false;
        // @TODO: Make small behavior tree to fly around
    }

    public override void Exit()
    {
        base.Exit();

        UIService.Instance.Hide<MenuWidget>();
    }
}
