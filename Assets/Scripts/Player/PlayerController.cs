using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public SpriteRenderer TargetSizeSprite;
    private Rigidbody2D m_RigidBody;

    private Vector3 m_LastControlledWorldPosition = Vector3.zero;
    private bool m_bControlled = false;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();

        Assert.IsNotNull(TargetSizeSprite);
    }

    private void Update()
    {
        if (Input.touchCount <= 0)
        {
            ProcessMouse();
        }
        else
        {
            ProcessTouch();
        }

        CheckBounds();
    }

    private void ProcessTouch()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                m_bControlled = true;
                m_LastControlledWorldPosition = ScreenToWorldPosition(touch.position);
                break;

            case TouchPhase.Stationary:
                m_LastControlledWorldPosition = ScreenToWorldPosition(touch.position);
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                m_bControlled = false;
                break;

            case TouchPhase.Moved:
                Vector3 currentTouchWorldPosition = ScreenToWorldPosition(touch.position);
                Vector3 deltaPosition = currentTouchWorldPosition - m_LastControlledWorldPosition;
                m_LastControlledWorldPosition = currentTouchWorldPosition;

                transform.position += deltaPosition;
                break;
        }
    }

    private void ProcessMouse()
    {
        if (!Input.GetMouseButton(0))
        {
            m_bControlled = false;
            return;
        }

        if (!m_bControlled)
        {
            m_LastControlledWorldPosition = ScreenToWorldPosition(Input.mousePosition);
            m_bControlled = true;
            return;
        }

        Vector3 currentMouseWorldPosition = ScreenToWorldPosition(Input.mousePosition);
        Vector3 deltaPosition = currentMouseWorldPosition - m_LastControlledWorldPosition;
        m_LastControlledWorldPosition = currentMouseWorldPosition;

        transform.position += deltaPosition;
    }

    private Vector3 ScreenToWorldPosition(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }

    private void CheckBounds()
    {
        if (!TargetSizeSprite)
        {
            return;
        }

        Vector3 currentPosition = transform.position;

        Vector3 boundsSizeDiv2 = TargetSizeSprite.bounds.size * 0.5f;
        Vector3 boundsCenter = TargetSizeSprite.bounds.center;

        Vector3 boundsBottomLeft = boundsCenter - boundsSizeDiv2;
        Vector3 boundsTopRight = boundsCenter + boundsSizeDiv2;

        if (currentPosition.x < boundsBottomLeft.x) currentPosition.x = boundsBottomLeft.x;
        if (currentPosition.y < boundsBottomLeft.y) currentPosition.y = boundsBottomLeft.y;

        if (currentPosition.y > boundsTopRight.y) currentPosition.y = boundsTopRight.y;
        if (currentPosition.x > boundsTopRight.x) currentPosition.x = boundsTopRight.x;

        transform.position = currentPosition;
    }
}
