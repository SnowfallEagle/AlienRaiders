using System.Collections;

public class GameManager : CustomBehavior
{
    private void Start()
    {
        InitializePlatformSdk();
        PlayerState.Instance.SpawnShip();
        var UI = UIService.Instance;
        var Console = ConsoleService.Instance;

        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        /* @NOTE:
            Skip frame before initializing first state, because state
            may want to call UIService that didn't has time to collect huds.

            Later we may want to change execution orders, but for now it's fine.
        */
        yield return null;

        InitializeGameStateMachine();
    }

    private void InitializePlatformSdk()
    {
        if (GameEnvironment.Instance.SdkType == GameEnvironment.PlatformSdk.Yandex)
        {
            ServiceLocator.Instance.Add<PlatformSdk, YandexSdk>();
        }

        var Sdk = PlatformSdk.Instance;
        Sdk.OnPostInitialization = () =>
        {
            Sdk.ToggleStickyAd(true);
            Sdk.ShowFullscreenAd();

            InitializeRenderingService();
        };
    }

    private void InitializeRenderingService()
    {
        RenderingService.Instance.UpdateAppearance();
    }

    private void InitializeGameStateMachine()
    {
        var StateMachine = GameStateMachine.Instance;

        GameState NewState;
        if (GameEnvironment.Instance.GetDebugOption<bool>("bDebugLevel.bEnabled"))
        {
            int Level = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificLevel") ?
                GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Level") :
                FightGameState.AnyIdx;

            int Stage = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificStage") ?
                GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Stage") :
                FightGameState.AnyIdx;

            int Spawner = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificSpawner") ?
                GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Spawner") :
                FightGameState.AnyIdx;

            NewState = new FightGameState(Level, Stage, Spawner);
        }
        else
        {
            NewState = new MenuGameState();
        }

        StateMachine.SwitchState(NewState);
    }
}
