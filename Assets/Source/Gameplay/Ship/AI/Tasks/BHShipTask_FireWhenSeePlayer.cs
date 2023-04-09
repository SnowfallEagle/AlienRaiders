using System;
using UnityEngine;

// @TODO: Move FOV -> AlienShip?
// @INCOMPLETE: Port to service
public class BHShipTask_FireWhenSeePlayer : BHTaskNode
{
    private float m_FOV;

    public BHShipTask_FireWhenSeePlayer(float FOV = 180f)
    {
        m_FOV = FOV;
    }

    public override void Update()
    {
        PlayerShip PlayerShip = PlayerState.Instance.PlayerShip;
        if (!PlayerShip)
        {
            new BHShipCommand_StopFire().Process(m_Owner);
            return;
        }

        Vector3 ToPlayerVector = (PlayerShip.transform.position - m_Owner.transform.position).normalized;
        float Angle = Mathf.Rad2Deg * MathF.Acos(Vector3.Dot(m_Owner.transform.up, ToPlayerVector));

        if (MathF.Abs(Angle) < m_FOV * 0.5f)
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
                Debug.DrawRay(m_Owner.transform.position, PlayerShip.transform.position - m_Owner.transform.position, Color.red);
            }

            // FOV
            {
                float FOVDiv2Rad = Mathf.Deg2Rad * (m_FOV * 0.5f);

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
