#if !UNITY_EDITOR && UNITY_WEBGL
    #define NDEBUG
#else
    #define _DEBUG
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : Service<GameEnvironment>
{
    public enum PlatformSdk
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

        [SerializeField] public bool bSpecificSpawner = false;
        [SerializeField] public int Spawner = 0;
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

    [SerializeField] public PlatformSdk SdkType =
#if NDEBUG
        PlatformSdk.Yandex;
#else
        PlatformSdk.Fake;
#endif

    [SerializeField] public bool bDebugMode = false;

    [SerializeField] public DebugLevelInfo DebugLevel = new DebugLevelInfo();
    [SerializeField] public DebugPlayerInfo DebugPlayer = new DebugPlayerInfo();
    [SerializeField] public DebugAIInfo DebugAI = new DebugAIInfo();

    private Dictionary<KeyCode, ValueRef<bool>> InputBindings;

    public GameEnvironment()
    {
        EnforceEnvironment();
        SetBindings();
    }
 
    private void Update()
    {
#if NDEBUG
        return;
#endif

        ProcessInput();
    }

    /** Get debug option if game is in debug mode */
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

    private void EnforceEnvironment()
    {
#if _DEBUG
        SdkType = PlatformSdk.Fake;
#else
        bDebugMode = false;
#endif
    }

    private void SetBindings()
    {
        InputBindings = new Dictionary<KeyCode, ValueRef<bool>>
        {
            // AI
            { // bEnabled
                KeyCode.Keypad7,
                new ValueRef<bool>(() => DebugAI.bEnabled, Value => DebugAI.bEnabled = Value)
            },
            { // bDrawEyesight
                KeyCode.Keypad1,
                new ValueRef<bool>(() => DebugAI.bDrawEyesight, Value => DebugAI.bDrawEyesight = Value)
            },

            // Player
            { // bEnabled
                KeyCode.Keypad8,
                new ValueRef<bool>(() => DebugPlayer.bEnabled, Value => DebugPlayer.bEnabled = Value)
            },
            { // bGodMode
                KeyCode.Keypad2,
                new ValueRef<bool>(() => DebugPlayer.bGodMode, Value => DebugPlayer.bGodMode = Value)
            },

            // Level
            { // bEnabled
                KeyCode.Keypad9,
                new ValueRef<bool>(() => DebugLevel.bEnabled, Value => DebugLevel.bEnabled = Value)
            },
            { // bSpecificLevel
                KeyCode.Keypad4,
                new ValueRef<bool>(() => DebugLevel.bSpecificLevel, Value => DebugLevel.bSpecificLevel = Value)
            },
            { // bSpecificStage
                KeyCode.Keypad5,
                new ValueRef<bool>(() => DebugLevel.bSpecificStage, Value => DebugLevel.bSpecificStage = Value)
            },
        };
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            bDebugMode ^= true;
        }

        if (!bDebugMode)
        {
            return;
        }

        foreach (var InputBinding in InputBindings)
        {
            if (Input.GetKeyDown(InputBinding.Key))
            {
                InputBinding.Value.Value ^= true;
            }
        }
    }
}
