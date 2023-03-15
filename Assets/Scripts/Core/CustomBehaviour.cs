using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    /** Component initializaiton
        It's safe to call it multiple times.
        Only 1 component with this type'll be added to game object.
        If component is not registered then ComponentType'll be used to initialize.
    */
    protected Component InitializeComponent(Type ComponentType)
    {
        Type RegisteredType = GetComponentType(ComponentType);

        Component ExistingComponent = GetComponent(RegisteredType);
        if (ExistingComponent)
        {
            return ExistingComponent;
        }

        return gameObject.AddComponent(RegisteredType);
    }

    protected T InitializeComponent<T>() where T : Component
    {
        return (T)InitializeComponent(typeof(T));
    }
}

