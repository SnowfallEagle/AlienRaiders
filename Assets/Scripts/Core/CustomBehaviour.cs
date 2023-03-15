using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBehaviour : MonoBehaviour
{
    private Dictionary<Type, Type> m_CustomTypes = new Dictionary<Type, Type>();

    public void AddBaseCustomComponent(Type ComponentType, Type BaseType)
    {
        if (!m_CustomTypes.ContainsKey(ComponentType))
        {
            m_CustomTypes[ComponentType] = BaseType;
        }
    }

    public void AddBaseCustomComponent<TComponent, TBase>()
    {
        AddBaseCustomComponent(typeof(TComponent), typeof(TBase));
    }

    public void OverrideCustomComponent(Type ComponentType, Type OverrideType)
    {
        m_CustomTypes[ComponentType] = OverrideType;
    }

    public void OverrideCustomComponent<TComponent, TOverride>()
    {
        OverrideCustomComponent(typeof(TComponent), typeof(TOverride));
    }

    public Type GetCustomComponentType(Type ComponentType)
    {
        Type Type;
        return m_CustomTypes.TryGetValue(ComponentType, out Type) ? Type : ComponentType;
    }

    public Type GetCustomComponentType<T>()
    {
        return GetCustomComponentType(typeof(T));
    }

    public Component GetCustomComponent(Type ComponentType)
    {
        return GetComponent(GetCustomComponentType(ComponentType));
    }

    public T GetCustomComponent<T>() where T : Component
    {
        return (T)GetCustomComponent(typeof(T));
    }

    // TODO: Maybe redefine Unity's GetComponent() methods?

    // TODO: InitializeComponent() that create new component and check if we have one already, so we don't break unity
}
