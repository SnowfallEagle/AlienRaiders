using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service<T> : CustomBehavior where T : MonoBehaviour
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
