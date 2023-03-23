using System;
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

    [Serializable]
    public class DebugInfo
    {
        [SerializeField] public bool bEnabled = false;
    }

    [Serializable]
    public class DebugLevelInfo : DebugInfo
    {
        [SerializeField] public bool bSpecificLevel = false;
        [SerializeField] public int Level = 0;

        [SerializeField] public bool bSpecificStage = false;
        [SerializeField] public int Stage = 0;
    }

    [Serializable]
    public class DebugPlayerInfo : DebugInfo
    {
        [SerializeField] public bool bGodMode = false;
    }

    [Serializable]
    public class DebugAIInfo : DebugInfo
    {
        [SerializeField] public bool bDrawEyesight = false;
    }

    [SerializeField] public PlatformSDK SDKType =
#if UNITY_EDITOR
        PlatformSDK.Fake;
#else
        PlatformSDK.Yandex;
#endif

    [SerializeField] public bool bDebugMode = false;

    [SerializeField] public DebugLevelInfo DebugLevel = new DebugLevelInfo();
    [SerializeField] public DebugPlayerInfo DebugPlayer = new DebugPlayerInfo();
    [SerializeField] public DebugAIInfo DebugAI = new DebugAIInfo();

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

        // TODO: Bindings

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
            DebugAI.bDrawEyesight ^= true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            DebugPlayer.bGodMode ^= true;
        }
    }

    // Get debug option if game is in debug mode
    public T GetDebugOption<T>(string OptionName)
    {
        if (!bDebugMode)
        {
            return default(T);
        }

        string[] Options = OptionName.Split('.');

        Type CurrentType = GetType();
        object CurrentValue = this;

        foreach (var Option in Options)
        {
            System.Reflection.FieldInfo Field = CurrentType.GetField(Option);
            if (Field == null)
            {
                return default(T);
            }

            CurrentValue = Field.GetValue(CurrentValue);
            CurrentType = CurrentValue.GetType();

            var CurrentValueAsDebugInfo = CurrentValue as DebugInfo;
            if (CurrentValueAsDebugInfo != null && !CurrentValueAsDebugInfo.bEnabled)
            {
                return default(T);
            }
        }

        return CurrentValue != null && CurrentType == typeof(T) ? (T)CurrentValue : default(T);
    }
}
