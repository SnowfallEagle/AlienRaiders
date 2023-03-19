using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CustomBehavior : MonoBehaviour
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

        Component NewComponent = gameObject.AddComponent(RegisteredType);
        Assert.IsNotNull(NewComponent);

        return NewComponent;
    }

    protected T InitializeComponent<T>() where T : Component
    {
        return (T)InitializeComponent(typeof(T));
    }

    // Spawn MonoBehaviour by original
    protected static T SpawnInState<T>(T Component) where T : MonoBehaviour 
    {
        var Instance = Instantiate(Component);
        ReferenceInState(Instance);
        return Instance;
    }

    // Spawn GameObject by original
    protected static GameObject SpawnInState(GameObject Object) 
    {
        var Instance = Instantiate(Object);
        ReferenceInState(Instance);
        return Instance;
    }

    // Spawn empty GameObject
    protected static GameObject SpawnInState() 
    {
        var Object = new GameObject();
        ReferenceInState(Object);
        return Object;
    }

    // Spawn Component by Type
    protected static Component SpawnInState(Type ComponentType)
    {
        var GameObject = SpawnInState();
        var Component = GameObject.AddComponent(ComponentType);
        return Component;
    }

    // Spawn Component
    protected static T SpawnInState<T>() where T : MonoBehaviour
    {
        return (T)SpawnInState(typeof(T));
    }

    // Spawn Component by Type
    protected static T SpawnInState<T>(Type ComponentType) where T : MonoBehaviour
    {
        return (T)SpawnInState(ComponentType);
    }

    // Reference Component in GameState
    protected static void ReferenceInState(MonoBehaviour Object)
    {
        ServiceLocator.Instance.Get<GameStateMachine>().CurrentState.ReferenceObject(Object);
    }

    // Reference GameObject in GameState
    protected static void ReferenceInState(GameObject Object)
    {
        ServiceLocator.Instance.Get<GameStateMachine>().CurrentState.ReferenceObject(Object);
    }
}

