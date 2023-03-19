using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : CustomBehavior
{
    public enum PlatformSDK
    {
        Fake,
        Yandex
    }

    [SerializeField] public PlatformSDK SDKType = PlatformSDK.Yandex;

    [SerializeField] public bool bDebugMode = false;
    [SerializeField] public bool bDebugDrawAI = false;

    private void Start()
    {
#if UNITY_EDITOR
        SDKType = PlatformSDK.Fake;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        bDebugMode = false;
#endif
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
