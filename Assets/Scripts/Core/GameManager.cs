#if UNITY_WEBGL && !UNITY_EDITOR
    #define GAME_MODE_WEB
#endif

using UnityEngine;

public class GameManager : CustomBehavior
{
    private void Start()
    {
        InitializeSDK();
        InitializeGameStateMachine();
    }

    private void InitializeSDK()
    {
#if GAME_MODE_WEB
        ServiceLocator.Instance.Add<PlatformSDK, YandexSDK>();
#endif

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
