public abstract class BHDecorator : BHNode
{
    public virtual void ComputeStartCondition(ref bool bResult)
    { }

    public virtual void ComputeUpdateCondition(ref bool bResult)
    { }
}
