using System;
using System.Collections.Generic;
using UnityEngine;

namespace NP
{
    public static class ServiceLocator
    {
        public static void RegisterService<T>(T service) where T : class
        {
            if (s_services.ContainsKey(typeof(T)))
            {
                Debug.Assert(false);
                return;
            }

            s_services.Add(typeof(T), service);
        }

        public static T GetService<T>() where T : class
        {
            object result = null;
            s_services.TryGetValue(typeof(T), out result);
            return (T)result;
        }

        private static Dictionary<Type, object> s_services = new();
    }
}
