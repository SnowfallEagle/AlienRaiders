using UnityEngine;

public class GameManager : CustomBehavior
{
    private void Start()
    {
        InitializePlatformSdk();
        InitializeGameStateMachine();

        var Console = ConsoleService.Instance;
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
        };
    }

    private void InitializeGameStateMachine()
    {
        var StateMachine = GameStateMachine.Instance;

        var Object = new GameObject();
        var FightState = Object.AddComponent<FightGameState>();

        int Level = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificLevel") ?
            GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Level") :
            FightGameState.AnyIdx;

        int Stage = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificStage") ?
            GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Stage") :
            FightGameState.AnyIdx;

        int Spawner = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificSpawner") ?
            GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Spawner") :
            FightGameState.AnyIdx;

        FightState.Initialize(Level, Stage, Spawner);
        StateMachine.SwitchState(FightState);
    }
}
