using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                s_Instance.OnInstantiation();
            }
            return s_Instance;
        }
    }

    /** Derived classes should put their code that should be before Start() in MonoBehaviour
        Start() don't called immediately, so Service should use this method
    */
    protected virtual void OnInstantiation()
    { }
}
