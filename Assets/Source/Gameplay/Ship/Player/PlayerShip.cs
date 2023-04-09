using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{
    private enum Weapons
    {
        Launcher,
        MaxWeapons
    }

    private Vector3 m_LastControlledWorldPosition = Vector3.zero;
    private bool m_bControlled = false;

    private bool m_bCheckBounds = true;

    private void Start()
    {
        // @TODO: In future we'll see how spawn and initialize player better
        Initialize(new BuffMultipliers());
    }

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        gameObject.layer = LayerMask.NameToLayer("Player");

        m_Team = ShipTeam.Player;
    }

    private void LateUpdate()
    {
        CheckBounds();
    }

    protected override void ProcessInput()
    {
        if (Input.touchCount <= 0)
        {
            ProcessMouse();
        }
        else
        {
            ProcessTouch();
        }
    }

    protected override Type[] OnPreInitializeWeapons()
    {
        Type[] WeaponTypes = new Type[(int)Weapons.MaxWeapons];
        WeaponTypes[(int)Weapons.Launcher] = typeof(LauncherWeapon);
        return WeaponTypes;
    }

    private void ProcessTouch()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                m_bControlled = true;
                m_LastControlledWorldPosition = CoreUtils.ScreenToWorldPosition(touch.position);
                BehaviorComponent.AddTask(new BHShipTask_StartFire());
                break;

            case TouchPhase.Stationary:
                m_LastControlledWorldPosition = CoreUtils.ScreenToWorldPosition(touch.position);
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                m_bControlled = false;
                BehaviorComponent.AddTask(new BHShipTask_StopFire());
                break;

            case TouchPhase.Moved:
                Vector3 CurrentTouchWorldPosition = CoreUtils.ScreenToWorldPosition(touch.position);
                Vector3 DeltaPosition = CurrentTouchWorldPosition - m_LastControlledWorldPosition;
                m_LastControlledWorldPosition = CurrentTouchWorldPosition;

                BehaviorComponent.AddTask(new BHTask_RelativeMove(DeltaPosition));
                break;
        }
    }

    private void ProcessMouse()
    {
        if (!Input.GetMouseButton(0))
        {
            if (m_bControlled)
            {
                BehaviorComponent.AddTask(new BHShipTask_StopFire());
                m_bControlled = false;
            }
            return;
        }

        if (!m_bControlled)
        {
            m_LastControlledWorldPosition = CoreUtils.ScreenToWorldPosition(Input.mousePosition);
            m_bControlled = true;
            BehaviorComponent.AddTask(new BHShipTask_StartFire());
            return;
        }

        Vector3 CurrentMouseWorldPosition = CoreUtils.ScreenToWorldPosition(Input.mousePosition);
        Vector3 DeltaPosition = CurrentMouseWorldPosition - m_LastControlledWorldPosition;
        m_LastControlledWorldPosition = CurrentMouseWorldPosition;

        BehaviorComponent.AddTask(new BHTask_RelativeMove(DeltaPosition));
    }

    private void CheckBounds()
    {
        if (!m_bCheckBounds)
        {
            return;
        }

        var Renderer = RenderingService.Instance;

        Vector3 BoundsCenter = Renderer.TargetCenter;
        Vector3 BoundsSizeDiv2 = Renderer.TargetSize / 2;

        Vector3 BoundsBottomLeft = BoundsCenter - BoundsSizeDiv2;
        Vector3 BoundsTopRight = BoundsCenter + BoundsSizeDiv2;

        Vector3 CurrentPosition = transform.position;

        if (CurrentPosition.x < BoundsBottomLeft.x) CurrentPosition.x = BoundsBottomLeft.x;
        if (CurrentPosition.y < BoundsBottomLeft.y) CurrentPosition.y = BoundsBottomLeft.y;

        if (CurrentPosition.y > BoundsTopRight.y) CurrentPosition.y = BoundsTopRight.y;
        if (CurrentPosition.x > BoundsTopRight.x) CurrentPosition.x = BoundsTopRight.x;

        transform.position = CurrentPosition;
    }
}
