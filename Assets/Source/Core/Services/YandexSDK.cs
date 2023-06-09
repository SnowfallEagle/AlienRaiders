using System;
using System.Collections;
using UnityEngine;
using Agava.WebUtility;
using Agava.YandexGames;

public class YandexSdk : PlatformSdk
{
    private bool m_bAudioMutedByBackground;

    private IEnumerator Start()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChanged;

        YandexGamesSdk.CallbackLogging = true;
        yield return YandexGamesSdk.Initialize();

        PostInitialize();
    }

    private void Update()
    {
        // @DEBUG
        if (Input.GetKey(KeyCode.S)) StickyAd.Show();
        if (Input.GetKey(KeyCode.D)) StickyAd.Hide();

        if (Input.GetKey(KeyCode.A)) PlayerAccount.Authorize();
        if (Input.GetKey(KeyCode.V)) VideoAd.Show();
        if (Input.GetKey(KeyCode.Z)) InterstitialAd.Show();
    }

    private void OnInBackgroundChanged(bool bInBackground)
    {
        // @TEST
        if (bInBackground && !AudioService.Instance.bMuted)
        {
            AudioService.Instance.Mute(true);
            m_bAudioMutedByBackground = true;
        }
        else if (!bInBackground && m_bAudioMutedByBackground)
        {
            AudioService.Instance.Mute(false);
            m_bAudioMutedByBackground = false;
        }
    }

    public override void ToggleStickyAd(bool bEnable)
    {
        if (bEnable)
        {
            StickyAd.Show();
        }
        else
        {
            StickyAd.Hide();
        }
    }

    public override void ShowVideoAd(
        Action OnOpenCallback = null, Action OnRewardedCallback = null,
        Action OnCloseCallback = null, Action<string> OnErrorCallback = null
    )
    {
        VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
    }

    public override void ShowFullscreenAd(
        Action OnOpenCallback = null, Action<bool> OnCloseCallback = null,
        Action<string> OnErrorCallback = null, Action OnOfflineCallback = null
    )
    {
        InterstitialAd.Show(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
    }
}
