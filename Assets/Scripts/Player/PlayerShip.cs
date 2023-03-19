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

    protected override void Start()
    {
        base.Start();

        m_ShipTeam = Team.Player;
        m_bCheckBounds = true;
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

    protected override void OnPreInitializeWeapons()
    {
        PreSetNumWeapons((int)Weapons.MaxWeapons);
        PreAddWeapon<LauncherWeapon>((int)Weapons.Launcher);
    }

    private void ProcessTouch()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                m_bControlled = true;
                m_LastControlledWorldPosition = ScreenToWorldPosition(touch.position);
                BehaviorComponent.AddTask(new BHTaskStartFire());
                break;

            case TouchPhase.Stationary:
                m_LastControlledWorldPosition = ScreenToWorldPosition(touch.position);
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                m_bControlled = false;
                BehaviorComponent.AddTask(new BHTaskStopFire());
                break;

            case TouchPhase.Moved:
                Vector3 CurrentTouchWorldPosition = ScreenToWorldPosition(touch.position);
                Vector3 DeltaPosition = CurrentTouchWorldPosition - m_LastControlledWorldPosition;
                m_LastControlledWorldPosition = CurrentTouchWorldPosition;

                BehaviorComponent.AddTask(new BHTaskRelativeMove(DeltaPosition));
                break;
        }
    }

    private void ProcessMouse()
    {
        if (!Input.GetMouseButton(0))
        {
            if (m_bControlled)
            {
                BehaviorComponent.AddTask(new BHTaskStopFire());
                m_bControlled = false;
            }
            return;
        }

        if (!m_bControlled)
        {
            m_LastControlledWorldPosition = ScreenToWorldPosition(Input.mousePosition);
            m_bControlled = true;
            BehaviorComponent.AddTask(new BHTaskStartFire());
            return;
        }

        Vector3 CurrentMouseWorldPosition = ScreenToWorldPosition(Input.mousePosition);
        Vector3 DeltaPosition = CurrentMouseWorldPosition - m_LastControlledWorldPosition;
        m_LastControlledWorldPosition = CurrentMouseWorldPosition;

        BehaviorComponent.AddTask(new BHTaskRelativeMove(DeltaPosition));
    }

    // TODO: Put it in CoreUtils
    private Vector3 ScreenToWorldPosition(Vector3 Position)
    {
        return Camera.main.ScreenToWorldPoint(Position);
    }
}
