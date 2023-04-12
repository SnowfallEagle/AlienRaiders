using UnityEngine;

public class GameManager : CustomBehavior
{
    private void Start()
    {
        InitializePlatformSdk();

        var StateMachine = GameStateMachine.Instance;
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
}
