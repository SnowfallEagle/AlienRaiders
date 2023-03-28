using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move FOV -> Ship?
public class BHShipTask_FireWhenSeePlayer : BHTask
{
    private float m_FOV;
    private BehaviorComponent m_BehaviorComponent;

    public BHShipTask_FireWhenSeePlayer(float FOV = 180f)
    {
        m_FOV = FOV;
    }

    public override void Start(MonoBehaviour Owner)
    {
        m_BehaviorComponent = Owner.GetComponent<BehaviorComponent>();
    }

    public override void Update(MonoBehaviour Owner)
    {
        PlayerShip PlayerShip = PlayerState.Instance.PlayerShip;
        if (!PlayerShip)
        {
            m_BehaviorComponent.AddTask(new BHShipTask_StopFire());
            return;
        }

        Vector3 ToPlayerVector = Vector3.Normalize(PlayerShip.transform.position - Owner.transform.position);
        float CosAngle = Vector3.Dot(Owner.transform.up, ToPlayerVector);
        float Angle = Mathf.Rad2Deg * Mathf.Acos(CosAngle);

        if (Mathf.Abs(Angle) < m_FOV * 0.5f)
        {
            m_BehaviorComponent.AddTask(new BHShipTask_StartFire());
        }
        else
        {
            m_BehaviorComponent.AddTask(new BHShipTask_StopFire());
        }

        // DEBUG
        if (GameEnvironment.Instance.GetDebugOption<bool>("DebugAI.bDrawEyesight"))
        {
            // Direction to player
            {
                Debug.DrawRay(Owner.transform.position, PlayerShip.transform.position - Owner.transform.position, Color.red);
            }

            // FOV
            {
                float FOVDiv2Rad = Mathf.Deg2Rad * (m_FOV * 0.5f);

                Vector3 TransformAngles = Mathf.Deg2Rad * Owner.transform.rotation.eulerAngles;
                float SpriteZRotation = Mathf.PI * 0.5f;
                float ZTransform = TransformAngles.z - SpriteZRotation;

                Vector3 DirectionRight = new Vector3(
                    Mathf.Cos(FOVDiv2Rad + ZTransform),
                    -Mathf.Sin(FOVDiv2Rad + ZTransform),
                    0f
                ) * 5f;
                Vector3 DirectionLeft = new Vector3(
                    Mathf.Cos(-FOVDiv2Rad + ZTransform),
                    -Mathf.Sin(-FOVDiv2Rad + ZTransform),
                    0f
                ) * 5f;

                Debug.DrawRay(Owner.transform.position, DirectionLeft, Color.magenta);
                Debug.DrawRay(Owner.transform.position, DirectionRight, Color.magenta);
            }
        }
    }
}
