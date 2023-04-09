public abstract class BHDecorator : BHNode
{
    public virtual void ComputeStartCondition(ref bool bResult)
    {
        bResult = true;
    }

    public virtual void ComputeUpdateCondition(ref bool bResult, ref bool bFailOnFalseCondition)
    {
        bResult = true;
    }
}
