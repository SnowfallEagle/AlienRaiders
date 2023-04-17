public class SuicideConsoleCommand : ConsoleCommand
{
    public override void Execute(object[] Args)
    {
        PlayerState.Instance.PlayerShip.HealthComponent.Kill();
    }
}
