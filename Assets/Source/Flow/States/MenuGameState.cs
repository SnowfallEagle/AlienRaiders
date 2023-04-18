using UnityEngine;

public class MenuGameState : GameState
{
    public override void Start()
    {
        base.Start();

        UIService.Instance.Show<MenuWidget>();

        var PlayerShip = PlayerState.Instance.PlayerShip;
        PlayerShip.Revive(false);

        PlayerShip.OnRevived = () =>
        {
            NextCruise(PlayerShip, Random.Range(0, 2) == 1);
        };
    }

    public override void Exit()
    {
        base.Exit();

        UIService.Instance.Hide<MenuWidget>();
        PlayerState.Instance.PlayerShip.OnRevived = null;
    }

    private void NextCruise(PlayerShip PlayerShip, bool bLeftCruise)
    {
        PlayerShip.BehaviorComponent.AddExclusiveAction(
            new BHPlayerAction_CinematicMove(bLeftCruise ? PlayerShip.LeftCruisePosition : PlayerShip.RightCruisePosition)
                .AddOnActionFinished((_) => { NextCruise(PlayerShip, !bLeftCruise); } )
        );
    }
}
