using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace NP
{
    public interface ISimpleCollectionPool
    {
        void Clear();
    }

    public struct SimpleCollectionPoolItem<TCollection, TValue> : IDisposable where TCollection : class, ICollection<TValue>, new()
    {
        public TCollection Value;

        private SimpleCollectionPool<TCollection, TValue> m_pool;

        public SimpleCollectionPoolItem(SimpleCollectionPool<TCollection, TValue> pool, TCollection value)
        {
            m_pool = pool;
            Value = value;
        }

        public void Dispose()
        {
            m_pool.Release(Value);
        }
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

        public SimpleCollectionPoolItem<TCollection, TValue> GetPoolItem()
        {
            return new SimpleCollectionPoolItem<TCollection, TValue>(this, m_pool.Get());
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