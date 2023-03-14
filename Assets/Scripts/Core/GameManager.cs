using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
#if UNITY_WEBGL
        ServiceLocator.Instance.Add<AdService, YandexAdService>();
#endif
        var AdService = ServiceLocator.Instance.Get<AdService>();
        AdService.ShowFullscreenAd();
    }
}
