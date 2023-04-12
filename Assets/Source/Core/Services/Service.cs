public class Service<T> : CustomBehavior where T : Service<T>
{
    protected static T s_Instance;
    public static T Instance
    {
        get
        {
            if (!s_Instance)
            {
                s_Instance = ServiceLocator.Instance.Get<T>();
            }
            return s_Instance;
        }
    }
}
