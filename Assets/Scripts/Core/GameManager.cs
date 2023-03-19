using UnityEngine;

public class GameManager : CustomBehavior
{
    private void Start()
    {
        InitializePlatformSDK();
        InitializeGameStateMachine();
    }

    private void InitializePlatformSDK()
    {
        if (GameEnvironment.SDKType == GameEnvironment.PlatformSDK.Yandex)
        {
            ServiceLocator.Instance.Add<PlatformSDK, YandexSDK>();
        }

        var SDK = ServiceLocator.Instance.Get<PlatformSDK>();
        SDK.OnPostInitialization = () =>
        {
            SDK.ToggleStickyAd(true);
            SDK.ShowFullscreenAd();
        };
    }

    private void InitializeGameStateMachine()
    {
        var GameStateMachine = ServiceLocator.Instance.Get<GameStateMachine>();
    }
}
