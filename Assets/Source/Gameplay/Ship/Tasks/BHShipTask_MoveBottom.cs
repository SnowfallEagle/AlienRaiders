using UnityEngine;

public class BHShipTask_MoveBottom : BHTask
{
    private float m_Speed;
    private BehaviorComponent m_BehaviorComponent;

    public override void Start()
    {
        m_Speed = -m_Owner.GetComponent<Ship>().Speed;
        m_BehaviorComponent = m_Owner.GetComponent<BehaviorComponent>();
    }

    public override void Update()
    {
        m_BehaviorComponent.AddTask(new BHTask_RelativeMove(new Vector3(0f, m_Speed * Time.deltaTime, 0f)));
    }
}
