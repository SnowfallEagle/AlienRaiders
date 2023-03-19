using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingService : CustomBehavior
{
    // Set up only on start
    public float ZNearClip = 0.1f;
    public float ZFarClip = 2000f;

    // If null then use TargetSize and TargetCenter
    [SerializeField] protected SpriteRenderer m_TargetSizeSprite;

    [SerializeField] protected Vector3 m_TargetSize = new Vector3(11.5f, 20f);
    public Vector3 TargetSize => m_TargetSizeSprite ? m_TargetSizeSprite.bounds.size : m_TargetSize;

    [SerializeField] protected Vector3 m_TargetCenter = Vector3.zero;
    public Vector3 TargetCenter => m_TargetSizeSprite ? m_TargetSizeSprite.bounds.center : m_TargetCenter;

    private void Start()
    {
        Camera.main.nearClipPlane = ZNearClip;
        Camera.main.farClipPlane = ZFarClip;
    }

    private void Update()
    {
        Vector3 CurrentTargetSize = TargetSize;

        float ScreenRatio = (float)Screen.width / (float)Screen.height;
        float TargetRatio = CurrentTargetSize.x / CurrentTargetSize.y;
        float TargetYDivTwo = CurrentTargetSize.y * 0.5f;

        if (ScreenRatio >= TargetRatio)
        {
            Camera.main.orthographicSize = TargetYDivTwo;
        }
        else
        {
            /*
                If TargetRatio > ScreenRatio then we need to expand width of target,
                so we can get at least target's needed with and more than target's height.

                We multiply target's height by ratio of target and screen.

                Example:
                    Screen Size: 800x600
                    Target Size: 1280x720

                    Screen Ratio: 1.(3)
                    Target Ratio: 1.(7)
                    Target Ratio / Screen Ratio: 1.(3)

                    Multiply (Target Height / 2) by Target Screen Ratio:
                    360 * 1.(3) = 480

                    So we get at least target width and more than target height.
            */

            float YScale = TargetRatio / ScreenRatio;
            Camera.main.orthographicSize = TargetYDivTwo * YScale;
        }
    }
}
