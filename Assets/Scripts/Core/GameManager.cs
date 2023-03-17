#if UNITY_WEBGL && !UNITY_EDITOR
    #define GAME_MODE_WEB
#endif

using UnityEngine;

public class GameManager : CustomBehaviour
{
    private void Start()
    {
        InitializeAd();
        InitializeGameStateMachine();
    }

    private void InitializeAd()
    {
#if GAME_MODE_WEB
        ServiceLocator.Instance.Add<AdService, YandexAdService>();
#endif

        var AdService = ServiceLocator.Instance.Get<AdService>();
        AdService.OnPostInitialization = () =>
        {
            AdService.ToggleStickyAd(true);
            AdService.ShowFullscreenAd();
        };
    }

    private void InitializeGameStateMachine()
    {
        var GameStateMachine = ServiceLocator.Instance.Get<GameStateMachine>();
    }
}
