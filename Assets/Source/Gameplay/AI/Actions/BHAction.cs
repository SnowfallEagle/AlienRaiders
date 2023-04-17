public abstract class BHAction
{
    protected BehaviorComponent m_Owner;

    protected bool m_bFixedUpdate = false;
    public bool bFixedUpdate => m_bFixedUpdate;

    public delegate void OnActionFinishedSignature(BHAction Action, bool bSucceeded);
    private OnActionFinishedSignature m_OnActionFinished;

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

    /** Called by Behavior Component */
    public virtual void OnAbort()
    {
        m_OnActionFinished?.Invoke(this, false);
    }

    /** Called by Behavior Component */
    public virtual void OnFinish()
    {
        m_OnActionFinished?.Invoke(this, true);
    }

    public BHAction AddOnActionFinished(OnActionFinishedSignature Callback)
    {
        m_OnActionFinished += Callback;
        return this;
    }
}
