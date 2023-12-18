using System;
using System.Collections.Generic;

namespace NP
{
    public class SimpleCollectionPoolManager
    {
        public SimpleCollectionPoolManager(uint defaultCapacity)
        {
            m_poolDic = new Dictionary<Type, ISimpleCollectionPool>();
            m_poolDefaultCapacity = defaultCapacity;
        }

        public TCollection Get<TCollection, TValue>(bool createPool = true, uint capacity = 0)
            where TCollection : class, ICollection<TValue>, new()
        {
            Type t = typeof(TCollection);
            if (!m_poolDic.ContainsKey(t))
            {
                if (!createPool)
                    return null;

                var pool = new SimpleCollectionPool<TCollection, TValue>(capacity == 0 ? m_poolDefaultCapacity : capacity);
                m_poolDic.Add(t, pool);
            }

            return (m_poolDic[t] as SimpleCollectionPool<TCollection, TValue>)?.Get();
        }

        public void Release<TCollection, TValue>(TCollection collection) where TCollection : class, ICollection<TValue>, new()
        {
            (m_poolDic[typeof(TCollection)] as SimpleCollectionPool<TCollection, TValue>)?.Release(collection);
        }

        public void Clear()
        {
            foreach (var kv in m_poolDic)
            {
                kv.Value.Clear();
            }
        }

        private readonly Dictionary<Type, ISimpleCollectionPool> m_poolDic = null;
        private uint m_poolDefaultCapacity = 0;
    }
}