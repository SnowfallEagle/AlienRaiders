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

    private class DebugOptionBinding
    {
        public ConstValueRef<bool> bEnabled;
        public ValueRef<bool> bOption;
    }

    [SerializeField] public PlatformSDK SDKType =
#if NDEBUG
        PlatformSDK.Yandex;
#else
        PlatformSDK.Fake;
#endif

    [SerializeField] public bool bDebugMode = false;

    [SerializeField] public DebugLevelInfo DebugLevel = new DebugLevelInfo();
    [SerializeField] public DebugPlayerInfo DebugPlayer = new DebugPlayerInfo();
    [SerializeField] public DebugAIInfo DebugAI = new DebugAIInfo();

    private Dictionary<KeyCode, DebugOptionBinding> InputBindings;

    protected override void Initialize()
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

    private void EnforceEnvironment()
    {
#if _DEBUG
        SDKType = PlatformSDK.Fake;
#else
        bDebugMode = false;
#endif
    }

    private void SetBindings()
    {
        InputBindings = new Dictionary<KeyCode, DebugOptionBinding>
        {
            // AI
            { KeyCode.Keypad7, // bEnabled
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => true),
                    bOption = new ValueRef<bool>(() => DebugAI.bEnabled, Value => DebugAI.bEnabled = Value)
                }
            },
            { KeyCode.Keypad1, // bDrawEyesight
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => DebugAI.bEnabled),
                    bOption = new ValueRef<bool>(() => DebugAI.bDrawEyesight, Value => DebugAI.bDrawEyesight = Value)
                }
            },

            // Player
            { KeyCode.Keypad8, // bEnabled
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => true),
                    bOption = new ValueRef<bool>(() => DebugPlayer.bEnabled, Value => DebugPlayer.bEnabled = Value)
                }
            },
            { KeyCode.Keypad2, // bGodMode
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => DebugPlayer.bEnabled),
                    bOption = new ValueRef<bool>(() => DebugPlayer.bGodMode, Value => DebugPlayer.bGodMode = Value)
                }
            },

            // Level
            { KeyCode.Keypad9, // bEnabled
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => true),
                    bOption = new ValueRef<bool>(() => DebugLevel.bEnabled, Value => DebugLevel.bEnabled = Value)
                }
            },
            { KeyCode.Keypad4, // bSpecificLevel
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => DebugLevel.bEnabled),
                    bOption = new ValueRef<bool>(() => DebugLevel.bSpecificLevel, Value => DebugLevel.bSpecificLevel = Value)
                }
            },
            { KeyCode.Keypad5, // bSpecificStage
                new DebugOptionBinding
                {
                    bEnabled = new ConstValueRef<bool>(() => DebugLevel.bEnabled),
                    bOption = new ValueRef<bool>(() => DebugLevel.bSpecificStage, Value => DebugLevel.bSpecificStage = Value)
                }
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
                DebugOptionBinding Bind = InputBinding.Value;
                if (Bind.bEnabled.Value)
                {
                    Bind.bOption.Value = !Bind.bOption.Value;
                }
            }
        }
    }
}
