using UnityEngine.Pool;

namespace NP
{
    public interface ISimplePoolObject
    {
        void OnGet();
        void OnRelease();
        void OnDestroy();
    }
    
    public class SimpleObjectPool<T> where T: class, ISimplePoolObject, new()
    {
        public SimpleObjectPool(uint capacity)
        {
            m_pool = new ObjectPool<T>(() => { return new T(); }, value=> value.OnGet(), value => value.OnRelease(),
                value => value.OnDestroy(), true, (int)capacity, (int)capacity);
        }

        public T Get()
        {
            return m_pool.Get();
        }

        public void Release(T value)
        {
            m_pool.Release(value);
        }

        public void Clear()
        {
            m_pool.Clear();
        }

        private ObjectPool<T> m_pool;
    }
}