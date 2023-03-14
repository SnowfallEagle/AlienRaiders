#if UNITY_WEBGL && !UNITY_EDITOR
    #define GAME_MODE_WEB
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
#if GAME_MODE_WEB
        ServiceLocator.Instance.Add<AdService, YandexAdService>();
#endif

        var AdService = ServiceLocator.Instance.Get<AdService>();
        AdService.ToggleStickyAd(true);
        AdService.ShowFullscreenAd();
    }
}
