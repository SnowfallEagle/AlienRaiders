using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerShip : Ship
{
    private enum Weapons
    {
        Launcher,
        MaxWeapons
    }

    /** Should be right relative to ReadyPosition, so we can revive from random X position */
    [SerializeField] private Vector3 m_RevivePosition = Vector3.zero;
    public Vector3 RevivePosition => m_RevivePosition;

    [SerializeField] private Vector3 m_ReadyPosition = Vector3.zero;
    public Vector3 ReadyPosition => m_ReadyPosition;

    private Vector3 m_LastControlledWorldPosition = Vector3.zero;
    private bool m_bControlled = false;
    private bool m_bCheckBounds = true;

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        Vector3 Position = transform.position;
        Position.z = WorldZLayers.Player;
        transform.position = Position;

        gameObject.layer = LayerMask.NameToLayer("Player");
        m_Team = ShipTeam.Player;

        HealthComponent.OnDied += () =>
        {
            Assert.IsNotNull(GameStateMachine.Instance.GetCurrentState<FightGameState>()); ;

            gameObject.SetActive(false);
            TimerService.Instance.AddTimer(null, this, () => { UIService.Instance.Show<DeathWidget>(); }, 2.5f);
        };

#if UNITY_EDITOR
        HealthComponent.OnHealthChanged += (NewHealth, _) => { Debug.Log($"New Player Health: { NewHealth }"); };
#endif
    }

    private void LateUpdate()
    {
        CheckBounds();
    }

    private void OnDisable()
    {
        // Check to not to call TimerService in StopFire()
        if (gameObject.scene.isLoaded)
        {
            WeaponComponent.StopFire();
        }
    }

    public void StartRevive()
    {
        gameObject.SetActive(false);

        TimerService.Instance.AddTimer(null, this, () =>
            {
                gameObject.SetActive(true);
                m_bCheckBounds = false;

                HealthComponent.SetMaxHealth();
                // @TODO: Enable shield

                bProcessInput = false;
                BehaviorComponent.ClearActions();
                BehaviorComponent.AddAction(new BHPlayerAction_MoveFromReviveToReady()
                    .AddOnActionFinished((_, _) =>
                    {
                        bProcessInput  = GameStateMachine.Instance.GetCurrentState<FightGameState>() != null;
                        m_bCheckBounds = true;
                    })
                );
            },
            1f 
        );
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
                new BHShipCommand_StartFire().Process(BehaviorComponent);
                break;

            case TouchPhase.Stationary:
                m_LastControlledWorldPosition = CoreUtils.ScreenToWorldPosition(touch.position);
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                m_bControlled = false;
                new BHShipCommand_StopFire().Process(BehaviorComponent);
                break;

            case TouchPhase.Moved:
                Vector3 CurrentTouchWorldPosition = CoreUtils.ScreenToWorldPosition(touch.position);
                Vector3 DeltaPosition = CurrentTouchWorldPosition - m_LastControlledWorldPosition;
                m_LastControlledWorldPosition = CurrentTouchWorldPosition;

                new BHCommand_RelativeMove(DeltaPosition).Process(BehaviorComponent);
                break;
        }
    }

    private void ProcessMouse()
    {
        if (!Input.GetMouseButton(0))
        {
            if (m_bControlled)
            {
                new BHShipCommand_StopFire().Process(BehaviorComponent);
                m_bControlled = false;
            }
            return;
        }

        if (!m_bControlled)
        {
            m_LastControlledWorldPosition = CoreUtils.ScreenToWorldPosition(Input.mousePosition);
            m_bControlled = true;
                new BHShipCommand_StartFire().Process(BehaviorComponent);
            return;
        }

        Vector3 CurrentMouseWorldPosition = CoreUtils.ScreenToWorldPosition(Input.mousePosition);
        Vector3 DeltaPosition = CurrentMouseWorldPosition - m_LastControlledWorldPosition;
        m_LastControlledWorldPosition = CurrentMouseWorldPosition;

        new BHCommand_RelativeMove(DeltaPosition).Process(BehaviorComponent);
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
