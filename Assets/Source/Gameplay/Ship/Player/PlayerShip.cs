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

    [Header("Shield")]
    [SerializeField] private SpriteRenderer m_Shield;

    /** Should be right relative to ReadyPosition, so we can revive from random X position */
    // @TODO: Put this stuff in PlayerState instead
    [Header("Position")]
    [SerializeField] private Vector3 m_RevivePosition = Vector3.zero;
    public Vector3 RevivePosition => m_RevivePosition;

    [SerializeField] private Vector3 m_ReadyPosition = Vector3.zero;
    public Vector3 ReadyPosition => m_ReadyPosition;

    [SerializeField] private Vector3 m_LeftCruisePosition = Vector3.zero;
    public Vector3 LeftCruisePosition => m_LeftCruisePosition;

    [SerializeField] private Vector3 m_RightCruisePosition = Vector3.zero;
    public Vector3 RightCruisePosition => m_RightCruisePosition;

    public Action OnRevived;

    private Vector3 m_LastControlledWorldPosition = Vector3.zero;
    private bool m_bControlled  = false;
    private bool m_bCheckBounds = true;

    private BHAction_AnimateSpriteColor m_ShieldIdleAnimationAction;
    private BHAction_AnimateSpriteColor m_ShieldFadeOffAnimationAction;

    public override void Initialize(BuffMultipliers Buffs)
    {
        base.Initialize(Buffs);

        Vector3 Position   = transform.position;
        Position.z         = WorldZLayers.Player;
        m_RevivePosition.z = WorldZLayers.Player;
        m_ReadyPosition.z  = WorldZLayers.Player;
        transform.position = Position;

        gameObject.layer = LayerMask.NameToLayer("Player");
        m_Team = ShipTeam.Player;

        HealthComponent.OnDied += () =>
        {
            Assert.IsNotNull(GameStateMachine.Instance.GetCurrentState<FightGameState>());

            // @INCOMPLETE: On IntroLevel we should respawn player every time without ad...
            gameObject.SetActive(false);
            TimerService.Instance.AddTimer(null, this, () => { UIService.Instance.Show<DeathWidget>(); }, 2.5f);
        };

        Assert.IsNotNull(m_Shield);
        m_Shield.color = Color.clear;

        HealthComponent.OnShieldToggled += (bToggle) =>
        {
            if (bToggle)
            {
                BehaviorComponent.AbortAction(m_ShieldIdleAnimationAction); // Don't save color

                m_Shield.color = Color.white;
                m_ShieldIdleAnimationAction = new BHAction_AnimateSpriteColor(m_Shield, new Color(0.5f, 0.5f, 0.5f), 1f, true, true);
                BehaviorComponent.AddAction(m_ShieldIdleAnimationAction);
            }
            else
            {
                BehaviorComponent.FinishAction(m_ShieldFadeOffAnimationAction); // Save current color
                BehaviorComponent.FinishAction(m_ShieldIdleAnimationAction);    // Save current color

                m_ShieldFadeOffAnimationAction = new BHAction_AnimateSpriteColor(m_Shield, Color.clear, 0.5f);
                BehaviorComponent.AddAction(m_ShieldFadeOffAnimationAction);
            }
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

    public void Revive(bool bShield = true, float ShieldTimeRate = 2.5f)
    {
        gameObject.SetActive(true);
        m_bCheckBounds = false;

        BehaviorComponent.ClearActions();

        HealthComponent.SetMaxHealth();
        if (bShield)
        {
            HealthComponent.ToggleShield(true);
        }

        bProcessInput = false;

        Vector3 RevivePosition = m_RevivePosition;
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            RevivePosition.x = -RevivePosition.x;
        }
        transform.position = RevivePosition;

        BehaviorComponent.AddAction(new BHPlayerAction_CinematicMove(m_ReadyPosition)
            .AddOnActionFinished((_) =>
            {
                bProcessInput  = GameStateMachine.Instance.GetCurrentState<FightGameState>() != null;
                m_bCheckBounds = true;

                OnRevived?.Invoke();

                TimerService.Instance.AddTimer(null, this, () => { HealthComponent.ToggleShield(false); }, ShieldTimeRate);
            })
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
