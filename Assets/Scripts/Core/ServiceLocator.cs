using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : Singleton<ServiceLocator>
{
    private Dictionary<Type, MonoBehaviour> m_Services = new Dictionary<Type, MonoBehaviour>();

    public void Add<TType>(MonoBehaviour Service) where TType : MonoBehaviour
    {
        MonoBehaviour ExistingService;
        if (m_Services.TryGetValue(typeof(TType), out ExistingService))
        {
            Destroy(ExistingService.gameObject);
        }

        Service.transform.SetParent(transform);
        m_Services[typeof(TType)] = Service;
    }

    public void Add<TType, TService>()
        where TType : MonoBehaviour
        where TService : MonoBehaviour
    {
        MonoBehaviour ExistingService;
        if (m_Services.TryGetValue(typeof(TType), out ExistingService))
        {
            Destroy(ExistingService.gameObject);
        }

        CreateService<TType, TService>();
    }

    public TType Get<TType>() where TType : MonoBehaviour
    {
        MonoBehaviour Service;
        if (!m_Services.TryGetValue(typeof(TType), out Service))
        {
            Service = CreateService<TType, TType>();
        }

        return (TType)Service;
    }

    private TService CreateService<TType, TService>()
        where TType : MonoBehaviour
        where TService : MonoBehaviour
    {
        GameObject ServiceObject = new GameObject();
        ServiceObject.name = typeof(TService).Name;
        ServiceObject.transform.SetParent(transform);

        var Service = ServiceObject.AddComponent<TService>();
        m_Services[typeof(TType)] = Service;

        return Service;
    }
}
