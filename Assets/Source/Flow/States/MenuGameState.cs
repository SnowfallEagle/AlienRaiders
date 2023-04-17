public class MenuGameState : GameState
{
    public override void Start()
    {
        base.Start();

        UIService.Instance.Show<MenuWidget>();

        var PlayerShip = PlayerState.Instance.PlayerShip;
        PlayerShip.StartRevive();
        // @TODO: Fly around after reviving
    }

    public override void Exit()
    {
        base.Exit();

        UIService.Instance.Hide<MenuWidget>();
    }
}
