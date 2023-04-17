public abstract class BHAction
{
    protected BehaviorComponent m_Owner;

    protected bool m_bFixedUpdate = false;
    public bool bFixedUpdate => m_bFixedUpdate;

    private bool m_bDone = false;
    public bool bDone => m_bDone;

    private bool m_bSucceeded;
    public bool bSucceeded => m_bSucceeded;

    public delegate void OnActionFinishedSignature(BHAction Action);
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
        m_bDone = true;
        m_bSucceeded = false;

        m_OnActionFinished?.Invoke(this);
    }

    /** Called by Behavior Component */
    public virtual void OnFinish()
    {
        m_bDone = true;
        m_bSucceeded = true;

        m_OnActionFinished?.Invoke(this);
    }

    public BHAction AddOnActionFinished(OnActionFinishedSignature Callback)
    {
        m_OnActionFinished += Callback;
        return this;
    }
}
