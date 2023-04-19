using UnityEngine;

public class MenuGameState : GameState
{
    public override void Start()
    {
        base.Start();

        UIService.Instance.ShowWidget<MenuWidget>();

        var PlayerShip = PlayerState.Instance.PlayerShip;

        PlayerShip.Revive(false);
        PlayerShip.OnRevived = () =>
        {
            Cruise(PlayerShip, Random.Range(0, 2) == 1);
        };
    }

    public override void Exit()
    {
        base.Exit();

        UIService.Instance.HideWidget<MenuWidget>();
    }

    private void Cruise(PlayerShip PlayerShip, bool bLeft)
    {
        const float CruiseDelay = 1f;

        PlayerShip.BehaviorComponent.AddExclusiveAction(
            new BHPlayerAction_CinematicMove(bLeft ? PlayerShip.LeftCruisePosition : PlayerShip.RightCruisePosition, FirstPart: 0.5f, Deceleration: 0.05f)
                .AddOnActionFinished((_) =>
                {
                    TimerService.Instance.AddTimer(null, PlayerShip, () =>
                        {
                            // If we cruised to left -> start flying around to right and cruise right after
                            FlyAround(PlayerShip, !bLeft, !bLeft);
                        },
                        CruiseDelay
                    );
                })
        );
    }

    private void FlyAround(PlayerShip PlayerShip, bool bLeft, bool bNextCruiseIsLeft, bool bCruiseOnFinished = false)
    {
        const float FlyAroundDelay = 0.5f;

        Vector3 Diff = (bLeft ? -PlayerShip.FlyAroundDiff : PlayerShip.FlyAroundDiff) * (bCruiseOnFinished ? 2f : 1f);
        Vector3 Destination = PlayerShip.transform.position + Diff;

        PlayerShip.BehaviorComponent.AddExclusiveAction(
            new BHPlayerAction_CinematicMove(Destination, Acceleration: 0.025f, MaxRotationSpeed: 30f, FirstPart: 0.4f)
                .AddOnActionFinished((_) =>
                {
                    TimerService.Instance.AddTimer(null, PlayerShip, () =>
                        {
                            if (bCruiseOnFinished)
                            {
                                Cruise(PlayerShip, bNextCruiseIsLeft);
                            }
                            else
                            {
                                FlyAround(PlayerShip, !bLeft, bNextCruiseIsLeft, true);
                            }
                        },
                        FlyAroundDelay
                    );
                })
        );
    }
}
