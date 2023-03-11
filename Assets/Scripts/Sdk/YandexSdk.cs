using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.WebUtility;
using Agava.YandexGames;

// TODO: Later we'll composite it better, but for now it works
public class YandexSdk : MonoBehaviour
{
    private IEnumerator Start()
    {
/*
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
*/
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChanged;

        YandexGamesSdk.CallbackLogging = true;
        yield return YandexGamesSdk.Initialize();
    }

    // DEBUG
    private void Update()
    {
        if (Input.GetKey(KeyCode.S)) StickyAd.Show();
        if (Input.GetKey(KeyCode.D)) StickyAd.Show();

        if (Input.GetKey(KeyCode.A)) PlayerAccount.Authorize();
        if (Input.GetKey(KeyCode.V)) VideoAd.Show();
        if (Input.GetKey(KeyCode.Z)) InterstitialAd.Show();
    }

    private void OnInBackgroundChanged(bool bInBackground)
    {
        AudioListener.pause = bInBackground;
        AudioListener.volume = bInBackground ? 0f : 1f;
    }
}
