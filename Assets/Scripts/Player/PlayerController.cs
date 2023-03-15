using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer m_ScreenBounds;

    private Ship m_Ship;

    private Vector3 m_LastControlledWorldPosition = Vector3.zero;
    private bool m_bControlled = false;

    private void Start()
    {
        m_Ship = GetComponent<Ship>();

        Assert.IsNotNull(m_Ship); // Ship check already asserts its components
        Assert.IsNotNull(m_ScreenBounds);
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
                m_Ship.AddTask(new BHTaskStartFire());
                break;

            case TouchPhase.Stationary:
                m_LastControlledWorldPosition = ScreenToWorldPosition(touch.position);
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                m_bControlled = false;
                m_Ship.AddTask(new BHTaskStopFire());
                break;

            case TouchPhase.Moved:
                Vector3 CurrentTouchWorldPosition = ScreenToWorldPosition(touch.position);
                Vector3 DeltaPosition = CurrentTouchWorldPosition - m_LastControlledWorldPosition;
                m_LastControlledWorldPosition = CurrentTouchWorldPosition;

                m_Ship.AddTask(new BHTaskMoveByDelta(DeltaPosition));
                break;
        }
    }

    private void ProcessMouse()
    {
        if (!Input.GetMouseButton(0))
        {
            if (m_bControlled)
            {
                m_Ship.AddTask(new BHTaskStopFire());
                m_bControlled = false;
            }
            return;
        }

        if (!m_bControlled)
        {
            m_LastControlledWorldPosition = ScreenToWorldPosition(Input.mousePosition);
            m_bControlled = true;
            m_Ship.AddTask(new BHTaskStartFire());
            return;
        }

        Vector3 CurrentMouseWorldPosition = ScreenToWorldPosition(Input.mousePosition);
        Vector3 DeltaPosition = CurrentMouseWorldPosition - m_LastControlledWorldPosition;
        m_LastControlledWorldPosition = CurrentMouseWorldPosition;

        m_Ship.AddTask(new BHTaskMoveByDelta(DeltaPosition));
    }

    private Vector3 ScreenToWorldPosition(Vector3 Position)
    {
        return Camera.main.ScreenToWorldPoint(Position);
    }

    private void CheckBounds()
    {
        if (!m_ScreenBounds)
        {
            return;
        }

        Vector3 CurrentPosition = transform.position;

        Vector3 BoundsSizeDiv2 = m_ScreenBounds.bounds.size * 0.5f;
        Vector3 BoundsCenter = m_ScreenBounds.bounds.center;

        Vector3 BoundsBottomLeft = BoundsCenter - BoundsSizeDiv2;
        Vector3 BoundsTopRight = BoundsCenter + BoundsSizeDiv2;

        if (CurrentPosition.x < BoundsBottomLeft.x) CurrentPosition.x = BoundsBottomLeft.x;
        if (CurrentPosition.y < BoundsBottomLeft.y) CurrentPosition.y = BoundsBottomLeft.y;

        if (CurrentPosition.y > BoundsTopRight.y) CurrentPosition.y = BoundsTopRight.y;
        if (CurrentPosition.x > BoundsTopRight.x) CurrentPosition.x = BoundsTopRight.x;

        transform.position = CurrentPosition;
    }
}
