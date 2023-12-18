using System.Collections.Generic;
using UnityEngine.Pool;

namespace NP
{
    public interface ISimpleCollectionPool
    {
        void Clear();
    }
    
    public class SimpleCollectionPool<TCollection, TValue> : ISimpleCollectionPool where TCollection : class, ICollection<TValue>, new() 
    {
        public SimpleCollectionPool(uint capacity)
        {
            m_pool = new ObjectPool<TCollection>(() => { return new TCollection(); }, null, value => value.Clear(),
                null, true, (int)capacity, (int)capacity);
        }

        public TCollection Get()
        {
            return m_pool.Get();
        }

        public void Release(TCollection value)
        {
            if (value == null)
                return;
            
            m_pool.Release(value);
        }

        public void Clear()
        {
            m_pool.Clear();
        }

        private readonly ObjectPool<TCollection> m_pool;
    }
}