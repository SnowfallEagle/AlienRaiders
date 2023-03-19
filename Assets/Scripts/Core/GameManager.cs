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
        if (GameEnvironment.Instance.SDKType == GameEnvironment.PlatformSDK.Yandex)
        {
            ServiceLocator.Instance.Add<PlatformSDK, YandexSDK>();
        }

        var SDK = PlatformSDK.Instance;
        SDK.OnPostInitialization = () =>
        {
            SDK.ToggleStickyAd(true);
            SDK.ShowFullscreenAd();
        };
    }

    private void InitializeGameStateMachine()
    {
        var StateMachine = GameStateMachine.Instance;
    }
}
