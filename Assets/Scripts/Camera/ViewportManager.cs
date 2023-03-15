using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportManager : MonoBehaviour
{
    public SpriteRenderer TargetSizeSprite = null;

    private void Update()
    {
        // TODO: Move resizing in Awake()?

        if (!TargetSizeSprite)
        {
            return;
        }

        float ScreenRatio = (float)Screen.width / (float)Screen.height;
        float TargetRatio = TargetSizeSprite.bounds.size.x / TargetSizeSprite.bounds.size.y;
        float TargetYDivTwo = TargetSizeSprite.bounds.size.y * 0.5f;

        Camera Camera = GetComponent<Camera>();

        if (ScreenRatio >= TargetRatio)
        {
            Camera.orthographicSize = TargetYDivTwo;
        }
        else
        {
            float ScreenToTarget = TargetRatio / ScreenRatio;
            Camera.orthographicSize = TargetYDivTwo * ScreenToTarget;
        }
    }
}
