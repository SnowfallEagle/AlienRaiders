using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEnvironment
{
    public enum PlatformSDK
    {
        Fake,
        Yandex
    }

    public static PlatformSDK SDKType =
#if UNITY_WEBGL && !UNITY_EDITOR
        PlatformSDK.Yandex;
#else
        PlatformSDK.Fake;
#endif

    public static bool bDebugDrawAI =
#if UNITY_EDITOR && DEBUG
        true;
#else
        false;
#endif
}
