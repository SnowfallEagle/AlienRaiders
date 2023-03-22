using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : Service<GameEnvironment>
{
    public enum PlatformSDK
    {
        Fake,
        Yandex
    }

    [SerializeField] public PlatformSDK SDKType =
#if UNITY_EDITOR
        PlatformSDK.Fake;
#else
        PlatformSDK.Yandex;
#endif

    [SerializeField] public bool bDebugMode = false;
    [SerializeField] public bool bDebugDrawAI = false;
    [SerializeField] public bool bDebugGodMode = false;

    protected override void Initialize()
    {
        // Enforce values

#if UNITY_EDITOR
        SDKType = PlatformSDK.Fake;
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
        bDebugMode = false;
#endif
    }

    private void Update()
    {
    #if !UNITY_EDITOR && UNITY_WEBGL
        return;
    #endif

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            bDebugMode ^= true;
        }

        if (!bDebugMode)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            bDebugDrawAI ^= true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            bDebugGodMode ^= true;
        }
    }

    // Get debug option if game is in debug mode
    public T GetDebugOption<T>(string OptionName)
    {
        if (!bDebugMode)
        {
            return default(T);
        }

        var Field = GetType().GetField(OptionName);
        if (Field == null)
        {
            return default(T);
        }

        return (T)Field.GetValue(this);
    }
}
