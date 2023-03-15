using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBehaviour : MonoBehaviour
{
    private Dictionary<Type, Type> m_CustomTypes = new Dictionary<Type, Type>();

    /** Registration of base components
        Base components registration don't override existing components.
        Can be freely called by base classes to set component base classes.
     */
    public void RegisterBaseComponent(Type ComponentType, Type BaseType)
    {
        if (!m_CustomTypes.ContainsKey(ComponentType))
        {
            m_CustomTypes[ComponentType] = BaseType;
        }
    }

    public void RegisterBaseComponent<TComponent, TBase>()
    {
        RegisterBaseComponent(typeof(TComponent), typeof(TBase));
    }

    /** Overridance of base components
        Can be called freely to override/register components
     */
    public void OverrideComponent(Type ComponentType, Type OverrideType)
    {
        m_CustomTypes[ComponentType] = OverrideType;
    }

    public void OverrideComponent<TComponent, TOverride>()
    {
        OverrideComponent(typeof(TComponent), typeof(TOverride));
    }

    /** Getters for registered component type
        Returns type of given ComponentType if component is not registered
    */
    public Type GetComponentType(Type ComponentType)
    {
        Type Type;
        return m_CustomTypes.TryGetValue(ComponentType, out Type) ? Type : ComponentType;
    }

    public Type GetComponentType<T>()
    {
        return GetComponentType(typeof(T));
    }

    /* Redefinitions of Unity's GetComponent() methods */
    new public Component GetComponent(Type ComponentType)
    {
        return base.GetComponent(GetComponentType(ComponentType));
    }

    new public T GetComponent<T>() where T : Component
    {
        return (T)GetComponent(typeof(T));
    }

    new public Component GetComponent(string ComponentTypeName)
    {
        return GetComponent(Type.GetType(ComponentTypeName));
    }

    // TODO: InitializeComponent() that create new component and check if we have one already, so we don't break unity
}

