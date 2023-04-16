using System;
using UnityEngine;

public class PlatformSdk : Service<PlatformSdk>
{
    public Action OnPostInitialization;

    private void Start()
    {
        OnPostInitialization();
    }

    /** All derived classes have to call this method after initialization */
    protected virtual void PostInitialize()
    {
        OnPostInitialization?.Invoke();
    }

    public virtual void ToggleStickyAd(bool bEnable)
    { }

    public virtual void ShowVideoAd(
        Action OnOpenCallback = null, Action OnRewardedCallback = null,
        Action OnCloseCallback = null, Action<string> OnErrorCallback = null
    )
    {
        OnOpenCallback?.Invoke();
        OnRewardedCallback?.Invoke();
        OnCloseCallback?.Invoke();
    }

    public virtual void ShowFullscreenAd(
        Action OnOpenCallback = null, Action<bool> OnCloseCallback = null,
        Action<string> OnErrorCallback = null, Action OnOfflineCallback = null
    )
    {
        OnOpenCallback?.Invoke();
        OnCloseCallback?.Invoke(true);
    }
}
