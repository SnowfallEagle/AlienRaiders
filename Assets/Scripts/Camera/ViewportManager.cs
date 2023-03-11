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

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = TargetSizeSprite.bounds.size.x / TargetSizeSprite.bounds.size.y;
        float targetYDivTwo = TargetSizeSprite.bounds.size.y * 0.5f;

        Camera camera = GetComponent<Camera>();

        if (screenRatio >= targetRatio)
        {
            camera.orthographicSize = targetYDivTwo;
        }
        else
        {
            float screenToTarget = targetRatio / screenRatio;
            camera.orthographicSize = targetYDivTwo * screenToTarget;
        }
    }
}
