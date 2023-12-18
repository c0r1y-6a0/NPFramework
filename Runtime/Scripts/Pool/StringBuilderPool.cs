using System;
using System.Text;
using UnityEngine.Pool;

namespace NP
{
    public class StringBuilderPool
    {
        private const int c_ShortSize = 32;
        private const int c_MediumSize = 128;
        private const int c_LongSize = 256;

        public StringBuilder Short => m_shortPool.Get();
        public StringBuilder Medium => m_mediumPool.Get();
        public StringBuilder Long => m_longPool.Get();

        public StringBuilderPool(int maxCapacity)
        {
            m_shortPool = new ObjectPool<StringBuilder>(() => new StringBuilder(c_ShortSize), null,
                (StringBuilder sb) => sb.Clear(), null, true, maxCapacity, maxCapacity);
            m_mediumPool = new ObjectPool<StringBuilder>(() => new StringBuilder(c_MediumSize), null,
                (StringBuilder sb) => sb.Clear(), null, true, maxCapacity, maxCapacity);
            m_longPool = new ObjectPool<StringBuilder>(() => new StringBuilder(c_LongSize), null,
                (StringBuilder sb) => sb.Clear(), null, true, maxCapacity, maxCapacity);
        }

        public StringBuilder Get(int length)
        {
            if (length >= c_LongSize)
                return m_longPool.Get();
            if (length >= c_MediumSize)
                return m_mediumPool.Get();
            return m_shortPool.Get();
        }

        public void Release(StringBuilder sb)
        {
            if (sb.Length >= c_LongSize)
                m_longPool.Release(sb);
            else if (sb.Length >= c_MediumSize)
                m_mediumPool.Release(sb);
            else
                m_shortPool.Release(sb);
        }

        public void Clear()
        {
            m_shortPool.Clear();
            m_mediumPool.Clear();
            m_longPool.Clear();
        }

        private readonly ObjectPool<StringBuilder> m_shortPool = null;
        private readonly ObjectPool<StringBuilder> m_mediumPool = null;
        private readonly ObjectPool<StringBuilder> m_longPool = null;
    }
}