using UnityEngine;

public class BHDecorator_TimeLimit : BHDecorator
{
    float m_TimeLimit;
    float m_TimeElapsed;
    bool m_bFailOnFalseCondition;

    public BHDecorator_TimeLimit(float TimeLimit, bool bFailOnFalseCondition = true)
    {
        m_TimeLimit = TimeLimit;
        m_bFailOnFalseCondition = bFailOnFalseCondition;
    } 

    public override void ComputeStartCondition(ref bool bResult)
    {
        m_TimeElapsed = 0.0f;
        bResult = true;
    }

    public override void ComputeUpdateCondition(ref bool bResult, ref bool bFailOnFalseCondition)
    {
        if (m_TimeElapsed >= m_TimeLimit)
        {
            bResult = false;
            bFailOnFalseCondition = m_bFailOnFalseCondition;
            return;
        }

        m_TimeElapsed += Time.deltaTime;
        bResult = true;
    }
}
