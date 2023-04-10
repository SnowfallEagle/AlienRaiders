public abstract class BHAction
{
    protected BehaviorComponent m_Owner;

    public void Initialize(BehaviorComponent Owner)
    {
        m_Owner = Owner;
    }

    /** Returns true if we should continue */
    public virtual bool Start()
    {
        return true;
    }

    /** Returns true if we should continue */
    public virtual bool Update()
    {
        return true;
    }

    public virtual void Abort()
    { }
}
