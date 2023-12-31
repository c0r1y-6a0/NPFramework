using System.Collections.Generic;
using System;

namespace NP
{
    
    public static class EventManager
    {
        public static void AddListener<T>(Action<T> listener) where T : struct
        {
            Type eventType = typeof(T);
            if (!m_eventListerns.ContainsKey(eventType))
            {
                m_eventListerns.Add(eventType, new List<Delegate>());
            }

            m_eventListerns[eventType].Add(listener);
        }
        
        public static void RemoveListener<T>(Action<T> listener) where T : struct
        {
            Type eventType = typeof(T);
            if (!m_eventListerns.ContainsKey(eventType))
            {
                return;
            }

            m_eventListerns[eventType].Remove(listener);
        }
        
        public static void Raise<T>(T eventObj) where T : struct
        {
            Type eventType = typeof(T);
            if (!m_eventListerns.ContainsKey(eventType))
            {
                return;
            }

            for (int i = m_eventListerns[eventType].Count - 1; i >= 0; i--)
            {
                ((Action<T>)m_eventListerns[eventType][i])(eventObj);
            }
        }
        
        private static Dictionary<Type, List<Delegate>> m_eventListerns= new Dictionary<Type, List<Delegate>>();
    }
}