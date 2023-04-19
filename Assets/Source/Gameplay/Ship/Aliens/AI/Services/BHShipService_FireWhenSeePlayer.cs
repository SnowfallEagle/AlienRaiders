using System;
using UnityEngine;

public class BHShipService_FireWhenSeePlayer : BHService
{
    private float m_HalfFov;
    PlayerShip m_PlayerShip = PlayerState.Instance.PlayerShip;

    public BHShipService_FireWhenSeePlayer(float Fov = 180f)
    {
        m_HalfFov = Fov * 0.5f;
    }

    public override void Update()
    {
        Vector3 PlayerPosition = m_PlayerShip.transform.position;
        PlayerPosition.z = 0f;

        Vector3 OwnerPosition = m_Owner.transform.position;
        OwnerPosition.z = 0f;

        Vector3 DirectionToPlayer = (PlayerPosition - OwnerPosition).normalized;
        float Angle = Mathf.Rad2Deg * MathF.Acos(Vector3.Dot(m_Owner.transform.up, DirectionToPlayer));

        if (MathF.Abs(Angle) < m_HalfFov)
        {
            new BHShipCommand_StartFire().Process(m_Owner);
        }
        else
        {
            new BHShipCommand_StopFire().Process(m_Owner);
        }

        // @DEBUG
        if (GameEnvironment.Instance.GetDebugOption<bool>("DebugAI.bDrawEyesight"))
        {
            // Direction to player
            {
                Debug.DrawRay(m_Owner.transform.position, DirectionToPlayer, Color.red);
            }

            // FOV
            {
                float FOVDiv2Rad = Mathf.Deg2Rad * m_HalfFov;

                Vector3 TransformAngles = Mathf.Deg2Rad * m_Owner.transform.rotation.eulerAngles;
                float SpriteZRotation = MathF.PI * 0.5f;
                float ZTransform = TransformAngles.z - SpriteZRotation;

                Vector3 DirectionRight = new Vector3(
                    MathF.Cos(FOVDiv2Rad + ZTransform),
                    -MathF.Sin(FOVDiv2Rad + ZTransform),
                    0f
                ) * 5f;
                Vector3 DirectionLeft = new Vector3(
                    MathF.Cos(-FOVDiv2Rad + ZTransform),
                    -MathF.Sin(-FOVDiv2Rad + ZTransform),
                    0f
                ) * 5f;

                Debug.DrawRay(m_Owner.transform.position, DirectionLeft, Color.magenta);
                Debug.DrawRay(m_Owner.transform.position, DirectionRight, Color.magenta);
            }
        }
    }
}
