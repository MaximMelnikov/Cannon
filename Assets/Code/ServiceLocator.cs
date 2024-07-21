using System.Collections.Generic;
using System;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            throw new Exception("Service already registered: " + type.Name);
        }
        _services[type] = service;
    }

    public static T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            throw new Exception("Service not found: " + type.Name);
        }
        return (T)service;
    }

    public static void UnregisterService<T>()
    {
        var type = typeof(T);
        if (!_services.Remove(type))
        {
            throw new Exception("Service not found: " + type.Name);
        }
    }
}